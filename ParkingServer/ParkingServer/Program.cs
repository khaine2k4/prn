using System.Net;
using System.Net.Sockets;
using System.Text;

// --- CẤU HÌNH HỆ THỐNG ---
int capacity = 100;
int currentCars = 0; // Tổng số slot đã bị chiếm
long unitPrice = 500000; // 500k/giờ/slot
object lockObject = new object();

// Lưu trữ thông tin xe
Dictionary<string, DateTime> parkingTime = new Dictionary<string, DateTime>();
Dictionary<string, int> carSlots = new Dictionary<string, int>();

TcpListener server = new TcpListener(IPAddress.Any, 5000);
server.Start();

Console.Clear();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("====================================================");
Console.WriteLine("   SERVER QUẢN LÝ BÃI XE THÔNG MINH (BIG/SMALL)     ");
Console.WriteLine("====================================================");
Console.ResetColor();
Console.WriteLine($"Trạng thái: Cổng 5000 | Sức chứa: {capacity} slot");

while (true)
{
    TcpClient client = await server.AcceptTcpClientAsync();
    _ = Task.Run(() => HandleClient(client));
}

void HandleClient(TcpClient client)
{
    string clientIP = client.Client.RemoteEndPoint.ToString();
    try
    {
        using NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (true)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead == 0) break;

            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
            string[] parts = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2) continue;

            string gateId = parts[0];
            string command = parts[1].ToUpper();
            string response = "";

            lock (lockObject)
            {
                if (command == "ENTER")
                {
                    // Kiểm tra cú pháp: ENTER [TYPE] [ID]
                    if (parts.Length < 4)
                    {
                        response = "ERR|Sai cú pháp! Dùng: ENTER [SMALL/BIG] [MaXe]";
                    }
                    else
                    {
                        string type = parts[2].ToUpper();
                        string carId = parts[3];
                        int requiredSlots = (type == "BIG") ? 3 : 1;

                        if (parkingTime.ContainsKey(carId))
                            response = "ERR|Xe này đã ở trong bãi!";
                        else if (currentCars + requiredSlots > capacity)
                            response = $"ERR|Hết chỗ! Cần {requiredSlots} slot nhưng chỉ còn {capacity - currentCars}";
                        else
                        {
                            currentCars += requiredSlots;
                            parkingTime.Add(carId, DateTime.Now);
                            carSlots.Add(carId, requiredSlots);
                            response = $"OK|IN|{currentCars}/{capacity}|{DateTime.Now:HH:mm:ss}";
                        }
                    }
                }
                else if (command == "EXIT")
                {
                    // Kiểm tra cú pháp: EXIT [ID]
                    if (parts.Length < 3)
                    {
                        response = "ERR|Sai cú pháp! Dùng: EXIT [MaXe]";
                    }
                    else
                    {
                        string carId = parts[2];
                        if (parkingTime.ContainsKey(carId))
                        {
                            int slotsUsed = carSlots[carId];
                            currentCars -= slotsUsed;

                            DateTime timeIn = parkingTime[carId];
                            DateTime timeOut = DateTime.Now;
                            double hours = (timeOut - timeIn).TotalHours;
                            if (hours < 0.01) hours = 0.02; // Tối thiểu để test nhanh

                            long fee = (long)(hours * unitPrice * slotsUsed);

                            parkingTime.Remove(carId);
                            carSlots.Remove(carId);

                            response = $"OK|OUT|{currentCars}/{capacity}|{timeIn:HH:mm:ss}|{timeOut:HH:mm:ss}|{fee:N0}";
                        }
                        else response = "ERR|Không tìm thấy mã xe này!";
                    }
                }
                else response = "ERR|Lệnh không hợp lệ!";
            }

            byte[] resData = Encoding.UTF8.GetBytes(response);
            stream.Write(resData, 0, resData.Length);
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {gateId}: {request} -> Slot hien tai: {currentCars}/{capacity}");
        }
    }
    catch { /* Ngắt kết nối */ }
}
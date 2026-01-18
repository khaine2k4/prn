using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.Write("Nhập tên cổng (ví dụ: Gate1): ");
string gateId = Console.ReadLine() ?? "UnknownGate";

try
{
    using TcpClient client = new TcpClient("127.0.0.1", 5000);
    using NetworkStream stream = client.GetStream();

    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"=== HỆ THỐNG CỔNG: {gateId.ToUpper()} ===");
    Console.WriteLine("QUY ĐỊNH: XE LỚN (BIG) = 3 SLOT | XE NHỎ (SMALL) = 1 SLOT");
    Console.WriteLine("GIÁ VÉ: 500.000 VNĐ / GIỜ / SLOT");
    Console.WriteLine("----------------------------------------------------");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("CÚ PHÁP:");
    Console.WriteLine("  - Xe vào: ENTER [SMALL/BIG] [MãXe]");
    Console.WriteLine("  - Xe ra : EXIT [MãXe]");
    Console.WriteLine("  - Thoát : QUIT");
    Console.ResetColor();

    while (true)
    {
        Console.Write($"\n{gateId} > ");
        string input = Console.ReadLine();
        if (string.IsNullOrEmpty(input)) continue;
        if (input.ToUpper() == "QUIT") break;

        // Gửi dữ liệu qua Server
        byte[] data = Encoding.UTF8.GetBytes($"{gateId} {input}");
        stream.Write(data, 0, data.Length);

        // Nhận phản hồi
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        // Xử lý hiển thị dựa trên phản hồi của Server
        string[] res = response.Split('|');
        if (res[0] == "OK")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (res[1] == "IN")
                Console.WriteLine($">> THÀNH CÔNG: Xe đã vào. Slot hiện tại: {res[2]}. Lúc: {res[3]}");
            else
                Console.WriteLine($">> THÀNH CÔNG: Xe đã ra. Slot còn lại: {res[2]}.\n   Vào: {res[3]} - Ra: {res[4]}\n   TỔNG PHÍ: {res[5]} VNĐ");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($">> THÔNG BÁO: {res[1]}");
        }
        Console.ResetColor();
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Lỗi kết nối: " + ex.Message);
    Console.ResetColor();
    Console.ReadKey();
}
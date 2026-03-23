USE [HealthcareDB];
GO

-- 1. Thêm Dữ liệu Bác sĩ (Doctors)
-- Không set cứng ID nữa, để CSDL tự tăng
INSERT INTO [Doctors] ([DoctorName], [Specialty], [LicenseNumber], [MaxPatients], [Active])
VALUES 
(N'Dr. Emily Davis', N'Neurology', 'L901234', 5, 1),
(N'Dr. Michael Brown', N'Pediatrics', 'L567890', 20, 1),
(N'Dr. Sarah Miller', N'Dermatology', 'L112233', 12, 1);
GO

-- 2. Thêm Dữ liệu Bệnh nhân (Users)
-- Password 'patient123' có mã hash sha256 là: 7fa3b767c460b54a2be4d49030b349c7f6b9c9e53bfa3197607a726190abf8b8
INSERT INTO [Users] ([FullName], [Email], [Password], [Role], [CreatedAt])
VALUES 
(N'Nguyễn Văn Bệnh Nhân', 'patient1@healthcare.com', '7fa3b767c460b54a2be4d49030b349c7f6b9c9e53bfa3197607a726190abf8b8', 'Patient', GETDATE()),
(N'Trần Thị Khám Bệnh', 'patient2@healthcare.com', '7fa3b767c460b54a2be4d49030b349c7f6b9c9e53bfa3197607a726190abf8b8', 'Patient', GETDATE());
GO

-- 3. Thêm Lịch hẹn (Appointments)
-- Lấy động ID của Patient và Doctor vừa thêm (hoặc đã có sẵn) để khỏi bị lỗi khóa ngoại
DECLARE @Patient1_ID INT = (SELECT TOP 1 ID FROM [Users] WHERE Email = 'patient1@healthcare.com');
DECLARE @Patient2_ID INT = (SELECT TOP 1 ID FROM [Users] WHERE Email = 'patient2@healthcare.com');

DECLARE @Doctor1_ID INT = (SELECT TOP 1 ID FROM [Doctors] WHERE LicenseNumber = 'L123456'); -- Bác sĩ đã được tạo tự động
DECLARE @DoctorNew_ID INT = (SELECT TOP 1 ID FROM [Doctors] WHERE LicenseNumber = 'L901234'); -- Bác sĩ vừa thêm ở trên

INSERT INTO [Appointments] ([PatientID], [DoctorID], [AppointmentDate], [BookingDate], [IsCancelled])
VALUES 
(@Patient1_ID, @Doctor1_ID, DATEADD(day, 1, GETDATE()), GETDATE(), 0), 
(@Patient1_ID, @DoctorNew_ID, DATEADD(day, 2, GETDATE()), GETDATE(), 0), 
(@Patient2_ID, @Doctor1_ID, DATEADD(day, 1, GETDATE()), GETDATE(), 0);
GO

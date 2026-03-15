using CareBusiness.Entities;

namespace CareRepositories.Interfaces
{
    public interface IDoctorRepository
    {
        List<Doctor> GetAll();
        Doctor? GetById(int id);
        List<Doctor> Search(string? keyword);
        bool LicenseNumberExists(string licenseNumber, int excludeId = 0);
        int GetBookingCountForDate(int doctorId, DateTime date);
        void Add(Doctor doctor);
        void Update(Doctor doctor);
    }
}

using CareBusiness.Entities;
using CareDataAccess;
using CareRepositories.Interfaces;

namespace CareRepositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DoctorDAO _dao;

        public DoctorRepository(DoctorDAO dao)
        {
            _dao = dao;
        }

        public List<Doctor> GetAll() => _dao.GetAll();
        public Doctor? GetById(int id) => _dao.GetById(id);
        public List<Doctor> Search(string? keyword) => _dao.Search(keyword);
        public bool LicenseNumberExists(string licenseNumber, int excludeId = 0) => _dao.LicenseNumberExists(licenseNumber, excludeId);
        public int GetBookingCountForDate(int doctorId, DateTime date) => _dao.GetBookingCountForDate(doctorId, date);
        public void Add(Doctor doctor) => _dao.Add(doctor);
        public void Update(Doctor doctor) => _dao.Update(doctor);
    }
}

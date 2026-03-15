using CareBusiness;
using CareBusiness.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareDataAccess
{
    public class DoctorDAO
    {
        private readonly HealthcareContext _context;

        public DoctorDAO(HealthcareContext context)
        {
            _context = context;
        }

        public List<Doctor> GetAll()
        {
            return _context.Doctors.ToList();
        }

        public Doctor? GetById(int id)
        {
            return _context.Doctors.FirstOrDefault(d => d.ID == id);
        }

        /// <summary>
        /// Req 3: Search by DoctorName, Specialty, or LicenseNumber
        /// </summary>
        public List<Doctor> Search(string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _context.Doctors.Where(d => d.Active).ToList();

            string kw = keyword.Trim().ToLower();
            return _context.Doctors
                .Where(d => d.Active && (
                    d.DoctorName.ToLower().Contains(kw) ||
                    d.Specialty.ToLower().Contains(kw) ||
                    d.LicenseNumber.ToLower().Contains(kw)
                ))
                .ToList();
        }

        public bool LicenseNumberExists(string licenseNumber, int excludeId = 0)
        {
            return _context.Doctors.Any(d => d.LicenseNumber == licenseNumber && d.ID != excludeId);
        }

        /// <summary>
        /// Req 4: Count how many active (non-cancelled) appointments a doctor has for a given date
        /// </summary>
        public int GetBookingCountForDate(int doctorId, DateTime date)
        {
            return _context.Appointments
                .Count(a => a.DoctorID == doctorId
                         && !a.IsCancelled
                         && a.AppointmentDate.Date == date.Date);
        }

        public void Add(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }

        public void Update(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            _context.SaveChanges();
        }
    }
}

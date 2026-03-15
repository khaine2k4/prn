using CareBusiness;
using CareBusiness.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareDataAccess
{
    public class AppointmentDAO
    {
        private readonly HealthcareContext _context;

        public AppointmentDAO(HealthcareContext context)
        {
            _context = context;
        }

        public List<Appointment> GetAll()
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToList();
        }

        public List<Appointment> GetByPatient(int patientId)
        {
            return _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientID == patientId)
                .ToList();
        }

        /// <summary>
        /// Req 4: Check if patient already booked this doctor for the same day
        /// </summary>
        public bool HasDuplicateBooking(int patientId, int doctorId, DateTime appointmentDate)
        {
            return _context.Appointments.Any(a =>
                a.PatientID == patientId &&
                a.DoctorID == doctorId &&
                !a.IsCancelled &&
                a.AppointmentDate.Date == appointmentDate.Date);
        }

        public void Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }

        public void Cancel(int appointmentId)
        {
            var appt = _context.Appointments.FirstOrDefault(a => a.ID == appointmentId);
            if (appt != null)
            {
                appt.IsCancelled = true;
                _context.SaveChanges();
            }
        }
    }
}

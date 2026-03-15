using CareBusiness.Entities;
using CareDataAccess;
using CareRepositories.Interfaces;

namespace CareRepositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDAO _dao;

        public AppointmentRepository(AppointmentDAO dao)
        {
            _dao = dao;
        }

        public List<Appointment> GetAll() => _dao.GetAll();
        public List<Appointment> GetByPatient(int patientId) => _dao.GetByPatient(patientId);
        public bool HasDuplicateBooking(int patientId, int doctorId, DateTime appointmentDate) => _dao.HasDuplicateBooking(patientId, doctorId, appointmentDate);
        public void Add(Appointment appointment) => _dao.Add(appointment);
        public void Cancel(int appointmentId) => _dao.Cancel(appointmentId);
    }
}

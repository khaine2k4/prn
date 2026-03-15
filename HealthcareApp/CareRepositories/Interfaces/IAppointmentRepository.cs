using CareBusiness.Entities;

namespace CareRepositories.Interfaces
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAll();
        List<Appointment> GetByPatient(int patientId);
        bool HasDuplicateBooking(int patientId, int doctorId, DateTime appointmentDate);
        void Add(Appointment appointment);
        void Cancel(int appointmentId);
    }
}

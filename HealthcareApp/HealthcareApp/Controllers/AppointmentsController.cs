using Microsoft.AspNetCore.Mvc;
using CareRepositories.Interfaces;
using CareBusiness.Entities;
using HealthcareApp.Filters;

namespace HealthcareApp.Controllers
{
    [SessionAuth]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentsController(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        // Req 4: Appointment Booking
        [SessionAuth("Patient")]
        [HttpGet]
        public IActionResult Book(int doctorId)
        {
            var doctor = _doctorRepository.GetById(doctorId);
            if (doctor == null || !doctor.Active)
            {
                return NotFound();
            }

            ViewBag.Doctor = doctor;
            return View();
        }

        [SessionAuth("Patient")]
        [HttpPost]
        public IActionResult Book(int doctorId, DateTime appointmentDate)
        {
            var doctor = _doctorRepository.GetById(doctorId);
            var currentUser = ViewBag.CurrentUser as User;

            if (doctor == null || !doctor.Active)
            {
                ModelState.AddModelError("", "Doctor does not exist or is not active.");
            }
            else
            {
                // Validation 1: Max patients limit
                int currentBookings = _doctorRepository.GetBookingCountForDate(doctorId, appointmentDate);
                if (currentBookings >= doctor.MaxPatients)
                {
                    ModelState.AddModelError("", $"Dr. {doctor.DoctorName} has reached the booking limit for {appointmentDate.ToShortDateString()}.");
                }

                // Validation 2: Duplicate booking check
                if (currentUser != null && _appointmentRepository.HasDuplicateBooking(currentUser.ID, doctorId, appointmentDate))
                {
                    ModelState.AddModelError("", "You have already booked this doctor for this day.");
                }
            }

            if (ModelState.IsValid && currentUser != null)
            {
                var appointment = new Appointment
                {
                    PatientID = currentUser.ID,
                    DoctorID = doctorId,
                    AppointmentDate = appointmentDate,
                    BookingDate = DateTime.Now,
                    IsCancelled = false
                };

                _appointmentRepository.Add(appointment);
                return RedirectToAction("MyAppointments");
            }

            ViewBag.Doctor = doctor;
            return View();
        }

        [SessionAuth("Patient")]
        public IActionResult MyAppointments()
        {
            var currentUser = ViewBag.CurrentUser as User;
            if (currentUser == null) return RedirectToAction("Login", "Auth");

            var appointments = _appointmentRepository.GetByPatient(currentUser.ID);
            return View(appointments);
        }

        [SessionAuth("Admin")]
        public IActionResult AllAppointments()
        {
            var appointments = _appointmentRepository.GetAll();
            return View(appointments);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using CareRepositories.Interfaces;
using CareBusiness.Entities;
using HealthcareApp.Filters;

namespace HealthcareApp.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorsController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        // Req 3: Doctor Search
        public IActionResult Index(string? keyword)
        {
            var doctors = _doctorRepository.Search(keyword);
            ViewBag.Keyword = keyword;
            return View(doctors);
        }

        // Req 5: Admin CRUD (Add Doctors)
        [SessionAuth("Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [SessionAuth("Admin")]
        [HttpPost]
        public IActionResult Create(Doctor doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor.DoctorName) || string.IsNullOrWhiteSpace(doctor.Specialty))
            {
                ModelState.AddModelError("", "DoctorName and Specialty cannot be empty.");
            }

            if (_doctorRepository.LicenseNumberExists(doctor.LicenseNumber))
            {
                ModelState.AddModelError("LicenseNumber", "LicenseNumber must be unique.");
            }

            if (doctor.MaxPatients < 0)
            {
                ModelState.AddModelError("MaxPatients", "MaxPatients must be >= 0.");
            }

            if (ModelState.IsValid)
            {
                doctor.Active = true;
                _doctorRepository.Add(doctor);
                return RedirectToAction("Index");
            }

            return View(doctor);
        }
    }
}

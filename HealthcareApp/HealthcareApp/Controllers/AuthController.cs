using Microsoft.AspNetCore.Mvc;
using CareRepositories.Interfaces;
using CareBusiness.Entities;
using System.Security.Cryptography;
using System.Text;

namespace HealthcareApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;

        public AuthController(IUserRepository userRepository, ISessionRepository sessionRepository)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _userRepository.GetByEmail(email);

            // Simple password check (for production, use BCrypt/Argon2)
            if (user != null && user.Password == HashPassword(password))
            {
                var sessionId = Guid.NewGuid().ToString();
                var session = new Session
                {
                    SessionID = sessionId,
                    UserID = user.ID,
                    Role = user.Role,
                    ExpiresAt = DateTime.Now.AddHours(2)
                };

                _sessionRepository.Add(session);

                // Set cookie
                Response.Cookies.Append("SessionID", sessionId, new CookieOptions
                {
                    Expires = session.ExpiresAt,
                    HttpOnly = true
                });

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Doctors");
                }
                else
                {
                    return RedirectToAction("Index", "Doctors"); // Or search page
                }
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }

        public IActionResult Logout()
        {
            var sessionId = Request.Cookies["SessionID"];
            if (!string.IsNullOrEmpty(sessionId))
            {
                _sessionRepository.Delete(sessionId);
                Response.Cookies.Delete("SessionID");
            }
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}

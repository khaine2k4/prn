using CareBusiness;
using CareBusiness.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareDataAccess
{
    public class UserDAO
    {
        private readonly HealthcareContext _context;

        public UserDAO(HealthcareContext context)
        {
            _context = context;
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User? GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.ID == id);
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }
    }
}

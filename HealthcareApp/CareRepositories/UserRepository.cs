using CareBusiness.Entities;
using CareDataAccess;
using CareRepositories.Interfaces;

namespace CareRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO _dao;

        public UserRepository(UserDAO dao)
        {
            _dao = dao;
        }

        public User? GetByEmail(string email) => _dao.GetByEmail(email);
        public User? GetById(int id) => _dao.GetById(id);
        public bool EmailExists(string email) => _dao.EmailExists(email);
        public void Add(User user) => _dao.Add(user);
        public List<User> GetAll() => _dao.GetAll();
    }
}

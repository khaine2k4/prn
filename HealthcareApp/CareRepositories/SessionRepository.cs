using CareBusiness.Entities;
using CareDataAccess;
using CareRepositories.Interfaces;

namespace CareRepositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly SessionDAO _dao;

        public SessionRepository(SessionDAO dao)
        {
            _dao = dao;
        }

        public Session? GetById(string sessionId) => _dao.GetById(sessionId);
        public Session? GetByIdWithUser(string sessionId) => _dao.GetByIdWithUser(sessionId);
        public bool IsValid(string sessionId) => _dao.IsValid(sessionId);
        public void Add(Session session) => _dao.Add(session);
        public void Delete(string sessionId) => _dao.Delete(sessionId);
        public void DeleteExpired() => _dao.DeleteExpired();
    }
}

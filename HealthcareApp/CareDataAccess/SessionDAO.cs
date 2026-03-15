using CareBusiness;
using CareBusiness.Entities;

namespace CareDataAccess
{
    public class SessionDAO
    {
        private readonly HealthcareContext _context;

        public SessionDAO(HealthcareContext context)
        {
            _context = context;
        }

        public Session? GetById(string sessionId)
        {
            return _context.Sessions
                .FirstOrDefault(s => s.SessionID == sessionId);
        }

        public Session? GetByIdWithUser(string sessionId)
        {
            return _context.Sessions
                .FirstOrDefault(s => s.SessionID == sessionId && s.ExpiresAt > DateTime.Now);
        }

        public void Add(Session session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }

        public void Delete(string sessionId)
        {
            var session = _context.Sessions.FirstOrDefault(s => s.SessionID == sessionId);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                _context.SaveChanges();
            }
        }

        public bool IsValid(string sessionId)
        {
            return _context.Sessions.Any(s => s.SessionID == sessionId && s.ExpiresAt > DateTime.Now);
        }

        public void DeleteExpired()
        {
            var expired = _context.Sessions.Where(s => s.ExpiresAt <= DateTime.Now).ToList();
            _context.Sessions.RemoveRange(expired);
            _context.SaveChanges();
        }
    }
}

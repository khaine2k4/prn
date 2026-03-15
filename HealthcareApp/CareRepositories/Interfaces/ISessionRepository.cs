using CareBusiness.Entities;

namespace CareRepositories.Interfaces
{
    public interface ISessionRepository
    {
        Session? GetById(string sessionId);
        Session? GetByIdWithUser(string sessionId);
        bool IsValid(string sessionId);
        void Add(Session session);
        void Delete(string sessionId);
        void DeleteExpired();
    }
}

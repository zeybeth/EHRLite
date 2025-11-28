using EHRLite.Models;

namespace EHRLite.Repository
{
    public interface IVisitRepository : IRepository<Visit>
    {
        void Update(Visit obj);
        void Save();
    }
}
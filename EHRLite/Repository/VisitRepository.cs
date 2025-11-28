using EHRLite.Data;
using EHRLite.Models;

namespace EHRLite.Repository
{
    public class VisitRepository : Repository<Visit>, IVisitRepository
    {
        private readonly ApplicationDbContext _db;

        public VisitRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Visit obj)
        {
            _db.Visits.Update(obj);
        }
    }
}
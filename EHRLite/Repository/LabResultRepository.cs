using EHRLite.Data;
using EHRLite.Models;

namespace EHRLite.Repository
{
    public class LabResultRepository : Repository<LabResult>, ILabResultRepository
    {
        private readonly ApplicationDbContext _db;

        public LabResultRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(LabResult obj)
        {
            _db.LabResults.Update(obj);
        }
    }
}
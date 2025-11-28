using EHRLite.Data;
using EHRLite.Models;

namespace EHRLite.Repository
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly ApplicationDbContext _db;

        public PatientRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Patient obj)
        {
            _db.Patients.Update(obj);
        }
    }
}
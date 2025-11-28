using EHRLite.Models;

namespace EHRLite.Repository
{
    public interface ILabResultRepository : IRepository<LabResult>
    {
        void Update(LabResult obj);
        void Save();
    }
}
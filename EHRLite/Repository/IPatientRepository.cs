using EHRLite.Models;

namespace EHRLite.Repository
{
    // Genel IRepository özelliklerini miras alıyoruz + Kendine özel Update ve Save metodu ekliyoruz
    public interface IPatientRepository : IRepository<Patient>
    {
        void Update(Patient obj);
        void Save();
    }
}
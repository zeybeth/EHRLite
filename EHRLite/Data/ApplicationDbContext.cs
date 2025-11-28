using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EHRLite.Models;
using Microsoft.EntityFrameworkCore;

namespace EHRLite.Data
{
    // DbContext sınıfından miras alıyoruz
    public class ApplicationDbContext : IdentityDbContext //DbContext vardı değiştirdik
    {
        // Yapıcı metod (Constructor): Ayarları program.cs'ten alır
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Modellerimizi Veritabanı Tablolarına dönüştürüyoruz
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<LabResult> LabResults { get; set; }
    }
}
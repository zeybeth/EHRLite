using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // Bu kütüphane gerekli
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRLite.Models
{
    public class Visit
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Muayene tarihi zorunludur.")]
        [Display(Name = "Muayene Tarihi")]
        public DateTime VisitDate { get; set; } = DateTime.Now;

        [Display(Name = "Doktor Notları")]
        public string? DoctorNotes { get; set; } // Not yazmak zorunlu değilse ? koyabiliriz

        [Display(Name = "Tanı / Teşhis")]
        public string? Diagnosis { get; set; } // Tanı zorunlu değilse ? koyabiliriz

        // İLİŞKİ AYARLARI (Foreign Key)
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        [ValidateNever] // Doğrulama yapma (Validasyon hatasını engeller)
        public Patient? Patient { get; set; }

        [ValidateNever] // Doğrulama yapma
        public ICollection<LabResult>? LabResults { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace EHRLite.Models
{
    public class Patient
    {
        [Key] // Bu alanın tablonun anahtarı (ID) olduğunu belirtir
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim alanı boş bırakılamaz.")] // Zorunlu alan
        [Display(Name = "Ad")] // Ekranda görünecek etiket
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyisim alanı boş bırakılamaz.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)] // Sadece tarih tutsun, saat olmasın
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }

        [Display(Name = "Telefon")]
        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // Bir hasta sistemde ne zaman oluşturuldu?
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
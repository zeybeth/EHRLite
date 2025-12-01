using System.ComponentModel.DataAnnotations;

namespace EHRLite.Models
{
    public class Patient
    {
        [Key] // Bu alanın tablonun anahtarı (ID) olduğunu belirtir
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name space cannot be empty")] // Zorunlu alan
        [Display(Name = "First Name")] // Ekranda görünecek etiket
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name space cannot be empty")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)] // Sadece tarih tutsun, saat olmasın
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Phone")]
        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // Bir hasta sistemde ne zaman oluşturuldu?
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
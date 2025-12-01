using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHRLite.Models
{
    public class LabResult
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Test Name is required")]
        [Display(Name = "Test Name")] // Örn: Hemoglobin, Şeker
        public string TestName { get; set; }

        [Required]
        [Display(Name = "Value")] // Grafik için kullanılacak sayısal veri
        public double Value { get; set; }

        [Display(Name = "Unit")] // Örn: mg/dL
        public string Unit { get; set; }

        [Display(Name = "Min. Reference")]
        public double? MinReference { get; set; } // Referans aralığı alt sınır

        [Display(Name = "Max. Reference")]
        public double? MaxReference { get; set; } // Referans aralığı üst sınır

        public DateTime ResultDate { get; set; } = DateTime.Now;

        // Hangi muayeneye ait?
        public int VisitId { get; set; }

        [ForeignKey("VisitId")]
        public Visit? Visit { get; set; }
    }
}
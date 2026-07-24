using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProjesi.Entities
{
    public class UrunResmi
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string DosyaAdi { get; set; } 
        public bool AnaResimMi { get; set; }
        public int Sira { get; set; } 
        public int UrunId { get; set; }
        [ForeignKey("UrunId")]
        public Urun Urun { get; set; } = null!;
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProjesi.Entities 
{
    public class Favori
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int UrunId { get; set; }
        [ForeignKey("UrunId")]
        public virtual Urun Urun { get; set; }
    }
}
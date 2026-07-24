using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ECommerceProjesi.Entities
{
    public class Yorum
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yorum yazmalısınız.")]
        public string Icerik { get; set; }
        [Range(1, 5)]
        public int Puan { get; set; }
        public DateTime Tarih { get; set; } = DateTime.Now;
        public int UrunId { get; set; }
        [ForeignKey("UrunId")]
        public virtual Urun Urun { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; }
    }
}
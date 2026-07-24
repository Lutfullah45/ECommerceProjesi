using ECommerceProjesi.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECommerceProjesi.WebUI.Areas.Admin.Models
{
    public class UrunResimIndexViewModel
    {
        public Urun Urun { get; set; } = new Urun();
        public List<UrunResmi> Resimler { get; set; } = new List<UrunResmi>();
        public IFormFile? Dosya { get; set; }
        public bool AnaResimMi { get; set; } = false; 
        public int Sira { get; set; } = 1; 
    }
}
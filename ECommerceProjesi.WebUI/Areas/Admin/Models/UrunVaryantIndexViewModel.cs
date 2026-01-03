using ECommerceProjesi.Entities;
using System.Collections.Generic;

namespace ECommerceProjesi.WebUI.Areas.Admin.Models
{
    public class UrunVaryantIndexViewModel
    {
        public Urun Urun { get; set; } = null!;
        public List<UrunVaryanti> Varyantlar { get; set; } = new List<UrunVaryanti>();
    }
}
using ECommerceProjesi.Entities;
using System.Collections.Generic;

namespace ECommerceProjesi.WebUI.ViewModels
{
    public class UrunDetayViewModel
    {
        public Urun Urun { get; set; }
        public List<UrunResmi> Resimler { get; set; }
        public List<UrunVaryanti> Varyantlar { get; set; }
        public List<Yorum> Yorumlar { get; set; }
        public double OrtalamaPuan { get; set; }
    }
}
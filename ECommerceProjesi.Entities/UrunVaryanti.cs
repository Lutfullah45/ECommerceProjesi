using System.Collections.Generic;

namespace ECommerceProjesi.Entities 
{
    public class UrunVaryanti
    {
        public UrunVaryanti()
        {
            SiparisKalemleri = new HashSet<SiparisKalemi>();
            SepetKalemleri = new HashSet<SepetKalemi>();
            IstekListeleri = new HashSet<IstekListesi>();
        }
        public int Id { get; set; }
        public string Ad { get; set; }
        public int UrunId { get; set; }
        public virtual Urun Urun { get; set; }
        public decimal Fiyat { get; set; }
        public int StokAdedi { get; set; }
        public virtual ICollection<SiparisKalemi> SiparisKalemleri { get; set; }
        public virtual ICollection<SepetKalemi> SepetKalemleri { get; set; }
        public virtual ICollection<IstekListesi> IstekListeleri { get; set; }
    }
}
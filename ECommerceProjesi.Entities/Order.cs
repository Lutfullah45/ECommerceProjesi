using System;
using System.Collections.Generic;

namespace ECommerceProjesi.Entities 
{
    public class Siparis
    {
        public Siparis()
        {
            SiparisKalemleri = new HashSet<SiparisKalemi>();
            Odemeler = new HashSet<Odeme>();
        }
        public int Id { get; set; }
        public string SiparisNo { get; set; }
        public DateTime Tarih { get; set; }
        public decimal ToplamTutar { get; set; }
        public int MusteriId { get; set; }
        public virtual Musteri Musteri { get; set; }
        public int TeslimatAdresId { get; set; }
        public virtual Adres TeslimatAdresi { get; set; }
        public int FaturaAdresId { get; set; }
        public virtual Adres FaturaAdresi { get; set; }
        public int SiparisDurumId { get; set; }
        public virtual SiparisDurumu SiparisDurumu { get; set; }
        public virtual ICollection<SiparisKalemi> SiparisKalemleri { get; set; }
        public virtual ICollection<Odeme> Odemeler { get; set; }
        public virtual Fatura Fatura { get; set; }
        public virtual Teslimat Teslimat { get; set; }
    }
}
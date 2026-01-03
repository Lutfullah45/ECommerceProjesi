using System;

namespace ECommerceProjesi.Entities
{
    public class Odeme
    {
        public int Id { get; set; }
        public int SiparisId { get; set; }
        public virtual Siparis Siparis { get; set; }
        public decimal Tutar { get; set; }
        public string OdemeTipi { get; set; }
        public DateTime Tarih { get; set; }
        public string Durum { get; set; }
    }
}
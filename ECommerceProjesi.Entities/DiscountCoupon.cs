using System;

namespace ECommerceProjesi.Entities
{
    public class IndirimKuponu
    {
        public int Id { get; set; }
        public string Kod { get; set; }
        public decimal IndirimTutari { get; set; }
        public decimal? IndirimOrani { get; set; }
        public DateTime GecerlilikTarihi { get; set; }
        public bool Aktif { get; set; }
    }
}

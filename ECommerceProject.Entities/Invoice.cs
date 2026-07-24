using System;

namespace ECommerceProjesi.Entities
{
    public class Fatura
    {
        public int Id { get; set; }
        public int SiparisId { get; set; }
        public virtual Siparis Siparis { get; set; }
        public string FaturaNo { get; set; }
        public DateTime FaturaTarihi { get; set; }
        public string FaturaAdresiRaw { get; set; }
        public decimal ToplamKDV { get; set; }
        public decimal GenelToplam { get; set; }
    }
}
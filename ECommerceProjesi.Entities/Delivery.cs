using System;

namespace ECommerceProjesi.Entities
{
    public class Teslimat
    {
        public int Id { get; set; }
        public int SiparisId { get; set; }
        public virtual Siparis Siparis { get; set; }
        public int KargoSirketiId { get; set; }
        public virtual KargoSirketi KargoSirketi { get; set; }
        public string KargoTakipNo { get; set; }
        public DateTime GonderimTarihi { get; set; }
        public DateTime? TahminiTeslimTarihi { get; set; }
    }
}
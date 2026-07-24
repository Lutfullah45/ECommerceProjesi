namespace ECommerceProjesi.Entities
{
    public class SiparisKalemi
    {
        public int Id { get; set; }
        public int SiparisId { get; set; }
        public virtual Siparis Siparis { get; set; }
        public int UrunVaryantiId { get; set; }
        public virtual UrunVaryanti UrunVaryanti { get; set; }
        public int Adet { get; set; }
        public decimal BirimFiyat { get; set; }
    }
}

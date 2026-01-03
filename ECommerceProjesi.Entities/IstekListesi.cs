namespace ECommerceProjesi.Entities
{
    public class IstekListesi
    {
        public int MusteriId { get; set; }
        public virtual Musteri Musteri { get; set; }
        public int UrunVaryantiId { get; set; }
        public virtual UrunVaryanti UrunVaryanti { get; set; }
    }
}
using System.Collections.Generic; 
namespace ECommerceProjesi.Entities
{
    public class SepetKalemi
    {
        public int Id { get; set; }
        public int SepetId { get; set; }
        public virtual Sepet Sepet { get; set; }
        public int UrunVaryantiId { get; set; }
        public virtual UrunVaryanti UrunVaryanti { get; set; }
        public int Adet { get; set; }
    }
}
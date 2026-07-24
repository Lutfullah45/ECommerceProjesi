using System.Collections.Generic;

namespace ECommerceProjesi.Entities 
{
    public class Sepet
    {
        public Sepet()
        {
            SepetKalemleri = new HashSet<SepetKalemi>();
        }
        public int Id { get; set; }
        public int MusteriId { get; set; }
        public virtual Musteri Musteri { get; set; }
        public virtual ICollection<SepetKalemi> SepetKalemleri { get; set; }
    }
}
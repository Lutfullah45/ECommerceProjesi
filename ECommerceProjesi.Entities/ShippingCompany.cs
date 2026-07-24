using System.Collections.Generic;

namespace ECommerceProjesi.Entities 
{
    public class KargoSirketi
    {
        public KargoSirketi()
        {
            Teslimatlar = new HashSet<Teslimat>();
        }
        public int Id { get; set; }
        public string Ad { get; set; }
        public virtual ICollection<Teslimat> Teslimatlar { get; set; }
    }
}
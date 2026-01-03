using System.Collections.Generic;

namespace ECommerceProjesi.Entities
{
    public class Marka
    {
        public Marka()
        {
            Urunler = new HashSet<Urun>();
        }
        public int Id { get; set; }
        public string Ad { get; set; }
        public virtual ICollection<Urun> Urunler { get; set; }
    }
}
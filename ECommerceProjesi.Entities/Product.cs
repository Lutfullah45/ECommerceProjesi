using System.Collections.Generic;

namespace ECommerceProjesi.Entities 
{
    public class Urun
    {
        public Urun()
        {
            Varyantlar = new HashSet<UrunVaryanti>();
            Resimler = new HashSet<UrunResmi>();
            Yorumlar = new HashSet<Yorum>();
        }
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public int KategoriId { get; set; }
        public virtual Kategori Kategori { get; set; }
        public int MarkaId { get; set; }
        public virtual Marka Marka { get; set; }
        public virtual ICollection<UrunVaryanti> Varyantlar { get; set; }
        public virtual ICollection<UrunResmi> Resimler { get; set; }
        public virtual ICollection<Yorum> Yorumlar { get; set; }
    }
}
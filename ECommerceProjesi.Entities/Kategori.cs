using System.Collections.Generic;

namespace ECommerceProjesi.Entities
{
    public class Kategori
    {
        public Kategori()
        {
            Urunler = new HashSet<Urun>();
            AltKategoriler = new HashSet<Kategori>();
        }
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public int? ParentKategoriId { get; set; }
        public virtual Kategori ParentKategori { get; set; }
        public virtual ICollection<Kategori> AltKategoriler { get; set; }
        public virtual ICollection<Urun> Urunler { get; set; }
    }
}
using System.Collections.Generic;

namespace ECommerceProjesi.Entities // <- Bu satırı kontrol et
{
    public class Rol
    {
        public Rol()
        {
            KullaniciRolleri = new HashSet<KullaniciRol>();
        }
        public int Id { get; set; }
        public string Ad { get; set; }
        public virtual ICollection<KullaniciRol> KullaniciRolleri { get; set; }
    }
}
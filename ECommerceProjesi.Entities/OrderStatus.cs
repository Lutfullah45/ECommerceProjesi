using System.Collections.Generic;

namespace ECommerceProjesi.Entities
{
    public class SiparisDurumu
    {
        public SiparisDurumu()
        {
            Siparisler = new HashSet<Siparis>();
        }
        public int Id { get; set; }
        public string DurumAdi { get; set; }
        public virtual ICollection<Siparis> Siparisler { get; set; }
    }
}
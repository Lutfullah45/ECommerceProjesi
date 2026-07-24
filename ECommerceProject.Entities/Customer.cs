using System;
using System.Collections.Generic;

namespace ECommerceProjesi.Entities 
{
    public class Musteri
    {
        public Musteri()
        {
            Adresler = new HashSet<Adres>();
            Siparisler = new HashSet<Siparis>();
            Yorumlar = new HashSet<Yorum>();
            IstekListeleri = new HashSet<IstekListesi>();
            KullaniciRolleri = new HashSet<KullaniciRol>();
        }
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string ParolaHash { get; set; }
        public DateTime KayitTarihi { get; set; }
        public virtual ICollection<Adres> Adresler { get; set; }
        public virtual ICollection<Siparis> Siparisler { get; set; }
        public virtual ICollection<Yorum> Yorumlar { get; set; }
        public virtual ICollection<IstekListesi> IstekListeleri { get; set; }
        public virtual ICollection<KullaniciRol> KullaniciRolleri { get; set; }
        public virtual Sepet Sepet { get; set; }
    }
}
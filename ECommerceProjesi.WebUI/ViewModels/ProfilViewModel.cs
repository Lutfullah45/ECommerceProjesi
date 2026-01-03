using ECommerceProjesi.Entities;
using System.Collections.Generic;

namespace ECommerceProjesi.WebUI.Models
{
    public class ProfilViewModel
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public List<Adres> Adresler { get; set; }
        public Adres YeniAdres { get; set; }
    }
}
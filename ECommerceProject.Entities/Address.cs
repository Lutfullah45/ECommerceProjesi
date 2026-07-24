using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceProjesi.Entities 
{
    public class Adres
    {
        public int Id { get; set; }
        public string AdresBasligi { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Telefon { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string AcikAdres { get; set; }
        public int MusteriId { get; set; } 
    }
}
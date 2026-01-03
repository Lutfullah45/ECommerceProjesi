using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.DataAccess;
using ECommerceProjesi.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceProjesi.Business.Concrete
{
    public class AdresManager : IAdresService
    {
        private readonly ECommerceContext _context;
        public AdresManager(ECommerceContext context)
        {
            _context = context;
        }
        public void AdresEkle(Adres adres)
        {
            _context.Adresler.Add(adres);
            _context.SaveChanges();
        }
        public Adres GetirAdresById(int adresId)
        {
            return _context.Adresler.Find(adresId);
        }
        public List<Adres> GetirKullaniciAdresleri(string userId)
        {
            if (int.TryParse(userId, out int id))
            {
                return _context.Adresler.Where(a => a.MusteriId == id).ToList();
            }
            return new List<Adres>();
        }
        public void AdresSil(Adres adres)
        {
            _context.Adresler.Remove(adres);
            _context.SaveChanges();
        }
        public void AdresGuncelle(Adres adres)
        {
            _context.Adresler.Update(adres);
            _context.SaveChanges();
        }
    }
}
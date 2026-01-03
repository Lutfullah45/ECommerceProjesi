using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.DataAccess;
using ECommerceProjesi.Entities;
using Microsoft.EntityFrameworkCore; 
using System;
using System.Linq;

namespace ECommerceProjesi.Business.Concrete
{
    public class MusteriManager : IMusteriService
    {
        private readonly ECommerceContext _context;
        public MusteriManager(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Musteri? GetirMusteriByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            return _context.Musteriler.FirstOrDefault(m => m.Email == email);
        }
        public void MusteriEkle(Musteri musteri)
        {
            if (musteri == null) throw new ArgumentNullException(nameof(musteri));

            var existing = GetirMusteriByEmail(musteri.Email);
            if (existing != null)
            {
                return;
            }

            _context.Musteriler.Add(musteri);
            _context.SaveChanges();
        }
        public void MusteriGuncelle(Musteri musteri)
        {
            _context.Musteriler.Update(musteri);
            _context.SaveChanges();
        }
    }
}

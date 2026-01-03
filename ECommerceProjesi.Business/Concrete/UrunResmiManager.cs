using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.DataAccess;
using ECommerceProjesi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; 

namespace ECommerceProjesi.Business.Concrete
{
    public class UrunResmiManager : IUrunResmiService
    {
        private readonly ECommerceContext _context;
        public UrunResmiManager(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public UrunResmi? GetirResimById(int resimId)
        {
            return _context.UrunResimleri.Find(resimId);
        }
        public List<UrunResmi> GetirResimlerByUrunId(int urunId)
        {
            return _context.UrunResimleri
                           .Where(r => r.UrunId == urunId)
                           .OrderBy(r => r.Sira) 
                           .ThenByDescending(r => r.AnaResimMi) 
                           .ToList();
        }
        public void ResimEkle(UrunResmi resim)
        {
            if (resim == null) throw new ArgumentNullException(nameof(resim));
            var anaResimVarMi = _context.UrunResimleri
                                    .Any(r => r.UrunId == resim.UrunId && r.AnaResimMi);

            if (resim.AnaResimMi || !anaResimVarMi)
            {
                var digerResimler = _context.UrunResimleri
                                        .Where(r => r.UrunId == resim.UrunId && r.AnaResimMi);
                foreach (var r in digerResimler)
                {
                    r.AnaResimMi = false;
                }
                resim.AnaResimMi = true;
            }

            _context.UrunResimleri.Add(resim);
            _context.SaveChanges();
        }
        public void ResimSil(UrunResmi resim)
        {
            if (resim == null) throw new ArgumentNullException(nameof(resim));

            _context.UrunResimleri.Remove(resim);
            _context.SaveChanges();

            if (resim.AnaResimMi)
            {
                var baskaResim = _context.UrunResimleri
                                     .Where(r => r.UrunId == resim.UrunId)
                                     .OrderBy(r => r.Sira)
                                     .FirstOrDefault();
                if (baskaResim != null)
                {
                    baskaResim.AnaResimMi = true;
                    _context.SaveChanges();
                }
            }
        }
        public void AnaResimYap(int urunId, int resimId)
        {
            var tumResimler = _context.UrunResimleri.Where(r => r.UrunId == urunId);
            foreach (var resim in tumResimler)
            {
                resim.AnaResimMi = false;
            }
            var secilenResim = _context.UrunResimleri.Find(resimId);
            if (secilenResim != null && secilenResim.UrunId == urunId)
            {
                secilenResim.AnaResimMi = true;
            }
            _context.SaveChanges();
        }
    }
}
using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.DataAccess;
using ECommerceProjesi.Entities;
using System;
using System.Collections.Generic;
using System.Linq; 
using Microsoft.EntityFrameworkCore;
namespace ECommerceProjesi.Business.Concrete
{
    public class UrunVaryantiManager : IUrunVaryantiService
    {
        private readonly ECommerceContext _context;
        public UrunVaryantiManager(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public List<UrunVaryanti> GetirVaryantlarByUrunId(int urunId)
        {
            return _context.UrunVaryantilari
                           .Where(v => v.UrunId == urunId)
                           .ToList();
        }
        public UrunVaryanti? GetirVaryantById(int varyantId)
        {
            return _context.UrunVaryantilari.Find(varyantId);
        }
        public void VaryantEkle(UrunVaryanti varyant)
        {
            if (varyant == null) throw new ArgumentNullException(nameof(varyant));

            var urunVarMi = _context.Urunler.Any(u => u.Id == varyant.UrunId);
            if (!urunVarMi)
            {
                throw new Exception($"ID = {varyant.UrunId} olan bir ürün bulunamadı. Varyant eklenemiyor.");
            }

            _context.UrunVaryantilari.Add(varyant);
            _context.SaveChanges();
        }
        public void VaryantGuncelle(UrunVaryanti varyant)
        {
            if (varyant == null) throw new ArgumentNullException(nameof(varyant));
            var existingVaryant = _context.UrunVaryantilari.Find(varyant.Id);
            if (existingVaryant != null)
            {
                existingVaryant.Ad = varyant.Ad;
                existingVaryant.Fiyat = varyant.Fiyat;
                existingVaryant.StokAdedi = varyant.StokAdedi;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"ID = {varyant.Id} olan varyant bulunamadı. Güncellenemedi.");
            }
        }
        public void VaryantSil(int varyantId)
        {
            var varyant = _context.UrunVaryantilari.Find(varyantId);
            if (varyant != null)
            {
                _context.UrunVaryantilari.Remove(varyant);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"ID = {varyantId} olan varyant bulunamadı. Silinemedi.");
            }
        }
    }
}
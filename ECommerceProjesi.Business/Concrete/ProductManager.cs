using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.DataAccess;
using ECommerceProjesi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ECommerceProjesi.Business.Concrete
{
    public class UrunManager : IUrunService
    {
        private readonly ECommerceContext _context;
        public UrunManager(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void UrunEkle(Urun gelenUrun)
        {
            if (gelenUrun == null)
            {
                throw new ArgumentNullException(nameof(gelenUrun));
            }

            try
            {
                var yeniUrun = new Urun
                {
                    Ad = gelenUrun.Ad,
                    Aciklama = gelenUrun.Aciklama ?? "",
                    KategoriId = gelenUrun.KategoriId,
                    MarkaId = gelenUrun.MarkaId
                };

                _context.Urunler.Add(yeniUrun);
                int affectedRows = _context.SaveChanges();

                if (affectedRows <= 0)
                {
                    throw new Exception("Veritabanına kayıt yapılamadı (SaveChanges 0 döndü).");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UrunEkle HATA: {ex.ToString()}");
                throw;
            }
        }
        public void UrunGuncelle(Urun urun)
        {
            if (urun == null) throw new ArgumentNullException(nameof(urun));
            var existingUrun = _context.Urunler.Find(urun.Id);
            if (existingUrun != null)
            {
                _context.Entry(existingUrun).CurrentValues.SetValues(urun);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"ID'si {urun.Id} olan ürün bulunamadı.");
            }
        }
        public Urun? UrunuGetirById(int urunId)
        {
            return _context.Urunler
                           .Include(u => u.Kategori)
                           .Include(u => u.Marka)
                           .FirstOrDefault(u => u.Id == urunId);
        }
        public void UrunSil(int urunId)
        {
            var urun = _context.Urunler.Find(urunId);
            if (urun != null)
            {
                _context.Urunler.Remove(urun);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"ID'si {urunId} olan ürün bulunamadı.");
            }
        }
        public List<Urun> TumUrunleriGetir()
        {
            return _context.Urunler
                           .Include(u => u.Kategori)
                           .Include(u => u.Marka)
                           .Include(u => u.Varyantlar) 
                           .Include(u => u.Resimler)  
                           .ToList();
        }
        public List<Urun> KategoriyeGoreUrunleriGetir(int kategoriId)
        {
            return _context.Urunler
                           .Where(u => u.KategoriId == kategoriId)
                           .Include(u => u.Marka)
                           .Include(u => u.Varyantlar) 
                           .Include(u => u.Resimler)  
                           .ToList();
        }
        public List<UrunVaryanti> UrunVaryantlariniGetir(int urunId)
        {
            return _context.UrunVaryantilari
                           .Where(v => v.UrunId == urunId)
                           .ToList();
        }
        public List<UrunResmi> UrunResimleriniGetir(int urunId)
        {
            return _context.UrunResimleri
                           .Where(r => r.UrunId == urunId)
                           .OrderByDescending(r => r.AnaResimMi)
                           .ToList();
        }
        public List<Urun> UrunleriAra(string aramaMetni)
        {
            if (string.IsNullOrEmpty(aramaMetni))
            {
                return TumUrunleriGetir();
            }

            return _context.Urunler
                           .Include(u => u.Kategori)
                           .Include(u => u.Marka)
                           .Include(u => u.Resimler) 
                           .Where(u => u.Ad.Contains(aramaMetni) ||
                                       u.Aciklama.Contains(aramaMetni) ||
                                       u.Kategori.Ad.Contains(aramaMetni) ||
                                       u.Marka.Ad.Contains(aramaMetni))
                           .ToList();
        }
    }
}
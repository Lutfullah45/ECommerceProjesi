using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.DataAccess;
using ECommerceProjesi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace ECommerceProjesi.Business.Concrete
{
    public class SepetManager : ISepetService
    {
        private readonly ECommerceContext _context;
        private readonly IMusteriService _musteriService;
        public SepetManager(ECommerceContext context, IMusteriService musteriService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _musteriService = musteriService ?? throw new ArgumentNullException(nameof(musteriService));
        }
        private Sepet? GetirVeyaSiparisOlustur(string kullaniciEmail)
        {
            var musteri = _musteriService.GetirMusteriByEmail(kullaniciEmail);
            if (musteri == null)
            {
                Debug.WriteLine($"SepetManager HATA: '{kullaniciEmail}' e-postasına sahip Musteri kaydı bulunamadı!");
                return null;
            }
            var sepet = _context.Sepetler
                                .Include(s => s.SepetKalemleri)
                                .FirstOrDefault(s => s.MusteriId == musteri.Id);

            if (sepet == null)
            {
                Debug.WriteLine($"SepetManager UYARI: Musteri ID {musteri.Id} için sepet bulunamadı. Yenisi oluşturuluyor.");
                sepet = new Sepet { MusteriId = musteri.Id };
                _context.Sepetler.Add(sepet);
                _context.SaveChanges();
            }
            return sepet;
        }
        public Sepet? GetirSepetByKullaniciEmail(string kullaniciEmail)
        {
            var sepet = GetirVeyaSiparisOlustur(kullaniciEmail);
            if (sepet == null) return null;

            sepet.SepetKalemleri = _context.SepetKalemleri
                                        .Where(sk => sk.SepetId == sepet.Id)
                                        .Include(sk => sk.UrunVaryanti)
                                        .ThenInclude(uv => uv.Urun)
                                        .ThenInclude(u => u.Resimler)
                                        .ToList();
            return sepet;
        }
        public void SepeteEkle(string kullaniciEmail, int urunVaryantiId, int adet)
        {
            var sepet = GetirVeyaSiparisOlustur(kullaniciEmail);
            if (sepet == null) throw new Exception("Sepet bulunamadı veya oluşturulamadı. Müşteri kaydı eksik olabilir.");

            var mevcutKalem = sepet.SepetKalemleri
                                  .FirstOrDefault(sk => sk.UrunVaryantiId == urunVaryantiId);

            if (mevcutKalem != null)
            {
                mevcutKalem.Adet += adet;
            }
            else
            {
                var yeniKalem = new SepetKalemi
                {
                    SepetId = sepet.Id,
                    UrunVaryantiId = urunVaryantiId,
                    Adet = adet
                };
                _context.SepetKalemleri.Add(yeniKalem);
            }
            _context.SaveChanges();
        }
        public void SepettenSil(string kullaniciEmail, int sepetKalemiId)
        {
            var sepet = GetirVeyaSiparisOlustur(kullaniciEmail);
            if (sepet == null) return;
            var kalem = sepet.SepetKalemleri.FirstOrDefault(sk => sk.Id == sepetKalemiId);
            if (kalem != null)
            {
                _context.SepetKalemleri.Remove(kalem);
                _context.SaveChanges();
            }
        }
        public void SepetAdetGuncelle(string kullaniciEmail, int sepetKalemiId, int yeniAdet)
        {
            var sepet = GetirVeyaSiparisOlustur(kullaniciEmail);
            if (sepet == null) return;
            var kalem = sepet.SepetKalemleri.FirstOrDefault(sk => sk.Id == sepetKalemiId);
            if (kalem != null)
            {
                if (yeniAdet > 0)
                {
                    kalem.Adet = yeniAdet;
                }
                else
                {
                    _context.SepetKalemleri.Remove(kalem);
                }
                _context.SaveChanges();
            }
        }
        public void SepetiTemizle(int sepetId)
        {
            var kalemler = _context.SepetKalemleri.Where(sk => sk.SepetId == sepetId);
            _context.SepetKalemleri.RemoveRange(kalemler);
            _context.SaveChanges();
        }
    }
}
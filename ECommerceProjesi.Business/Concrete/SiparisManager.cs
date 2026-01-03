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
    public class SiparisManager : ISiparisService
    {
        private readonly ECommerceContext _context;
        public SiparisManager(ECommerceContext context)
        {
            _context = context;
        }
        public List<Siparis> GetirSiparislerByMusteriId(int musteriId)
        {
            return _context.Siparisler
                           .Include(s => s.SiparisDurumu)
                           .Where(s => s.MusteriId == musteriId)
                           .OrderByDescending(s => s.Tarih)
                           .ToList();
        }
        public Siparis? GetirSiparisDetay(int siparisId)
        {
            return _context.Siparisler
                          .Include(s => s.SiparisKalemleri)
                          .ThenInclude(sk => sk.UrunVaryanti)
                          .ThenInclude(uv => uv.Urun)
                          .Include(s => s.TeslimatAdresi)
                          .Include(s => s.FaturaAdresi)
                          .Include(s => s.SiparisDurumu)
                          .FirstOrDefault(s => s.Id == siparisId);
        }
        public Siparis SiparisOlustur(Sepet sepet, int teslimatAdresId, int faturaAdresId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var sepetKalemleri = _context.SepetKalemleri
                                            .Include(sk => sk.UrunVaryanti)
                                            .Where(sk => sk.SepetId == sepet.Id)
                                            .ToList();

                    if (!sepetKalemleri.Any())
                    {
                        throw new Exception("Sepet boş.");
                    }

                    decimal toplamTutar = sepetKalemleri.Sum(sk => sk.Adet * sk.UrunVaryanti.Fiyat);

                    var siparis = new Siparis
                    {
                        MusteriId = sepet.MusteriId,
                        Tarih = DateTime.UtcNow,
                        ToplamTutar = toplamTutar,
                        SiparisDurumId = 1, 
                        TeslimatAdresId = teslimatAdresId,
                        FaturaAdresId = faturaAdresId,
                        SiparisNo = "S" + DateTime.UtcNow.Ticks.ToString()
                    };
                    _context.Siparisler.Add(siparis);

                    foreach (var sepetKalemi in sepetKalemleri)
                    {
                        var siparisKalemi = new SiparisKalemi
                        {
                            Siparis = siparis,
                            UrunVaryantiId = sepetKalemi.UrunVaryantiId,
                            Adet = sepetKalemi.Adet,
                            BirimFiyat = sepetKalemi.UrunVaryanti.Fiyat
                        };
                        _context.SiparisKalemleri.Add(siparisKalemi);

                        var varyant = _context.UrunVaryantilari.Find(sepetKalemi.UrunVaryantiId);
                        if (varyant == null || varyant.StokAdedi < sepetKalemi.Adet)
                        {
                            throw new Exception($"Stok yetersiz: {sepetKalemi.UrunVaryanti.Ad}");
                        }
                        varyant.StokAdedi -= sepetKalemi.Adet;
                    }

                    _context.SepetKalemleri.RemoveRange(sepetKalemleri);
                    _context.SaveChanges();
                    transaction.Commit();
                    return siparis;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SiparisOlustur HATA: {ex.ToString()}");
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
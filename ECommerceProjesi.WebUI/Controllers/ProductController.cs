using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using ECommerceProjesi.Business.Abstract; 
using ECommerceProjesi.DataAccess; 
using ECommerceProjesi.Entities;
using ECommerceProjesi.WebUI.ViewModels; 

namespace ECommerceProjesi.WebUI.Controllers
{
    public class UrunController : Controller
    {
        private readonly IUrunService _urunService;
        private readonly IKategoriService _kategoriService;
        private readonly ECommerceContext _context;
        public UrunController(IUrunService urunService, IKategoriService kategoriService, ECommerceContext context)
        {
            _urunService = urunService;
            _kategoriService = kategoriService;
            _context = context;
        }
        public IActionResult Index(string q, int? kategoriId)
        {
            if (!string.IsNullOrEmpty(q) || kategoriId.HasValue)
            {
                List<Urun> urunler;

                if (kategoriId.HasValue)
                {
                    urunler = _urunService.KategoriyeGoreUrunleriGetir(kategoriId.Value);
                    var kategori = _kategoriService.GetirKategoriById(kategoriId.Value);
                    ViewData["Title"] = kategori != null ? $"{kategori.Ad}" : "Kategori";
                }
                else
                {
                    urunler = _urunService.UrunleriAra(q);
                    ViewData["Title"] = "Arama Sonuçları";
                    ViewData["AramaMetni"] = q;
                }
                return View("List", urunler);
            }
            else
            {
                var tumUrunler = _urunService.TumUrunleriGetir();

                var vitrinModeli = tumUrunler
                    .GroupBy(u => u.Kategori)
                    .Select(grup => new KategoriVitrinViewModel
                    {
                        KategoriAdi = grup.Key.Ad,
                        KategoriUrl = grup.Key.Id.ToString(),
                        Urunler = grup.OrderByDescending(x => x.Id).Take(4).ToList()
                    })
                    .ToList();

                ViewData["Title"] = "Mağaza Vitrini";
                return View(vitrinModeli);
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var urun = await _context.Urunler
                    .Include(u => u.Kategori)
                    .Include(u => u.Marka)
                    .Include(u => u.Resimler)
                    .Include(u => u.Varyantlar)
                    .Include(u => u.Yorumlar).ThenInclude(y => y.User)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (urun == null) return NotFound();
                var model = new UrunDetayViewModel
                {
                    Urun = urun,
                    Resimler = urun.Resimler.ToList(),
                    Varyantlar = urun.Varyantlar.ToList(),
                    Yorumlar = urun.Yorumlar.OrderByDescending(y => y.Tarih).ToList(),
                    OrtalamaPuan = urun.Yorumlar.Any() ? urun.Yorumlar.Average(y => y.Puan) : 0
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Urun Detay Hatası: {ex.Message}");
                TempData["ErrorMessage"] = "Ürün detayları yüklenirken bir hata oluştu.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
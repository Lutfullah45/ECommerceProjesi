using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProjesi.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KategorilerController : Controller
    {
        private readonly IKategoriService _kategoriService;

        public KategorilerController(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService ?? throw new ArgumentNullException(nameof(kategoriService));
        }
        public IActionResult Index()
        {
            try
            {
                var kategoriler = _kategoriService.TumKategorileriGetir();
                return View(kategoriler);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Kategoriler Index HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Kategoriler listelenirken bir hata oluştu.";
                return View(new List<Kategori>());
            }
        }
        public IActionResult Create()
        {
            LoadAnaKategorilerViewBag();
            return View(new Kategori());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Ad,Aciklama,ParentKategoriId")] Kategori kategori)
        {
            if (string.IsNullOrWhiteSpace(kategori.Ad)) ModelState.AddModelError("Ad", "Kategori Adı boş olamaz.");
            if (kategori.ParentKategoriId == 0) kategori.ParentKategoriId = null;

            ModelState.Remove("ParentKategori");
            ModelState.Remove("Urunler");
            ModelState.Remove("AltKategoriler");

            if (ModelState.IsValid)
            {
                try
                {
                    _kategoriService.KategoriEkle(kategori);
                    TempData["SuccessMessage"] = $"'{kategori.Ad}' kategorisi başarıyla eklendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Admin Kategoriler Create POST HATASI: {ex.ToString()}");
                    ModelState.AddModelError("", "Kategori kaydedilirken beklenmedik bir hata oluştu.");
                }
            }

            LoadAnaKategorilerViewBag(kategori.ParentKategoriId);
            return View(kategori);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var kategori = _kategoriService.KategoriGetirById(id.Value);
            if (kategori == null) return NotFound();

            LoadAnaKategorilerViewBag(kategori.ParentKategoriId);
            return View(kategori);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Ad,Aciklama,ParentKategoriId")] Kategori kategori)
        {
            if (id != kategori.Id) return BadRequest();

            if (string.IsNullOrWhiteSpace(kategori.Ad)) ModelState.AddModelError("Ad", "Kategori Adı boş olamaz.");
            if (kategori.ParentKategoriId == 0) kategori.ParentKategoriId = null;

            ModelState.Remove("ParentKategori");
            ModelState.Remove("Urunler");
            ModelState.Remove("AltKategoriler");

            if (ModelState.IsValid)
            {
                try
                {
                    _kategoriService.KategoriGuncelle(kategori);
                    TempData["SuccessMessage"] = $"'{kategori.Ad}' kategorisi başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_kategoriService.KategoriGetirById(kategori.Id) == null) return NotFound();
                    else ModelState.AddModelError("", "Kayıt başka bir kullanıcı tarafından güncellenmiş. Tekrar deneyin."); ;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Admin Kategoriler Edit POST HATASI: {ex.ToString()}");
                    ModelState.AddModelError("", "Kategori güncellenirken beklenmedik bir hata oluştu.");
                }
            }

            LoadAnaKategorilerViewBag(kategori.ParentKategoriId);
            return View(kategori);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var kategori = _kategoriService.KategoriGetirById(id.Value);
            if (kategori == null) return NotFound();
            return View(kategori);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _kategoriService.KategoriSil(id);
                TempData["SuccessMessage"] = $"ID = {id} olan kategori başarıyla silindi.";
            }
            catch (DbUpdateException ex) 
            {
                Debug.WriteLine($"Admin Kategoriler Delete POST DB HATASI (ID: {id}): {ex.InnerException?.Message ?? ex.Message}");
                TempData["ErrorMessage"] = $"Kategori (ID: {id}) silinemedi. Bu kategoriye bağlı ürünler veya alt kategoriler olabilir.";
            }
            catch (Exception ex) 
            {
                Debug.WriteLine($"Admin Kategoriler Delete POST HATASI (ID: {id}): {ex.ToString()}");
                TempData["ErrorMessage"] = $"Kategori (ID: {id}) silinirken bir hata oluştu.";
            }
            return RedirectToAction(nameof(Index));
        }
        private void LoadAnaKategorilerViewBag(int? selectedId = null)
        {
            try
            {
                ViewBag.AnaKategoriler = new SelectList(_kategoriService.AnaKategorileriGetir() ?? new List<Kategori>(), "Id", "Ad", selectedId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ana Kategoriler ViewBag yüklenirken HATA: {ex.Message}");
                ViewBag.AnaKategoriler = new SelectList(new List<Kategori>(), "Id", "Ad");
            }
        }
    }
}
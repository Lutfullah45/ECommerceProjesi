using ECommerceProjesi.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using ECommerceProjesi.Entities;
using ECommerceProjesi.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore; 

namespace ECommerceProjesi.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UrunlerController : Controller
    {
        private readonly IUrunService _urunService;
        private readonly IKategoriService _kategoriService;
        private readonly IMarkaService _markaService;

        public UrunlerController(IUrunService urunService, IKategoriService kategoriService, IMarkaService markaService)
        {
            _urunService = urunService ?? throw new ArgumentNullException(nameof(urunService));
            _kategoriService = kategoriService ?? throw new ArgumentNullException(nameof(kategoriService));
            _markaService = markaService ?? throw new ArgumentNullException(nameof(markaService));
        }
        // GET: /Admin/Urunler
        public IActionResult Index()
        {
            try
            {
                var urunler = _urunService.TumUrunleriGetir();
                return View(urunler);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Index HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Ürünler listelenirken bir hata oluştu.";
                return View(new List<Urun>());
            }
        }
        // GET: /Admin/Urunler/Create
        public IActionResult Create()
        {
            try
            {
                var viewModel = new UrunFormViewModel
                {
                    Kategoriler = GetKategoriSelectList(),
                    Markalar = GetMarkaSelectList()
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Create GET HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Yeni ürün formu yüklenirken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }
        // POST: /Admin/Urunler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UrunFormViewModel viewModel)
        {
            ValidateUrunViewModel(viewModel);
            if (viewModel.Urun.KategoriId > 0) ModelState.Remove("Urun.Kategori");
            if (viewModel.Urun.MarkaId > 0) ModelState.Remove("Urun.Marka");

            if (ModelState.IsValid)
            {
                try
                {
                    _urunService.UrunEkle(viewModel.Urun);
                    TempData["SuccessMessage"] = $"'{viewModel.Urun.Ad}' ürünü başarıyla eklendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Admin Create POST HATASI: {ex.ToString()}");
                    ModelState.AddModelError("", "Ürün kaydedilirken beklenmedik bir hata oluştu.");
                }
            }

            viewModel.Kategoriler = GetKategoriSelectList(viewModel.Urun.KategoriId);
            viewModel.Markalar = GetMarkaSelectList(viewModel.Urun.MarkaId);
            return View(viewModel);
        }
        // GET: /Admin/Urunler/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var urun = _urunService.UrunuGetirById(id.Value);
            if (urun == null) return NotFound();

            try
            {
                var viewModel = new UrunFormViewModel
                {
                    Urun = urun,
                    Kategoriler = GetKategoriSelectList(urun.KategoriId),
                    Markalar = GetMarkaSelectList(urun.MarkaId)
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Edit GET HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Düzenleme formu yüklenirken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }
        // POST: /Admin/Urunler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UrunFormViewModel viewModel)
        {
            if (id != viewModel.Urun.Id) return BadRequest();

            ValidateUrunViewModel(viewModel);
            if (viewModel.Urun.KategoriId > 0) ModelState.Remove("Urun.Kategori");
            if (viewModel.Urun.MarkaId > 0) ModelState.Remove("Urun.Marka");

            if (ModelState.IsValid)
            {
                try
                {
                    _urunService.UrunGuncelle(viewModel.Urun);
                    TempData["SuccessMessage"] = $"'{viewModel.Urun.Ad}' ürünü başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Debug.WriteLine($"Admin Edit POST Concurrency HATASI: {ex.ToString()}");
                    ModelState.AddModelError("", "Ürün siz düzenlerken başka bir kullanıcı tarafından değiştirilmiş.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Admin Edit POST HATASI: {ex.ToString()}");
                    ModelState.AddModelError("", "Ürün güncellenirken beklenmedik bir hata oluştu.");
                }
            }

            viewModel.Kategoriler = GetKategoriSelectList(viewModel.Urun.KategoriId);
            viewModel.Markalar = GetMarkaSelectList(viewModel.Urun.MarkaId);
            return View(viewModel);
        }
        // GET: /Admin/Urunler/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var urun = _urunService.UrunuGetirById(id.Value); 
            if (urun == null) return NotFound();

            return View(urun); 
        }
        // POST: /Admin/Urunler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _urunService.UrunSil(id);
                TempData["SuccessMessage"] = $"ID = {id} olan ürün başarıyla silindi.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Delete POST HATASI (ID: {id}): {ex.ToString()}");
                TempData["ErrorMessage"] = $"Ürün (ID: {id}) silinirken bir hata oluştu: {ex.Message}";
            }
            return RedirectToAction(nameof(Index)); 
        }
        // GET: /Admin/Urunler/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest("Geçersiz Ürün ID.");
            }

            try
            {
                var urun = _urunService.UrunuGetirById(id.Value);

                if (urun == null)
                {
                    return NotFound($"ID = {id} olan ürün bulunamadı.");
                }
                var varyantlar = _urunService.UrunVaryantlariniGetir(id.Value);
                var resimler = _urunService.UrunResimleriniGetir(id.Value);
                ViewBag.Varyantlar = varyantlar ?? new List<UrunVaryanti>();
                ViewBag.Resimler = resimler ?? new List<UrunResmi>();

                return View(urun);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"##### Admin Details GET HATASI (ID: {id}) #####\n{ex.ToString()}\n######################");
                TempData["ErrorMessage"] = "Ürün detayı yüklenirken bir hata oluştu.";
                return RedirectToAction(nameof(Index)); 
            }
        }
        private IEnumerable<SelectListItem> GetKategoriSelectList(int? selectedId = null)
        {
            var kategoriler = _kategoriService.TumKategorileriGetir() ?? new List<Kategori>();
            return new SelectList(kategoriler, "Id", "Ad", selectedId);
        }
        private IEnumerable<SelectListItem> GetMarkaSelectList(int? selectedId = null)
        {
            var markalar = _markaService.TumMarkalariGetir() ?? new List<Marka>();
            return new SelectList(markalar, "Id", "Ad", selectedId);
        }
        private void ValidateUrunViewModel(UrunFormViewModel viewModel)
        {
            if (viewModel.Urun.KategoriId == 0) ModelState.AddModelError("Urun.KategoriId", "Lütfen bir kategori seçin.");
            if (viewModel.Urun.MarkaId == 0) ModelState.AddModelError("Urun.MarkaId", "Lütfen bir marka seçin.");
            if (string.IsNullOrWhiteSpace(viewModel.Urun.Ad)) ModelState.AddModelError("Urun.Ad", "Ürün Adı boş olamaz.");
        }
    }
}
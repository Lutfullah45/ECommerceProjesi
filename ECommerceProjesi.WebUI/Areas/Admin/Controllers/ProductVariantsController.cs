using ECommerceProjesi.Business.Abstract;     
using ECommerceProjesi.Entities;               
using ECommerceProjesi.WebUI.Areas.Admin.Models; 
using Microsoft.AspNetCore.Authorization;    
using Microsoft.AspNetCore.Mvc;                
using Microsoft.AspNetCore.Mvc.Rendering;      
using System;                               
using System.Diagnostics;                     
using System.Collections.Generic;            
using System.Linq;                           
using Microsoft.EntityFrameworkCore;          
namespace ECommerceProjesi.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] 
    public class UrunVaryantilariController : Controller
    {
        private readonly IUrunVaryantiService _varyantService;
        private readonly IUrunService _urunService; 
        public UrunVaryantilariController(IUrunVaryantiService varyantService, IUrunService urunService)
        {
            _varyantService = varyantService ?? throw new ArgumentNullException(nameof(varyantService));
            _urunService = urunService ?? throw new ArgumentNullException(nameof(urunService));
        }
        // GET: /Admin/UrunVaryantilari/Index/{urunId}
        public IActionResult Index(int urunId)
        {
            if (urunId <= 0)
            {
                TempData["ErrorMessage"] = "Geçersiz Ürün ID.";
                return RedirectToAction("Index", "Urunler");
            }
            try
            {
                var urun = _urunService.UrunuGetirById(urunId);
                if (urun == null)
                {
                    TempData["ErrorMessage"] = "Ürün bulunamadı.";
                    return RedirectToAction("Index", "Urunler");
                }
                var varyantlar = _varyantService.GetirVaryantlarByUrunId(urunId);
                var viewModel = new UrunVaryantIndexViewModel
                {
                    Urun = urun,
                    Varyantlar = varyantlar ?? new List<UrunVaryanti>()
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Varyant Index GET HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Varyantlar yüklenirken bir hata oluştu.";
                return RedirectToAction("Index", "Urunler");
            }
        }

        // GET: /Admin/UrunVaryantilari/Create?urunId=5
        public IActionResult Create(int urunId)
        {
            if (urunId <= 0) return BadRequest("Geçersiz Ürün ID.");
            var urun = _urunService.UrunuGetirById(urunId);
            if (urun == null) return NotFound("İlgili ürün bulunamadı.");

            var yeniVaryant = new UrunVaryanti { UrunId = urunId };
            ViewBag.UrunAdi = urun.Ad;
            return View(yeniVaryant);
        }

        // POST: /Admin/UrunVaryantilari/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("UrunId,Ad,Fiyat,StokAdedi")] UrunVaryanti varyant)
        {
            if (string.IsNullOrWhiteSpace(varyant.Ad)) ModelState.AddModelError("Ad", "Varyant Adı boş olamaz.");
            if (varyant.Fiyat < 0) ModelState.AddModelError("Fiyat", "Fiyat 0'dan küçük olamaz.");
            if (varyant.StokAdedi < 0) ModelState.AddModelError("StokAdedi", "Stok 0'dan küçük olamaz.");
            ModelState.Remove("Urun");

            if (ModelState.IsValid)
            {
                try
                {
                    _varyantService.VaryantEkle(varyant);
                    TempData["SuccessMessage"] = $"'{varyant.Ad}' adlı varyant başarıyla eklendi.";
                    return RedirectToAction(nameof(Index), new { urunId = varyant.UrunId });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Admin Varyant Create POST HATASI: {ex.ToString()}");
                    ModelState.AddModelError("", "Varyant kaydedilirken beklenmedik bir hata oluştu.");
                }
            }

            if (varyant.UrunId > 0)
            {
                var urun = _urunService.UrunuGetirById(varyant.UrunId);
                ViewBag.UrunAdi = urun?.Ad ?? "Ürün Bulunamadı";
            }
            return View(varyant);
        }

        // GET: /Admin/UrunVaryantilari/Edit/15
        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Geçersiz Varyant ID.");
            try
            {
                var varyant = _varyantService.GetirVaryantById(id.Value);
                if (varyant == null) return NotFound($"ID = {id} olan varyant bulunamadı.");
                var urun = _urunService.UrunuGetirById(varyant.UrunId);
                ViewBag.UrunAdi = urun?.Ad ?? "Ürün Bulunamadı";
                return View(varyant);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Varyant Edit GET HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Düzenleme formu yüklenirken bir hata oluştu.";
                return RedirectToAction("Index", "Urunler");
            }
        }

        // POST: /Admin/UrunVaryantilari/Edit/15
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,UrunId,Ad,Fiyat,StokAdedi")] UrunVaryanti varyant)
        {
            if (id != varyant.Id) return BadRequest("ID uyuşmazlığı.");

            if (string.IsNullOrWhiteSpace(varyant.Ad)) ModelState.AddModelError("Ad", "Varyant Adı boş olamaz.");
            if (varyant.Fiyat < 0) ModelState.AddModelError("Fiyat", "Fiyat 0'dan küçük olamaz.");
            if (varyant.StokAdedi < 0) ModelState.AddModelError("StokAdedi", "Stok 0'dan küçük olamaz.");
            ModelState.Remove("Urun");

            if (ModelState.IsValid)
            {
                try
                {
                    _varyantService.VaryantGuncelle(varyant);
                    TempData["SuccessMessage"] = $"'{varyant.Ad}' adlı varyant başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index), new { urunId = varyant.UrunId });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Admin Varyant Edit POST HATASI: {ex.ToString()}");
                    ModelState.AddModelError("", "Varyant güncellenirken beklenmedik bir hata oluştu.");
                }
            }

            if (varyant.UrunId > 0)
            {
                var urun = _urunService.UrunuGetirById(varyant.UrunId);
                ViewBag.UrunAdi = urun?.Ad ?? "Ürün Bulunamadı";
            }
            return View(varyant);
        }

        // GET: /Admin/UrunVaryantilari/Delete/15
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest("Geçersiz Varyant ID.");
            try
            {
                var varyant = _varyantService.GetirVaryantById(id.Value);
                if (varyant == null) return NotFound($"ID = {id} olan varyant bulunamadı.");
                var urun = _urunService.UrunuGetirById(varyant.UrunId);
                ViewBag.UrunAdi = urun?.Ad ?? "Ürün Bulunamadı";
                return View(varyant);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Varyant Delete GET HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Silme sayfası yüklenirken bir hata oluştu.";
                return RedirectToAction("Index", "Urunler");
            }
        }

        // POST: /Admin/UrunVaryantilari/Delete/15
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            int urunId = 0;
            try
            {
                var varyant = _varyantService.GetirVaryantById(id);
                if (varyant == null)
                {
                    TempData["ErrorMessage"] = $"ID = {id} olan varyant bulunamadı.";
                    return RedirectToAction("Index", "Urunler");
                }
                urunId = varyant.UrunId;

                _varyantService.VaryantSil(id);
                TempData["SuccessMessage"] = $"'{varyant.Ad}' adlı varyant (ID: {id}) başarıyla silindi.";
                return RedirectToAction(nameof(Index), new { urunId = urunId });
            }
            catch (DbUpdateException dbEx)
            {
                Debug.WriteLine($"Admin Varyant Delete POST DB HATASI (ID: {id}): {dbEx.InnerException?.Message ?? dbEx.Message}");
                TempData["ErrorMessage"] = $"Varyant (ID: {id}) silinemedi. Bu varyant geçmiş siparişlerde kullanılıyor olabilir.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Varyant Delete POST HATASI (ID: {id}): {ex.ToString()}");
                TempData["ErrorMessage"] = $"Varyant (ID: {id}) silinirken bir hata oluştu.";
            }

            if (urunId > 0) return RedirectToAction(nameof(Index), new { urunId = urunId });
            return RedirectToAction("Index", "Urunler");
        }
    }
}
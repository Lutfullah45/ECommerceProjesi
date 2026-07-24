using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProjesi.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MarkalarController : Controller
    {
        private readonly IMarkaService _markaService;

        public MarkalarController(IMarkaService markaService)
        {
            _markaService = markaService ?? throw new ArgumentNullException(nameof(markaService));
        }
        // GET: /Admin/Markalar
        public IActionResult Index()
        {
            try
            {
                var markalar = _markaService.TumMarkalariGetir();
                return View(markalar);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Markalar Index HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Markalar listelenirken bir hata oluştu.";
                return View(new List<Marka>());
            }
        }
        // GET: /Admin/Markalar/Create
        public IActionResult Create()
        {
            return View(new Marka());
        }
        // POST: /Admin/Markalar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Ad")] Marka marka)
        {
            if (string.IsNullOrWhiteSpace(marka.Ad)) ModelState.AddModelError("Ad", "Marka Adı boş olamaz.");
            ModelState.Remove("Urunler");

            if (ModelState.IsValid)
            {
                try
                {
                    _markaService.MarkaEkle(marka);
                    TempData["SuccessMessage"] = $"'{marka.Ad}' markası başarıyla eklendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Admin Markalar Create POST HATASI: {ex.ToString()}");
                    ModelState.AddModelError("", "Marka kaydedilirken beklenmedik bir hata oluştu.");
                }
            }
            return View(marka);
        }
        // GET: /Admin/Markalar/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var marka = _markaService.MarkaGetirById(id.Value);
            if (marka == null) return NotFound();
            return View(marka);
        }
        // POST: /Admin/Markalar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Ad")] Marka marka)
        {
            if (id != marka.Id) return BadRequest();

            if (string.IsNullOrWhiteSpace(marka.Ad)) ModelState.AddModelError("Ad", "Marka Adı boş olamaz.");
            ModelState.Remove("Urunler"); 
            if (ModelState.IsValid)
            {
                try
                {
                    _markaService.MarkaGuncelle(marka);
                    TempData["SuccessMessage"] = $"'{marka.Ad}' markası başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_markaService.MarkaGetirById(marka.Id) == null) return NotFound();
                    else ModelState.AddModelError("", "Kayıt başka bir kullanıcı tarafından güncellenmiş. Tekrar deneyin."); ;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Admin Markalar Edit POST HATASI: {ex.ToString()}");
                    ModelState.AddModelError("", "Marka güncellenirken beklenmedik bir hata oluştu.");
                }
            }
            return View(marka);
        }
        // GET: /Admin/Markalar/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var marka = _markaService.MarkaGetirById(id.Value);
            if (marka == null) return NotFound();
            return View(marka);
        }
        // POST: /Admin/Markalar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _markaService.MarkaSil(id);
                TempData["SuccessMessage"] = $"ID = {id} olan marka başarıyla silindi.";
            }
            catch (DbUpdateException ex) 
            {
                Debug.WriteLine($"Admin Markalar Delete POST DB HATASI (ID: {id}): {ex.InnerException?.Message ?? ex.Message}");
                TempData["ErrorMessage"] = $"Marka (ID: {id}) silinemedi. Bu markaya bağlı ürünler olabilir.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Markalar Delete POST HATASI (ID: {id}): {ex.ToString()}");
                TempData["ErrorMessage"] = $"Marka (ID: {id}) silinirken bir hata oluştu.";
            }
            return RedirectToAction(nameof(Index)); 
        }
    }
}
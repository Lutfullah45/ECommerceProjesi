using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.Entities;
using ECommerceProjesi.WebUI.Areas.Admin.Models;
using ECommerceProjesi.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProjesi.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UrunResimleriController : Controller
    {
        private readonly IUrunResmiService _resimService;
        private readonly IUrunService _urunService;
        private readonly FileHelper _fileHelper;

        public UrunResimleriController(IUrunResmiService resimService, IUrunService urunService, FileHelper fileHelper)
        {
            _resimService = resimService ?? throw new ArgumentNullException(nameof(resimService));
            _urunService = urunService ?? throw new ArgumentNullException(nameof(urunService));
            _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));
        }

        // GET: /Admin/UrunResimleri/Index/{urunId}
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
                var resimler = _resimService.GetirResimlerByUrunId(urunId);
                var viewModel = new UrunResimIndexViewModel
                {
                    Urun = urun,
                    Resimler = resimler ?? new List<UrunResmi>()
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin Resim Index GET HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Resim yönetimi sayfası yüklenirken bir hata oluştu.";
                return RedirectToAction("Index", "Urunler");
            }
        }

        // POST: /Admin/UrunResimleri/ResimEkle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResimEkle(IFormFile dosya, int urunId, int sira, bool anaResimMi)
        {
            if (dosya == null || dosya.Length == 0)
            {
                TempData["ErrorMessage"] = "Lütfen yüklenecek bir resim dosyası seçin.";
                return RedirectToAction(nameof(Index), new { urunId = urunId });
            }

            try
            {
                string dosyaYolu = await _fileHelper.UploadFileAsync(dosya, "urunler");
                if (dosyaYolu != null)
                {
                    var yeniResim = new UrunResmi
                    {
                        UrunId = urunId,
                        DosyaAdi = dosyaYolu,
                        AnaResimMi = anaResimMi,
                        Sira = sira
                    };
                    _resimService.ResimEkle(yeniResim);
                    TempData["SuccessMessage"] = "Resim başarıyla yüklendi ve eklendi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Dosya yüklenirken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin ResimEkle POST HATASI: {ex.ToString()}");
                TempData["ErrorMessage"] = "Resim kaydedilirken beklenmedik bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index), new { urunId = urunId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AnaResimYap(int resimId, int urunId)
        {
            if (resimId <= 0 || urunId <= 0)
            {
                TempData["ErrorMessage"] = "Geçersiz ID.";
                return RedirectToAction("Index", "Urunler");
            }

            try
            {
                _resimService.AnaResimYap(urunId, resimId);
                TempData["SuccessMessage"] = $"Resim (ID: {resimId}) başarıyla ana resim olarak ayarlandı.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Admin AnaResimYap POST HATASI (ID: {resimId}): {ex.ToString()}");
                TempData["ErrorMessage"] = "Ana resim ayarlanırken bir hata oluştu.";
            }
            return RedirectToAction(nameof(Index), new { urunId = urunId });
        }
        // POST: /Admin/UrunResimleri/ResimSil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResimSil(int resimId) 
        {
            int urunId = 0;
            try
            {
                var resim = _resimService.GetirResimById(resimId);
                if (resim == null)
                {
                    TempData["ErrorMessage"] = $"ID = {resimId} olan resim bulunamadı.";
                    return RedirectToAction("Index", "Urunler");
                }
                urunId = resim.UrunId; 
                string dosyaYolu = resim.DosyaAdi;

                _resimService.ResimSil(resim);

                _fileHelper.DeleteFile(dosyaYolu);

                TempData["SuccessMessage"] = $"Resim (ID: {resimId}) başarıyla silindi.";
            }
            catch (DbUpdateException dbEx) 
            {
                Debug.WriteLine($"Admin ResimSil POST DB HATASI (ID: {resimId}): {dbEx.InnerException?.Message ?? dbEx.Message}");
                TempData["ErrorMessage"] = $"Resim (ID: {resimId}) silinemedi. (Veritabanı Hatası)";
            }
            catch (Exception ex) 
            {
                Debug.WriteLine($"Admin ResimSil POST HATASI (ID: {resimId}): {ex.ToString()}");
                TempData["ErrorMessage"] = $"Resim (ID: {resimId}) silinirken bir hata oluştu.";
            }
            if (urunId > 0) return RedirectToAction(nameof(Index), new { urunId = urunId });
            return RedirectToAction("Index", "Urunler");
        }
    }
}
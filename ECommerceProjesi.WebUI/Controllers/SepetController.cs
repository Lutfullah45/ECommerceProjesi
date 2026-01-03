using ECommerceProjesi.Business.Abstract; 
using ECommerceProjesi.Entities;       
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;        
using System;
using System.Diagnostics;                
using System.Security.Claims;          

namespace ECommerceProjesi.WebUI.Controllers
{
    [Authorize]
    public class SepetController : Controller
    {
        private readonly ISepetService _sepetService;
        private readonly IMusteriService _musteriService;

        public SepetController(ISepetService sepetService, IMusteriService musteriService)
        {
            _sepetService = sepetService ?? throw new ArgumentNullException(nameof(sepetService));
            _musteriService = musteriService ?? throw new ArgumentNullException(nameof(musteriService));
        }
        private string GetKullaniciEmail()
        {
            return User.Identity?.Name ?? throw new Exception("Kullanıcı bulunamadı.");
        }
        public IActionResult Index()
        {
            try
            {
                var kullaniciEmail = GetKullaniciEmail();
                var sepet = _sepetService.GetirSepetByKullaniciEmail(kullaniciEmail);

                if (sepet == null)
                {
                    TempData["ErrorMessage"] = "Sepetiniz getirilirken bir sorun oluştu. (Müşteri kaydı bulunamadı?)";
                    return RedirectToAction("Index", "Home");
                }
                return View(sepet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Sepet Index GET HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Sepetiniz yüklenirken bir hata oluştu.";
                return RedirectToAction("Index", "Home");
            }
        }
        // POST: /Sepet/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(int urunVaryantiId, int adet)
        {
            if (urunVaryantiId <= 0 || adet <= 0)
            {
                TempData["ErrorMessage"] = "Geçersiz ürün veya adet bilgisi.";
                return Redirect(Request.Headers["Referer"].ToString() ?? "/"); 
            }

            try
            {
                var kullaniciEmail = GetKullaniciEmail();
                _sepetService.SepeteEkle(kullaniciEmail, urunVaryantiId, adet);
                TempData["SuccessMessage"] = "Ürün başarıyla sepete eklendi!";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Sepet Ekle POST HATASI: {ex.Message}");
                TempData["ErrorMessage"] = $"Ürün sepete eklenirken bir hata oluştu: {ex.Message}";
            }
            return Redirect(Request.Headers["Referer"].ToString() ?? "/");
        }
        // POST: /Sepet/Sil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sil(int sepetKalemiId)
        {
            try
            {
                var kullaniciEmail = GetKullaniciEmail();
                _sepetService.SepettenSil(kullaniciEmail, sepetKalemiId);
                TempData["SuccessMessage"] = "Ürün sepetten kaldırıldı.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Sepet Sil POST HATASI: {ex.Message}");
                TempData["ErrorMessage"] = "Ürün sepetten kaldırılırken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }
        // POST: /Sepet/Guncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(int sepetKalemiId, int adet)
        {
            try
            {
                var kullaniciEmail = GetKullaniciEmail();
                _sepetService.SepetAdetGuncelle(kullaniciEmail, sepetKalemiId, adet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Sepet Guncelle POST HATASI: {ex.Message}");
                TempData["ErrorMessage"] = $"Sepet güncellenirken bir hata oluştu: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
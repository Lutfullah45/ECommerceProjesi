using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.Entities;
using ECommerceProjesi.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProjesi.WebUI.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ISepetService _sepetService;
        private readonly ISiparisService _siparisService;
        private readonly IAdresService _adresService;
        private readonly IMusteriService _musteriService;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        public CheckoutController(
            ISepetService sepetService,
            ISiparisService siparisService,
            IAdresService adresService,
            IMusteriService musteriService,
            IEmailSender emailSender,
            UserManager<IdentityUser> userManager)
        {
            _sepetService = sepetService;
            _siparisService = siparisService;
            _adresService = adresService;
            _musteriService = musteriService;
            _emailSender = emailSender;
            _userManager = userManager;
        }
        private Musteri? GetMevcutMusteri()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email)) return null;
            return _musteriService.GetirMusteriByEmail(email);
        }

        // GET: /Checkout/Index
        public IActionResult Index()
        {
            var musteri = GetMevcutMusteri();
            if (musteri == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // 2. Sepet kontrolü
            var sepet = _sepetService.GetirSepetByKullaniciEmail(musteri.Email);
            if (sepet == null || !sepet.SepetKalemleri.Any())
            {
                TempData["ErrorMessage"] = "Sepetiniz boş.";
                return RedirectToAction("Index", "Sepet");
            }
            var adresler = _adresService.GetirKullaniciAdresleri(musteri.Id.ToString());

            if (adresler == null || !adresler.Any())
            {
                TempData["ErrorMessage"] = "Sipariş verebilmek için lütfen önce bir adres ekleyin.";
                return RedirectToAction("Profil", "Hesabim");
            }

            var viewModel = new CheckoutViewModel
            {
                Sepet = sepet,
                KayitliAdresler = adresler,
                AdresSelectList = adresler.Select(a => new SelectListItem
                {
                    Text = $"{a.AdresBasligi} - {a.Il} / {a.Ilce}",
                    Value = a.Id.ToString()
                })
            };

            return View(viewModel);
        }

        // POST: /Checkout/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(int SecilenTeslimatAdresId, int SecilenFaturaAdresId)
        {
            var musteri = GetMevcutMusteri();
            if (musteri == null) return RedirectToAction("Login", "Account");
            if (SecilenTeslimatAdresId <= 0)
            {
                TempData["ErrorMessage"] = "Lütfen geçerli bir teslimat adresi seçin.";
                return RedirectToAction("Index");
            }
            if (SecilenFaturaAdresId <= 0) SecilenFaturaAdresId = SecilenTeslimatAdresId;

            var sepet = _sepetService.GetirSepetByKullaniciEmail(musteri.Email);
            if (sepet == null || !sepet.SepetKalemleri.Any())
            {
                return RedirectToAction("Index", "Sepet");
            }

            try
            {
                Siparis yeniSiparis = _siparisService.SiparisOlustur(sepet, SecilenTeslimatAdresId, SecilenFaturaAdresId);
                return RedirectToAction(nameof(Basarili), new { siparisId = yeniSiparis.Id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Siparis Hatası: {ex}");
                TempData["ErrorMessage"] = "Sipariş oluşturulurken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: /Checkout/Basarili/{siparisId}
        public IActionResult Basarili(int siparisId)
        {
            if (siparisId <= 0) return RedirectToAction("Index", "Home");

            var siparisDetay = _siparisService.GetirSiparisDetay(siparisId);
            if (siparisDetay == null) return NotFound();

            var musteri = GetMevcutMusteri();
            if (musteri == null || siparisDetay.MusteriId != musteri.Id)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(siparisDetay);
        }
    }
}
using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.Entities;
using ECommerceProjesi.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProjesi.WebUI.Controllers
{
    [Authorize]
    public class HesabimController : Controller
    {
        private readonly ISiparisService _siparisService;
        private readonly IMusteriService _musteriService;
        private readonly IAdresService _adresService;
        private readonly UserManager<IdentityUser> _userManager;

        public HesabimController(
            ISiparisService siparisService,
            IMusteriService musteriService,
            IAdresService adresService,
            UserManager<IdentityUser> userManager)
        {
            _siparisService = siparisService;
            _musteriService = musteriService;
            _adresService = adresService;
            _userManager = userManager;
        }

        private Musteri? GetMevcutMusteri()
        {
            var email = User.Identity?.Name;
            if (email == null) return null;
            return _musteriService.GetirMusteriByEmail(email);
        }

        // --- SİPARİŞLERİM ---
        public IActionResult Siparislerim()
        {
            var musteri = GetMevcutMusteri();
            if (musteri == null) return RedirectToAction("Index", "Home");
            var siparisler = _siparisService.GetirSiparislerByMusteriId(musteri.Id);
            return View(siparisler);
        }

        // --- SİPARİŞ DETAY ---
        public IActionResult SiparisDetay(int id)
        {
            var musteri = GetMevcutMusteri();
            var siparisDetay = _siparisService.GetirSiparisDetay(id);
            if (siparisDetay == null || musteri == null || siparisDetay.MusteriId != musteri.Id) return Forbid();
            return View("~/Views/Checkout/Basarili.cshtml", siparisDetay);
        }

        // --- PROFİL SAYFASI (GET) ---
        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            var musteri = GetMevcutMusteri();
            if (musteri == null) return RedirectToAction("Login", "Account");

            var adresler = _adresService.GetirKullaniciAdresleri(musteri.Id.ToString());

            var model = new ProfilViewModel
            {
                Ad = musteri.Ad,
                Soyad = musteri.Soyad,
                Email = musteri.Email,
                Telefon = musteri.Telefon,
                Adresler = adresler,
                YeniAdres = new Adres()
            };

            return View(model);
        }

        // --- PROFİL GÜNCELLEME ---
        [HttpPost]
        public IActionResult ProfilGuncelle(ProfilViewModel model)
        {
            var musteri = GetMevcutMusteri();
            if (musteri != null)
            {
                musteri.Ad = model.Ad;
                musteri.Soyad = model.Soyad;
                musteri.Telefon = model.Telefon;
                _musteriService.MusteriGuncelle(musteri);

                TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
            }
            return RedirectToAction("Profil");
        }

        // --- YENİ ADRES EKLEME ---
        [HttpPost]
        public IActionResult AdresEkle(ProfilViewModel model)
        {
            var musteri = GetMevcutMusteri();
            if (musteri != null && model.YeniAdres != null)
            {
                var yeniAdres = model.YeniAdres;
                yeniAdres.MusteriId = musteri.Id;
                _adresService.AdresEkle(yeniAdres);
                TempData["SuccessMessage"] = "Yeni adres eklendi.";
            }
            return RedirectToAction("Profil");
        }

        // --- ADRES SİLME ---
        public IActionResult AdresSil(int id)
        {
            var adres = _adresService.GetirAdresById(id);
            var musteri = GetMevcutMusteri();
            if (adres != null && musteri != null && adres.MusteriId == musteri.Id)
            {
                _adresService.AdresSil(adres);
            }
            return RedirectToAction("Profil");
        }

        // --- ADRES DÜZENLEME ---
        [HttpGet]
        public IActionResult AdresDuzenle(int id)
        {
            var adres = _adresService.GetirAdresById(id);
            var musteri = GetMevcutMusteri();
            if (adres == null || musteri == null || adres.MusteriId != musteri.Id)
            {
                return RedirectToAction("Profil");
            }

            return View(adres); 
        }

        [HttpPost]
        public IActionResult AdresDuzenle(Adres adres)
        {
            var mevcut = _adresService.GetirAdresById(adres.Id);
            if (mevcut != null)
            {
                mevcut.AdresBasligi = adres.AdresBasligi;
                mevcut.Il = adres.Il;
                mevcut.Ilce = adres.Ilce;
                mevcut.AcikAdres = adres.AcikAdres;
                mevcut.Ad = adres.Ad;
                mevcut.Soyad = adres.Soyad;
                mevcut.Telefon = adres.Telefon;

                _adresService.AdresGuncelle(mevcut); 
            }

            return RedirectToAction("Profil");
        }

        // --- ŞİFRE DEĞİŞTİRME SAYFASI (GET) ---
        [HttpGet]
        public IActionResult SifreDegistirme()
        {
            return View();
        }

        // --- ŞİFRE DEĞİŞTİRME İŞLEMİ (POST) ---
        [HttpPost]
        public async Task<IActionResult> SifreDegistirme(SifreDegistirmeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(user, model.EskiSifre, model.YeniSifre);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla güncellendi.";
                return RedirectToAction("Profil");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
    }
}
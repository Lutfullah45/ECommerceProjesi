using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using ECommerceProjesi.Business.Abstract; // <-- Gerekli
using ECommerceProjesi.Entities;         // <-- Gerekli
using ECommerceProjesi.DataAccess;       // <-- Gerekli (Context için)
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.UI.Services; // E-posta kullanmıyoruz
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace ECommerceProjesi.WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        // private readonly IEmailSender _emailSender; // E-posta kullanmıyoruz

        // --- YENİ EKLENEN SERVİSLER (Müşteri ve Sepet oluşturmak için) ---
        private readonly IMusteriService _musteriService;
        private readonly ECommerceContext _context;
        // --- BİTTİ ---

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            // IEmailSender emailSender,
            IMusteriService musteriService, // <-- Eklendi
            ECommerceContext context)       // <-- Eklendi
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            // _emailSender = emailSender;
            _musteriService = musteriService;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Ad alanı zorunludur.")]
            [Display(Name = "Ad")]
            [StringLength(50)]
            public string Ad { get; set; }

            [Required(ErrorMessage = "Soyad alanı zorunludur.")]
            [Display(Name = "Soyad")]
            [StringLength(50)]
            public string Soyad { get; set; }

            [Required(ErrorMessage = "Telefon alanı zorunludur.")]
            [Display(Name = "Telefon")]
            [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
            public string Telefon { get; set; }

            [Required(ErrorMessage = "E-posta alanı zorunludur.")]
            [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
            [Display(Name = "E-posta")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Şifre alanı zorunludur.")]
            [StringLength(100, ErrorMessage = "{0} en az {2} ve en fazla {1} karakter uzunluğunda olmalı.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Şifre")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Şifre Tekrar")]
            [Compare("Password", ErrorMessage = "Şifre ve şifre tekrarı eşleşmiyor.")]
            public string ConfirmPassword { get; set; }

            // --- ADRES ALANLARI ---
            [Required(ErrorMessage = "Adres Başlığı zorunludur.")]
            [Display(Name = "Adres Başlığı (Örn: Ev Adresim, İş Adresim)")]
            [StringLength(50)]
            public string AdresBasligi { get; set; }

            [Required(ErrorMessage = "İl zorunludur.")]
            [Display(Name = "İl")]
            [StringLength(50)]
            public string Il { get; set; }

            [Required(ErrorMessage = "İlçe zorunludur.")]
            [Display(Name = "İlçe")]
            [StringLength(50)]
            public string Ilce { get; set; }

            [Required(ErrorMessage = "Açık Adres zorunludur.")]
            [Display(Name = "Açık Adres (Cadde, Sokak, No vb.)")]
            [StringLength(500)]
            public string AcikAdres { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = Url.Action("Index", "Urun", new { area = "" }); // Ana sayfa (Ürünler)
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Action("Index", "Urun", new { area = "" }); // Ana sayfa (Ürünler)
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email, PhoneNumber = Input.Telefon };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Yeni kullanıcı (Identity) şifre ile oluşturuldu.");

                    try
                    {
                        // 1. E-Ticaret Müşteri (Musteriler) kaydını oluştur
                        var yeniMusteri = new Musteri
                        {
                            Ad = Input.Ad,
                            Soyad = Input.Soyad,
                            Email = Input.Email,
                            Telefon = Input.Telefon,
                            KayitTarihi = DateTime.UtcNow,
                            ParolaHash = "" // Identity kullandığımız için bu alana gerek yok
                        };
                        _musteriService.MusteriEkle(yeniMusteri); // Bu metot SaveChanges() yapıyor

                        // 2. Adresler tablosuna ekle
                        // 2. Adresler tablosuna ekle
                        var yeniAdres = new Adres
                        {
                            MusteriId = yeniMusteri.Id, // Eğer bu satırda hata alırsan 'user.Id' veya elindeki Id değişkenini yaz.
                            AdresBasligi = Input.AdresBasligi,
                            Ad = Input.Ad,
                            Soyad = Input.Soyad,
                            Telefon = Input.Telefon,
                            Il = Input.Il,
                            Ilce = Input.Ilce,
                            AcikAdres = Input.AcikAdres
                        };
                        _context.Adresler.Add(yeniAdres);

                        // 3. Yeni müşteri için boş bir sepet oluştur
                        var sepet = new Sepet { MusteriId = yeniMusteri.Id };
                        _context.Sepetler.Add(sepet);

                        // 4. Adres ve Sepet için değişiklikleri kaydet
                        await _context.SaveChangesAsync();

                        _logger.LogInformation($"Musteriler, Adresler ve Sepet tablolarına kayıt eklendi (Email: {Input.Email}).");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Musteri/Adres/Sepet tablosuna eklerken hata: {ex.Message}");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    TempData["SuccessMessage"] = "Hesabınız başarıyla oluşturuldu! Hoş geldiniz.";

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
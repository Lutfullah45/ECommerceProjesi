using System.ComponentModel.DataAnnotations;

namespace ECommerceProjesi.WebUI.Models
{
    public class SifreDegistirmeViewModel
    {
        [Required(ErrorMessage = "Eski şifrenizi girmelisiniz.")]
        [DataType(DataType.Password)]
        public string EskiSifre { get; set; }
        [Required(ErrorMessage = "Yeni şifre gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string YeniSifre { get; set; }
        [Required(ErrorMessage = "Şifre tekrarı gereklidir.")]
        [DataType(DataType.Password)]
        [Compare("YeniSifre", ErrorMessage = "Yeni şifreler uyuşmuyor.")]
        public string YeniSifreTekrar { get; set; }
    }
}
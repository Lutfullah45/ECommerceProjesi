using ECommerceProjesi.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerceProjesi.WebUI.ViewModels
{
    public class CheckoutViewModel
    {
        public Sepet Sepet { get; set; } = new Sepet();
        public List<Adres> KayitliAdresler { get; set; } = new List<Adres>();
        public IEnumerable<SelectListItem> AdresSelectList { get; set; } = new List<SelectListItem>();
        [Required(ErrorMessage = "Lütfen bir teslimat adresi seçin.")]
        public int SecilenTeslimatAdresId { get; set; }
        [Required(ErrorMessage = "Lütfen bir fatura adresi seçin.")]
        public int SecilenFaturaAdresId { get; set; }
    }
}
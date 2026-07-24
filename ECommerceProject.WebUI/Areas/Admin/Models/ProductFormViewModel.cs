using ECommerceProjesi.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ECommerceProjesi.WebUI.Areas.Admin.Models
{
    public class UrunFormViewModel
    {
        public Urun Urun { get; set; } = new Urun();
        public IEnumerable<SelectListItem> Kategoriler { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Markalar { get; set; } = new List<SelectListItem>();
    }
}
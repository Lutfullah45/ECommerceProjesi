using ECommerceProjesi.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProjesi.WebUI.ViewComponents
{
    public class KategoriMenu : ViewComponent
    {
        private readonly IKategoriService _kategoriService;
        public KategoriMenu(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService;
        }
        public IViewComponentResult Invoke()
        {
            var kategoriler = _kategoriService.TumKategorileriGetir();
            return View(kategoriler);
        }
    }
}
using ECommerceProjesi.Business.Abstract; 
using ECommerceProjesi.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace ECommerceProjesi.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUrunService _urunService;
        public HomeController(ILogger<HomeController> logger, IUrunService urunService)
        {
            _logger = logger;
            _urunService = urunService;
        }
        public IActionResult Index()
        {
            var urunler = _urunService.TumUrunleriGetir()
                                      .OrderByDescending(x => x.Id)
                                      .Take(8)
                                      .ToList();

            return View(urunler);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System;                  
using Microsoft.AspNetCore.Authorization;

namespace ECommerceProjesi.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
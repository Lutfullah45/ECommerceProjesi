using System.Security.Claims;
using ECommerceProjesi.DataAccess;
using ECommerceProjesi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProjesi.WebUI.Controllers
{
    public class YorumController : Controller
    {
        private readonly ECommerceContext _context;

        public YorumController(ECommerceContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> Ekle(int urunId, string icerik, int puan)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (puan < 1 || puan > 5) puan = 5;

            var yeniYorum = new Yorum
            {
                UrunId = urunId,
                UserId = userId,
                Icerik = icerik,
                Puan = puan,
                Tarih = DateTime.Now
            };

            _context.Yorumlar.Add(yeniYorum);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Yorumunuz ve puanınız başarıyla kaydedildi.";
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}

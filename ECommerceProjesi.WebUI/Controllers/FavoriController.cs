using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using ECommerceProjesi.DataAccess; 
using ECommerceProjesi.Entities;
using System.Security.Claims;

namespace ECommerceProjesi.Controllers
{
    [Authorize] 
    public class FavoriController : Controller
    {
        private readonly ECommerceContext _context;
        public FavoriController(ECommerceContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favoriler = await _context.Favoriler
                                          .Include(f => f.Urun)
                                          .Include(f => f.Urun.Resimler)   
                                          .Include(f => f.Urun.Varyantlar)
                                          .Where(f => f.UserId == userId)
                                          .ToListAsync();

            return View(favoriler);
        }
        [HttpPost]
        public async Task<IActionResult> EkleCikar(int urunId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var mevcutFavori = await _context.Favoriler
                .FirstOrDefaultAsync(f => f.UserId == userId && f.UrunId == urunId);

            if (mevcutFavori != null)
            {
                _context.Favoriler.Remove(mevcutFavori);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Ürün favorilerden çıkarıldı.";
            }
            else
            {
                var yeniFavori = new Favori
                {
                    UserId = userId,
                    UrunId = urunId
                };
                _context.Favoriler.Add(yeniFavori);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Ürün favorilere eklendi.";
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaeLilyDesigns.Data;

namespace BaeLilyDesigns.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var saleProducts = await _context.Products
                .Where(p => p.OriginalPrice != null)
                .ToListAsync();
            return View(saleProducts);
        }
    }
}

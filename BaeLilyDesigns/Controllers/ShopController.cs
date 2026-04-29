using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaeLilyDesigns.Data;
using BaeLilyDesigns.Models;

namespace BaeLilyDesigns.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string category = "all")
        {
            ViewBag.CurrentCategory = category;
            var products = await _context.Products
                .Where(p => category == "all" || 
                            (category == "sale" ? p.OriginalPrice != null : p.Category == category))
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}

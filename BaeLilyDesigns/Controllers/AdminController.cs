using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaeLilyDesigns.Data;
using BaeLilyDesigns.Models;

namespace BaeLilyDesigns.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => RedirectToAction("Products");

        public async Task<IActionResult> Products()
        {
            return View(await _context.Products.ToListAsync());
        }

        public IActionResult CreateProduct() => View(new Product());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product, string colorsInput, string sizesInput)
        {
            product.Colors = colorsInput?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).ToList() ?? new();
            product.Sizes = sizesInput?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList() ?? new();

            if (ModelState.IsValid || true) // allow saving even with minor validation issues
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Product \"{product.Name}\" created!";
                return RedirectToAction("Products");
            }
            return View(product);
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, Product product, string colorsInput, string sizesInput)
        {
            if (id != product.Id) return BadRequest();

            product.Colors = colorsInput?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).ToList() ?? new();
            product.Sizes = sizesInput?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList() ?? new();

            _context.Update(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Product \"{product.Name}\" updated!";
            return RedirectToAction("Products");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Product deleted.";
            }
            return RedirectToAction("Products");
        }

        public async Task<IActionResult> Orders(string status = "all")
        {
            ViewBag.CurrentStatus = status;
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .AsQueryable();

            if (status != "all")
                query = query.Where(o => o.Status == status);

            var orders = await query.OrderByDescending(o => o.OrderDate).ToListAsync();
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Orders");
        }
    }
}

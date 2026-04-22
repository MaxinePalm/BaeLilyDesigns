using Microsoft.AspNetCore.Mvc;
using BaeLilyDesigns.Models;

namespace BaeLilyDesigns.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index(string category = "all")
        {
            ViewBag.CurrentCategory = category;
            var products = ProductRepository.GetByCategory(category);
            return View(products);
        }

        public IActionResult Detail(int id)
        {
            var product = ProductRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}

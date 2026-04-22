using Microsoft.AspNetCore.Mvc;
using BaeLilyDesigns.Models;

namespace BaeLilyDesigns.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            var model = ProductRepository.GetOnSale();
            return View(model);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using BaeLilyDesigns.Models;

namespace BaeLilyDesigns.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = ProductRepository.GetBestsellers();
            return View(model);
        }
    }
}

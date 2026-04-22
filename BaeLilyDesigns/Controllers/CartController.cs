using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using BaeLilyDesigns.Models;

namespace BaeLilyDesigns.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartSessionKey);
            return json == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(json)!;
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
        }

        [HttpPost]
        public IActionResult Add(int productId, string size = "M")
        {
            var product = ProductRepository.GetById(productId);
            if (product == null || product.IsSoldOut)
                return Json(new { success = false, message = "Product not available." });

            var cart = GetCart();
            var existing = cart.FirstOrDefault(i => i.ProductId == productId && i.Size == size);
            if (existing != null)
                existing.Quantity++;
            else
                cart.Add(new CartItem {
                    ProductId = productId,
                    Name = product.Name,
                    Emoji = product.Emoji,
                    Price = product.Price,
                    Quantity = 1,
                    Size = size,
                    Color = product.Colors.FirstOrDefault() ?? ""
                });

            SaveCart(cart);
            return Json(new { success = true, message = $"{product.Name} added to bag!", cartCount = cart.Sum(i => i.Quantity) });
        }

        [HttpPost]
        public IActionResult Remove(int productId, string size = "M")
        {
            var cart = GetCart();
            cart.RemoveAll(i => i.ProductId == productId && i.Size == size);
            SaveCart(cart);
            return Json(new { success = true, cartCount = cart.Sum(i => i.Quantity) });
        }

        [HttpPost]
        public IActionResult UpdateQty(int productId, string size, int delta)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId && i.Size == size);
            if (item != null)
            {
                item.Quantity += delta;
                if (item.Quantity <= 0) cart.Remove(item);
            }
            SaveCart(cart);
            var total = cart.Sum(i => i.LineTotal);
            return Json(new { success = true, cartCount = cart.Sum(i => i.Quantity), cartTotal = total });
        }

        public IActionResult Summary()
        {
            var cart = GetCart();
            return Json(new {
                items = cart,
                total = cart.Sum(i => i.LineTotal),
                count = cart.Sum(i => i.Quantity)
            });
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            SaveCart(new List<CartItem>());
            TempData["OrderSuccess"] = true;
            return Json(new { success = true, message = "Pre-order placed! You'll receive shipping updates by email." });
        }
    }
}

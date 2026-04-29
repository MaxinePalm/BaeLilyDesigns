using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using BaeLilyDesigns.Data;
using BaeLilyDesigns.Models;
using BaeLilyDesigns.Services;

namespace BaeLilyDesigns.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _email;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, EmailService email)
        {
            _context = context;
            _userManager = userManager;
            _email = email;
        }

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
        public async Task<IActionResult> Add(int productId, string size = "M")
        {
            var product = await _context.Products.FindAsync(productId);
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
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
                return Json(new { success = false, message = "Your bag is empty." });

            if (!User.Identity!.IsAuthenticated)
                return Json(new { success = false, requireLogin = true, message = "Please sign in to place your order." });

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "User not found." });

            var total = cart.Sum(i => i.LineTotal);

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                Status = "Pending",
                Items = cart.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.Name,
                    Size = i.Size,
                    Color = i.Color,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Send order confirmation email
            var emailItems = cart.Select(i => (i.Name, i.Size, i.Quantity, i.Price)).ToList();
            try
            {
                await _email.SendOrderConfirmation(user.Email!, user.FullName, order.Id, total, emailItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email send failed: {ex.Message}");
            }

            SaveCart(new List<CartItem>());

            return Json(new { success = true, message = $"Pre-order #{order.Id} placed! Confirmation sent to {user.Email}", orderId = order.Id });
        }
    }
}

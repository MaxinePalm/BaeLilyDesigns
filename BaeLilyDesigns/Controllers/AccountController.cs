using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BaeLilyDesigns.Models;

namespace BaeLilyDesigns.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAjax([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return Json(new { success = false, message = "Email and password required." });

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var isAdmin = await _userManager.IsInRoleAsync(user!, "Admin");
                return Json(new { success = true, isAdmin, name = user!.FullName, email = user.Email });
            }
            return Json(new { success = false, message = "Invalid email or password." });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAjax([FromBody] RegisterModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.FullName))
                return Json(new { success = false, message = "All fields are required." });

            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null)
                return Json(new { success = false, message = "An account with this email already exists." });

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                await _signInManager.SignInAsync(user, isPersistent: true);
                return Json(new { success = true, name = user.FullName, email = user.Email });
            }

            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            return Json(new { success = false, message = errors });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAjax()
        {
            await _signInManager.SignOutAsync();
            return Json(new { success = true });
        }

        public IActionResult Login() => RedirectToAction("Index", "Home");
        public IActionResult AccessDenied() => View();

        [HttpGet]
        public async Task<IActionResult> Status()
        {
            if (!User.Identity!.IsAuthenticated)
                return Json(new { isLoggedIn = false });

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { isLoggedIn = false });

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            return Json(new { isLoggedIn = true, name = user.FullName, email = user.Email, isAdmin });
        }
    }

    public class LoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

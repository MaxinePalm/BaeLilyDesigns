using Microsoft.AspNetCore.Identity;
using BaeLilyDesigns.Models;

namespace BaeLilyDesigns.Data
{
    public static class DbInitializer
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.EnsureCreatedAsync();

            string[] roles = { "Admin", "Customer" };

            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            // Create default admin
            var admin = await userManager.FindByEmailAsync("admin@baelily.com");
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin@baelily.com",
                    Email = "admin@baelily.com",
                    FullName = "Bae Lily Admin",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Seed products if none exist
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new() {
                        Name = "Pearl Party", Category = "office", Emoji = "🦢",
                        Price = 1100, Description = "The hoodie that started it all. Buttery soft, structured silhouette, pearl-toned zip.",
                        Material = "70% organic cotton, 30% recycled polyester. Pearl-sheen zipper hardware.",
                        Colors = new() { "#F5F0E8", "#D4B896", "#8B6F47", "#2A2118" },
                        Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                        Stock = 150, Badge = "Top Seller"
                    },
                    new() {
                        Name = "Midnight Bloom", Category = "goth", Emoji = "🖤",
                        Price = 1250, Description = "Dark florals meet heavy hoodie energy. Oversized fit, dropped shoulders.",
                        Material = "70% organic cotton, 30% recycled polyester. Rose gold embroidery thread.",
                        Colors = new() { "#0D0D0D", "#2D1B3D", "#9B59B6", "#1A1A2E" },
                        Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                        Stock = 80
                    },
                    new() {
                        Name = "Chapter One", Category = "booktok", Emoji = "📚",
                        Price = 1100, Description = "For the reader who has strong feelings about their TBR pile.",
                        Material = "70% organic cotton, 30% recycled polyester. Vintage wash finish.",
                        Colors = new() { "#F4EDE4", "#DEB887", "#8B4513", "#2C1810" },
                        Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                        Stock = 120, Badge = "Fan Fave"
                    },
                    new() {
                        Name = "Sunshine Hours", Category = "bright", Emoji = "☀️",
                        Price = 950, OriginalPrice = 1250, Description = "Bold, citrus-pop colourblocking for the one who walks into a room and owns it.",
                        Material = "70% organic cotton, 30% recycled polyester. UV-reactive print.",
                        Colors = new() { "#FF6B00", "#FFD166", "#FFFEF0", "#1A1A00" },
                        Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                        Stock = 60, Badge = "On Sale"
                    },
                    new() {
                        Name = "Cloud Nine", Category = "kids", Emoji = "🌈",
                        Price = 750, Description = "Soft, machine-washable, and built to survive the playground.",
                        Material = "70% organic cotton, 30% recycled polyester. Non-toxic dyes.",
                        Colors = new() { "#FF5733", "#3498DB", "#2ECC71", "#F1C40F" },
                        Sizes = new() { "3-4Y", "5-6Y", "7-8Y", "9-10Y", "11-12Y" },
                        Stock = 200
                    },
                    new() {
                        Name = "Soft Power", Category = "bubbly", Emoji = "🌸",
                        Price = 1100, Description = "Pastel dreams and cozy reality. Oversized fit with adjustable ruched sleeve.",
                        Material = "70% organic cotton, 30% recycled polyester. Blush-toned metallic thread.",
                        Colors = new() { "#FF69B4", "#FFD1E8", "#FF91C8", "#2D1B3D" },
                        Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                        Stock = 90, Badge = "New"
                    },
                    new() {
                        Name = "Gravestone Garden", Category = "goth", Emoji = "🌑",
                        Price = 1350, Description = "Heavyweight goth hoodie with graveyard-print liner.",
                        Material = "70% organic cotton, 30% recycled polyester. Screen-printed interior lining.",
                        Colors = new() { "#0D0D0D", "#1A1A2E", "#7D3C98" },
                        Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                        Stock = 40
                    },
                    new() {
                        Name = "The Annotator", Category = "booktok", Emoji = "🖊️",
                        Price = 1200, OriginalPrice = 1500, Description = "Cream colourway with text-print sleeve graphics.",
                        Material = "70% organic cotton, 30% recycled polyester. Print from recycled inks.",
                        Colors = new() { "#F4EDE4", "#DEB887", "#A0522D" },
                        Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                        Stock = 55, Badge = "On Sale"
                    }
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}

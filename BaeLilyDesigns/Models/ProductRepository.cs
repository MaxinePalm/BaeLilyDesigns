namespace BaeLilyDesigns.Models
{
    public static class ProductRepository
    {
        public static List<Product> GetAll() => new List<Product>
        {
            new Product {
                Id = 1, Name = "Pearl Party", Category = "office", Emoji = "🦢",
                Price = 1100, OriginalPrice = null,
                Description = "The hoodie that started it all. Buttery soft, structured silhouette, pearl-toned zip. Wear it to the office, a gallery opening, or just to feel expensive doing nothing.",
                Material = "70% organic cotton, 30% recycled polyester. Pearl-sheen zipper hardware.",
                Colors = new() { "#F5F0E8", "#D4B896", "#8B6F47", "#2A2118" },
                Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                Stock = 150, Badge = "Top Seller"
            },
            new Product {
                Id = 2, Name = "Midnight Bloom", Category = "goth", Emoji = "🖤",
                Price = 1250, OriginalPrice = null,
                Description = "Dark florals meet heavy hoodie energy. Oversized fit, dropped shoulders, subtle embroidered rose at the cuff. For the ones who live in the in-between.",
                Material = "70% organic cotton, 30% recycled polyester. Rose gold embroidery thread.",
                Colors = new() { "#0D0D0D", "#2D1B3D", "#9B59B6", "#1A1A2E" },
                Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                Stock = 80, Badge = null
            },
            new Product {
                Id = 3, Name = "Chapter One", Category = "booktok", Emoji = "📚",
                Price = 1100, OriginalPrice = null,
                Description = "For the reader who has strong feelings about their TBR pile. Worn, vintage-feeling fleece, embossed spine detail on the sleeve. Perfect for libraries, coffee shops, and finishing just one more chapter.",
                Material = "70% organic cotton, 30% recycled polyester. Vintage wash finish.",
                Colors = new() { "#F4EDE4", "#DEB887", "#8B4513", "#2C1810" },
                Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                Stock = 120, Badge = "Fan Fave"
            },
            new Product {
                Id = 4, Name = "Sunshine Hours", Category = "bright", Emoji = "☀️",
                Price = 950, OriginalPrice = 1250,
                Description = "Bold, citrus-pop colourblocking for the one who walks into a room and owns it immediately. You are the main character. Act accordingly.",
                Material = "70% organic cotton, 30% recycled polyester. UV-reactive print.",
                Colors = new() { "#FF6B00", "#FFD166", "#FFFEF0", "#1A1A00" },
                Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                Stock = 60, Badge = "On Sale"
            },
            new Product {
                Id = 5, Name = "Cloud Nine", Category = "kids", Emoji = "🌈",
                Price = 750, OriginalPrice = null,
                Description = "Soft, machine-washable, and built to survive the playground. Rainbow colour options, extra durable stitching, and a hidden pocket for 'important things'.",
                Material = "70% organic cotton, 30% recycled polyester. Non-toxic dyes, gentle on skin.",
                Colors = new() { "#FF5733", "#3498DB", "#2ECC71", "#F1C40F" },
                Sizes = new() { "3-4Y", "5-6Y", "7-8Y", "9-10Y", "11-12Y" },
                Stock = 200, Badge = null
            },
            new Product {
                Id = 6, Name = "Soft Power", Category = "bubbly", Emoji = "🌸",
                Price = 1100, OriginalPrice = null,
                Description = "Pastel dreams and cozy reality. Oversized fit with an adjustable ruched sleeve detail. The hoodie version of a warm hug from your best friend.",
                Material = "70% organic cotton, 30% recycled polyester. Blush-toned metallic thread.",
                Colors = new() { "#FF69B4", "#FFD1E8", "#FF91C8", "#2D1B3D" },
                Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                Stock = 90, Badge = "New"
            },
            new Product {
                Id = 7, Name = "Gravestone Garden", Category = "goth", Emoji = "🌑",
                Price = 1350, OriginalPrice = null,
                Description = "Heavyweight goth hoodie with graveyard-print liner. Extra long sleeves, kangaroo pocket, and a dramatic silhouette that matches your energy.",
                Material = "70% organic cotton, 30% recycled polyester. Screen-printed interior lining.",
                Colors = new() { "#0D0D0D", "#1A1A2E", "#7D3C98" },
                Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                Stock = 40, Badge = null
            },
            new Product {
                Id = 8, Name = "The Annotator", Category = "booktok", Emoji = "🖊️",
                Price = 1200, OriginalPrice = 1500,
                Description = "Cream colourway with text-print sleeve graphics. Made for those who annotate their books and aren't apologising for it.",
                Material = "70% organic cotton, 30% recycled polyester. Print from recycled inks.",
                Colors = new() { "#F4EDE4", "#DEB887", "#A0522D" },
                Sizes = new() { "XS", "S", "M", "L", "XL", "2XL", "3XL" },
                Stock = 55, Badge = "On Sale"
            }
        };

        public static Product? GetById(int id) => GetAll().FirstOrDefault(p => p.Id == id);

        public static List<Product> GetBestsellers() => GetAll().Take(5).ToList();

        public static List<Product> GetByCategory(string category) =>
            category == "all" ? GetAll() :
            category == "sale" ? GetAll().Where(p => p.IsOnSale).ToList() :
            GetAll().Where(p => p.Category == category).ToList();

        public static List<Product> GetOnSale() => GetAll().Where(p => p.IsOnSale).ToList();
    }
}

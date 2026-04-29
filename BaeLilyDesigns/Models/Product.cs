namespace BaeLilyDesigns.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public List<string> Colors { get; set; } = new();
        public List<string> Sizes { get; set; } = new();
        public int Stock { get; set; }
        public string? Badge { get; set; }

        public bool IsOnSale => OriginalPrice.HasValue;
        public bool IsSoldOut => Stock <= 0;
    }
}
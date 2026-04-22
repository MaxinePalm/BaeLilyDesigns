namespace BaeLilyDesigns.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; } = "M";
        public string Color { get; set; } = string.Empty;

        public decimal LineTotal => Price * Quantity;
    }
}

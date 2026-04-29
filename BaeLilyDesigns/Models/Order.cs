namespace BaeLilyDesigns.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        public ApplicationUser? User { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}

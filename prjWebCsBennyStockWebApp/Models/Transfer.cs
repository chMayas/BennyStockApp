namespace prjWebCsBennyStockWebApp.Models
{
    public class Transfer
    {
        public int Id { get; set; }

        public int FromRestaurantId { get; set; }
        public Restaurant? FromRestaurant { get; set; }

        public int ToRestaurantId { get; set; }
        public Restaurant? ToRestaurant { get; set; }

        public int ItemId { get; set; }
        public Item? Item { get; set; }

        public decimal Quantity { get; set; }

        public string Status { get; set; } = "Demandé";

        
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
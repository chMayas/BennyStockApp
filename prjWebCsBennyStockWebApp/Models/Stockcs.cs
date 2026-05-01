namespace prjWebCsBennyStockWebApp.Models
{
    public class Stock
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public int ItemId { get; set; }
        public Item? Item { get; set; }

        public decimal Quantity { get; set; }
    }
}
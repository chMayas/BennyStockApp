namespace prjWebCsBennyStockWebApp.Models
{
    public class StockMovement
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item? Item { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public string Type { get; set; } = "";
        public decimal Quantite { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Reference { get; set; } = "";
    }
}
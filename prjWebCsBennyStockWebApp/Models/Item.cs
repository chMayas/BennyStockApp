namespace prjWebCsBennyStockWebApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string Unit { get; set; } = "";
        public decimal Price { get; set; }
    }
}
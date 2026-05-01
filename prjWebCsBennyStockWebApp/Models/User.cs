namespace prjWebCsBennyStockWebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Password { get; set; } = "";

        public string Role { get; set; } = ""; // Admin ou Manager

        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
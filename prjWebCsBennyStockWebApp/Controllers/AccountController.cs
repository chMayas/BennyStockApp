using Microsoft.AspNetCore.Mvc;
using prjWebCsBennyStockWebApp.Data;

namespace prjWebCsBennyStockWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password && u.IsActive);

            if (user == null)
            {
                ViewBag.Error = "Email ou mot de passe invalide.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Role", user.Role);

            if (user.RestaurantId.HasValue)
                HttpContext.Session.SetInt32("RestaurantId", user.RestaurantId.Value);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjWebCsBennyStockWebApp.Data;
using prjWebCsBennyStockWebApp.Filters;

namespace prjWebCsBennyStockWebApp.Controllers
{
    [SessionAuthorize]
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? restaurantId)
        {
            var restaurants = _context.Restaurants.ToList();
            ViewBag.Restaurants = new SelectList(restaurants, "Id", "Name");

            var stocksQuery = _context.Stocks
                .Include(s => s.Item)
                .Include(s => s.Restaurant)
                .AsQueryable();

            if (restaurantId.HasValue)
            {
                stocksQuery = stocksQuery.Where(s => s.RestaurantId == restaurantId.Value);
            }

            return View(stocksQuery.ToList());
        }

        public IActionResult Availability()
        {
            var availability = _context.Items
                .Select(item => new
                {
                    ProductName = item.Name,
                    BouchervilleQuantity = _context.Stocks
                        .Where(s => s.ItemId == item.Id && s.Restaurant.Name == "Benny Boucherville")
                        .Select(s => s.Quantity)
                        .FirstOrDefault(),

                    LaPrairieQuantity = _context.Stocks
                        .Where(s => s.ItemId == item.Id && s.Restaurant.Name == "Benny La Prairie")
                        .Select(s => s.Quantity)
                        .FirstOrDefault()
                })
                .ToList();

            return View(availability);
        }
    }
}
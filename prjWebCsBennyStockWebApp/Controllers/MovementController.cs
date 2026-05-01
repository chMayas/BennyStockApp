using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWebCsBennyStockWebApp.Data;
using prjWebCsBennyStockWebApp.Filters;

namespace prjWebCsBennyStockWebApp.Controllers
{
    [SessionAuthorize]
    public class MovementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovementController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var movements = _context.StockMovements
                .Include(m => m.Restaurant)
                .Include(m => m.Item)
                .OrderByDescending(m => m.Date)
                .ToList();

            return View(movements);
        }
    }
}
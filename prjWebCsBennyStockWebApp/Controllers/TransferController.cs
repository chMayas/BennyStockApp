using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjWebCsBennyStockWebApp.Data;
using prjWebCsBennyStockWebApp.Filters;
using prjWebCsBennyStockWebApp.Models;

namespace prjWebCsBennyStockWebApp.Controllers
{
    [SessionAuthorize]
    public class TransferController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var transfers = _context.Transfers
                .Include(t => t.FromRestaurant)
                .Include(t => t.ToRestaurant)
                .Include(t => t.Item)
                .OrderByDescending(t => t.Date)
                .ToList();

            return View(transfers);
        }

        public IActionResult Create()
        {
            ViewBag.Restaurants = new SelectList(_context.Restaurants, "Id", "Name");
            ViewBag.Items = new SelectList(_context.Items, "Id", "Name");

            return View(new Transfer
            {
                Status = "Demandé",
                Date = DateTime.UtcNow
            });
        }

        [HttpPost]
        public IActionResult Create(Transfer transfer)
        {
            ViewBag.Restaurants = new SelectList(_context.Restaurants, "Id", "Name");
            ViewBag.Items = new SelectList(_context.Items, "Id", "Name");

            if (transfer.FromRestaurantId == transfer.ToRestaurantId)
            {
                ModelState.AddModelError("", "Source et destination identiques !");
                return View(transfer);
            }

            if (transfer.Quantity <= 0)
            {
                ModelState.AddModelError("", "Quantité invalide !");
                return View(transfer);
            }

            var stockSource = _context.Stocks
                .FirstOrDefault(s => s.RestaurantId == transfer.FromRestaurantId && s.ItemId == transfer.ItemId);

            if (stockSource == null || stockSource.Quantity < transfer.Quantity)
            {
                ModelState.AddModelError("", "Stock insuffisant !");
                return View(transfer);
            }

            var stockDest = _context.Stocks
                .FirstOrDefault(s => s.RestaurantId == transfer.ToRestaurantId && s.ItemId == transfer.ItemId);

            stockSource.Quantity -= transfer.Quantity;

            if (stockDest == null)
            {
                _context.Stocks.Add(new Stock
                {
                    RestaurantId = transfer.ToRestaurantId,
                    ItemId = transfer.ItemId,
                    Quantity = transfer.Quantity
                });
            }
            else
            {
                stockDest.Quantity += transfer.Quantity;
            }

            transfer.Status = "Reçu";
            transfer.Date = DateTime.UtcNow;

            _context.Transfers.Add(transfer);

            _context.StockMovements.Add(new StockMovement
            {
                RestaurantId = transfer.FromRestaurantId,
                ItemId = transfer.ItemId,
                Type = "TRANSFER_OUT",
                Quantite = transfer.Quantity,
                Date = DateTime.UtcNow,
                Reference = "Transfert sortant"
            });

            _context.StockMovements.Add(new StockMovement
            {
                RestaurantId = transfer.ToRestaurantId,
                ItemId = transfer.ItemId,
                Type = "TRANSFER_IN",
                Quantite = transfer.Quantity,
                Date = DateTime.UtcNow,
                Reference = "Transfert entrant"
            });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
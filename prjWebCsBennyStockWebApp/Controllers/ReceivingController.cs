using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using prjWebCsBennyStockWebApp.Data;
using prjWebCsBennyStockWebApp.Filters;
using prjWebCsBennyStockWebApp.Models;

namespace prjWebCsBennyStockWebApp.Controllers
{
    [SessionAuthorize]
    public class ReceivingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ReceivingController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Create()
        {
            ViewBag.Restaurants = new SelectList(_context.Restaurants.ToList(), "Id", "Name");
            ViewBag.Items = new SelectList(_context.Items.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            int restaurantId,
            int itemId,
            decimal quantity,
            string supplier,
            IFormFile? invoiceFile)
        {
            ViewBag.Restaurants = new SelectList(_context.Restaurants.ToList(), "Id", "Name");
            ViewBag.Items = new SelectList(_context.Items.ToList(), "Id", "Name");

            if (quantity <= 0)
            {
                ModelState.AddModelError("", "La quantité doit être supérieure à 0.");
                return View();
            }

            if (string.IsNullOrWhiteSpace(supplier))
            {
                ModelState.AddModelError("", "Le fournisseur est obligatoire.");
                return View();
            }

            string invoiceFileName = "Aucune facture";

            if (invoiceFile != null && invoiceFile.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(invoiceFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Format de facture invalide. Utilisez PDF, JPG ou PNG.");
                    return View();
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "invoices");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                invoiceFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, invoiceFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await invoiceFile.CopyToAsync(stream);
                }
            }

            var stock = _context.Stocks
                .FirstOrDefault(s => s.RestaurantId == restaurantId && s.ItemId == itemId);

            if (stock == null)
            {
                stock = new Stock
                {
                    RestaurantId = restaurantId,
                    ItemId = itemId,
                    Quantity = quantity
                };

                _context.Stocks.Add(stock);
            }
            else
            {
                stock.Quantity += quantity;
            }

            _context.StockMovements.Add(new StockMovement
            {
                RestaurantId = restaurantId,
                ItemId = itemId,
                Type = "RECEIVING",
                Quantite = quantity,
                Date = DateTime.UtcNow,
                Reference = $"Livraison {supplier} - Facture: {invoiceFileName}"
            });

            _context.SaveChanges();

            return RedirectToAction("Index", "Stock");
        }
    }
}
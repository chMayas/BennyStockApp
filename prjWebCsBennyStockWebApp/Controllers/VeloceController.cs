using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using prjWebCsBennyStockWebApp.Data;
using prjWebCsBennyStockWebApp.Filters;
using prjWebCsBennyStockWebApp.Models;

namespace prjWebCsBennyStockWebApp.Controllers
{
    [SessionAuthorize]
    public class VeloceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeloceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Import()
        {
            ViewBag.Restaurants = new SelectList(_context.Restaurants.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import(int restaurantId, IFormFile? csvFile)
        {
            ViewBag.Restaurants = new SelectList(_context.Restaurants.ToList(), "Id", "Name");

            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError("", "Veuillez choisir un fichier CSV.");
                return View();
            }

            var extension = Path.GetExtension(csvFile.FileName).ToLower();

            if (extension != ".csv")
            {
                ModelState.AddModelError("", "Le fichier doit être au format CSV.");
                return View();
            }

            int totalLines = 0;
            int successLines = 0;
            int errorLines = 0;
            var errors = new List<string>();

            using var reader = new StreamReader(csvFile.OpenReadStream());

            string? header = await reader.ReadLineAsync();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                totalLines++;

                var parts = line.Split(',');

                if (parts.Length < 2)
                {
                    errorLines++;
                    errors.Add($"Ligne {totalLines}: format invalide.");
                    continue;
                }

                string code = parts[0].Trim();
                string quantityText = parts[1].Trim();

                if (!decimal.TryParse(quantityText, out decimal quantitySold) || quantitySold <= 0)
                {
                    errorLines++;
                    errors.Add($"Ligne {totalLines}: quantité invalide pour le code {code}.");
                    continue;
                }

                var item = _context.Items.FirstOrDefault(i => i.Code == code);

                if (item == null)
                {
                    errorLines++;
                    errors.Add($"Ligne {totalLines}: produit avec code {code} introuvable.");
                    continue;
                }

                var stock = _context.Stocks
                    .FirstOrDefault(s => s.RestaurantId == restaurantId && s.ItemId == item.Id);

                if (stock == null)
                {
                    errorLines++;
                    errors.Add($"Ligne {totalLines}: aucun stock trouvé pour {item.Name}.");
                    continue;
                }

                if (stock.Quantity < quantitySold)
                {
                    errorLines++;
                    errors.Add($"Ligne {totalLines}: stock insuffisant pour {item.Name}.");
                    continue;
                }

                stock.Quantity -= quantitySold;

                _context.StockMovements.Add(new StockMovement
                {
                    RestaurantId = restaurantId,
                    ItemId = item.Id,
                    Type = "SALE",
                    Quantite = quantitySold,
                    Date = DateTime.UtcNow,
                    Reference = $"Import Veloce CSV - {csvFile.FileName}"
                });

                successLines++;
            }

            await _context.SaveChangesAsync();

            ViewBag.Message = $"Import terminé. Lignes traitées: {totalLines}, succès: {successLines}, erreurs: {errorLines}.";
            ViewBag.Errors = errors;

            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using prjWebCsBennyStockWebApp.Data;
using prjWebCsBennyStockWebApp.Filters;

namespace prjWebCsBennyStockWebApp.Controllers
{
    [SessionAuthorize]
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Export()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Mayas Chabi");

            var stocks = _context.Stocks
                .Include(s => s.Restaurant)
                .Include(s => s.Item)
                .OrderBy(s => s.Restaurant!.Name)
                .ThenBy(s => s.Item!.Name)
                .ToList();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Inventaire");

            worksheet.Cells[1, 1].Value = "Restaurant";
            worksheet.Cells[1, 2].Value = "Code";
            worksheet.Cells[1, 3].Value = "Produit";
            worksheet.Cells[1, 4].Value = "Unité";
            worksheet.Cells[1, 5].Value = "Prix";
            worksheet.Cells[1, 6].Value = "Quantité";

            int row = 2;

            foreach (var stock in stocks)
            {
                worksheet.Cells[row, 1].Value = stock.Restaurant?.Name;
                worksheet.Cells[row, 2].Value = stock.Item?.Code;
                worksheet.Cells[row, 3].Value = stock.Item?.Name;
                worksheet.Cells[row, 4].Value = stock.Item?.Unit;
                worksheet.Cells[row, 5].Value = stock.Item?.Price;
                worksheet.Cells[row, 6].Value = stock.Quantity;
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var fileBytes = package.GetAsByteArray();
            var fileName = $"Inventaire_{DateTime.Now:yyyy_MM_dd_HHmm}.xlsx";

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}
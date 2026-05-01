using Microsoft.AspNetCore.Mvc;
using prjWebCsBennyStockWebApp.Filters;

namespace prjWebCsBennyStockWebApp.Controllers
{
    [SessionAuthorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
using Company.Data.Data;
using Microsoft.AspNetCore.Mvc;

namespace Company.PortalWWW.Controllers
{
    public class CartController(CompanyContext context, ILogger<CartController> logger) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

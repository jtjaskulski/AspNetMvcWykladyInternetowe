using Company.Data.Data;
using Company.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Company.PortalWWW.Controllers
{
    public class HomeController(CompanyContext context, ILogger<HomeController> logger) : Controller
    {
        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                id = context.Page.FirstOrDefault()?.IdPage;
            }
            var item = context.Page.Find(id);

            return View(item);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

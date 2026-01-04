using Company.Data.Data;
using Company.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Company.PortalWWW.Controllers
{
    public class HomeController(CompanyContext context, ILogger<HomeController> logger) : Controller
    {
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                id = context.Page.FirstOrDefault()?.IdPage;
            }
            var item = context.Page.Find(id);

            return View(item);
        }

        public async Task<IActionResult> About()
        {
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

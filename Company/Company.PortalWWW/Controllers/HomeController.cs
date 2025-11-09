using Company.Data.Data;
using Company.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Company.PortalWWW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CompanyContext _context;


        public HomeController(CompanyContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(int? id)
        {
            ViewBag.ModelPage =
                (
                    from page in _context.Page 
                    orderby page.DisplayOrder 
                    select page
                ).ToList();

            ViewBag.ModelNews =
                (
                    from news in _context.News
                    orderby news.DisplayOrder
                    select news
                ).ToList();

            if (id == null)
                id = _context.Page.FirstOrDefault()?.IdPage;
            var item = _context.Page.Find(id);

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

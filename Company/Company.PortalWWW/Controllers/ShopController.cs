using Company.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;
        private readonly CompanyContext _context;

        public ShopController(CompanyContext context, ILogger<ShopController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.TypeOfProducts = await _context.TypeOfProduct.ToListAsync();
            ViewBag.ModelNews = await _context.News.OrderBy(n => n.DisplayOrder).ToListAsync();
            if (id == null)
            {
                var firstType = await _context.TypeOfProduct.FirstAsync();
                id = firstType.IdTypeOfProduct;
            }
            return View(await _context.Product.Where(t => t.IdTypeOfProduct == id).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.TypeOfProducts = await _context.TypeOfProduct.ToListAsync();
            ViewBag.ModelNews = await _context.News.OrderBy(n => n.DisplayOrder).ToListAsync();
            var item = await _context.Product.FirstOrDefaultAsync(a => a.IdProduct == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
    }
}

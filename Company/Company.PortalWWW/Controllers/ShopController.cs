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
            if (id == null)
            {
                var firstType = await _context.TypeOfProduct.FirstAsync();
                id = firstType.IdTypeOfProduct;
            }
            ViewBag.ModelNews =
                (
                    from news in _context.News
                    orderby news.DisplayOrder
                    select news
                ).ToList();
            return View(await _context.Product.Where(t => t.IdTypeOfProduct == id).ToListAsync());
        }
    }
}

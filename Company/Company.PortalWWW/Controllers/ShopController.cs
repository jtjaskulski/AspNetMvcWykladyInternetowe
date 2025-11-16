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
            if (id == null)
            {
                var firstType = await _context.TypeOfProduct.FirstOrDefaultAsync();
                if (firstType == null)
                {
                    return NotFound();
                }
                id = firstType.IdTypeOfProduct;
            }
            return View(await _context.Product.Where(t => t.IdTypeOfProduct == id).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var item = await _context.Product.FirstOrDefaultAsync(a => a.IdProduct == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        public async Task<IActionResult> Discounts()
        {
            return View(await _context.Product
                                .Where(t => t.IsDiscount)
                                .ToListAsync());
        }
    }
}

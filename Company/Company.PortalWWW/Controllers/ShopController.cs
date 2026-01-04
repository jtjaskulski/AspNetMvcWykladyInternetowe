using Company.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Controllers
{
    public class ShopController(CompanyContext context, ILogger<ShopController> logger) : Controller
    {
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                var firstType = await context.TypeOfProduct.FirstOrDefaultAsync();
                if (firstType == null)
                {
                    return NotFound();
                }
                id = firstType.IdTypeOfProduct;
            }
            return View(await context.Product.Where(t => t.IdTypeOfProduct == id).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var item = await context.Product.FirstOrDefaultAsync(a => a.IdProduct == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        public async Task<IActionResult> Discounts()
        {
            return View(await context.Product
                                .Where(t => t.IsDiscount)
                                .ToListAsync());
        }
    }
}

using Company.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Controllers
{
    public class NewsController(CompanyContext context, ILogger<NewsController> logger) : Controller
    {
        public async Task<IActionResult> Index(int? id)
        {
            var item = await context.News.FirstOrDefaultAsync(a => a.IdNews == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
    }
}

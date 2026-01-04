using Company.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Controllers.Components
{
    public class NewsComponent(CompanyContext context, ILogger<NewsComponent> logger) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                logger.LogInformation("Start NewsComponent");
                return View("NewsComponent", await context.News.OrderBy(n => n.DisplayOrder).ToListAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in NewsComponent");
                throw;
            }
        }
    }
}
using Company.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Controllers.Components
{
    public class NavigationLinksComponent(CompanyContext context, ILogger<NavigationLinksComponent> logger) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                logger.LogInformation("Start NavigationLinksComponent");
                return View("NavigationLinksComponent", await context.Page.OrderBy(n => n.DisplayOrder).ToListAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in NavigationLinksComponent");
                throw;
            }
        }
    }
}
using Company.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Controllers.Components
{
    public class NavigationLinksComponent : ViewComponent
    {
        private readonly ILogger<NavigationLinksComponent> _logger;
        private readonly CompanyContext _context;
        public NavigationLinksComponent(CompanyContext context, ILogger<NavigationLinksComponent> logger)
        {
                _context = context;
                _logger = logger;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                _logger.LogInformation("Start NavigationLinksComponent");
                return View("NavigationLinksComponent", await _context.Page.OrderBy(n => n.DisplayOrder).ToListAsync());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in NavigationLinksComponent");
                throw;
            }
        }
    }
}
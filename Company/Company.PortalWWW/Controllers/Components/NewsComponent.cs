using Company.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Controllers.Components
{
    public class NewsComponent : ViewComponent
    {
        private readonly ILogger<NewsComponent> _logger;
        private readonly CompanyContext _context;
        public NewsComponent(CompanyContext context, ILogger<NewsComponent> logger)
        {
                _context = context;
                _logger = logger;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                _logger.LogInformation("Start NewsComponent");
                return View("NewsComponent", await _context.News.OrderBy(n => n.DisplayOrder).ToListAsync());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
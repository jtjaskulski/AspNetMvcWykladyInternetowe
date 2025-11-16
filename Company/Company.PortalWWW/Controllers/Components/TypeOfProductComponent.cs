using Company.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Controllers.Components
{
    public class TypeOfProductComponent : ViewComponent
    {
        private readonly ILogger<TypeOfProductComponent> _logger;
        private readonly CompanyContext _context;

        public TypeOfProductComponent(CompanyContext context, ILogger<TypeOfProductComponent> logger)
        {
                _context = context;
                _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                _logger.LogInformation("Start TypeOfProductComponent");
                return View("TypeOfProductComponent", await _context.TypeOfProduct.ToListAsync());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
using Company.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Controllers.Components
{
    public class TypeOfProductComponent(CompanyContext context, ILogger<TypeOfProductComponent> logger) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                logger.LogInformation("Start TypeOfProductComponent");
                return View("TypeOfProductComponent", await context.TypeOfProduct.ToListAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in TypeOfProductComponent");
                throw;
            }
        }
    }
}
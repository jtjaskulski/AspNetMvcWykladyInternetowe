using Company.Data.Data;
using Company.Data.Data.Shop;
using Company.PortalWWW.Models.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Controllers
{
    public class OrderController(CompanyContext context, ILogger<OrderController> logger) : Controller
    {
        public async Task<IActionResult> Data()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Data([Bind("Login,FirstName,LastName,Street,City,Voivodeship,ZipCode,Country,PhoneNumber,Email,Total")] Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }
            await new CartB(context, HttpContext)
                .ConvertToOrder(order, HttpContext);

            return RedirectToAction(nameof(Summary), new { id = order.IdOrder });
        }

        public async Task<IActionResult> Summary(int? id)
        {
            var order = await context.Order
                .FirstOrDefaultAsync(ord => ord.IdOrder == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
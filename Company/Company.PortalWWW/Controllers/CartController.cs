using Company.Data.Data;
using Company.PortalWWW.Models.BusinessLogic;
using Company.PortalWWW.Models.Shop;
using Microsoft.AspNetCore.Mvc;

namespace Company.PortalWWW.Controllers
{
    public class CartController(CompanyContext context, ILogger<CartController> logger) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var cartB = new CartB(context, this.HttpContext);
            var dataForCart = new CartViewModel
            {
                CartItems = await cartB.GetCartItems(),
                Total = await cartB.GetTotal()
            };
            return View(dataForCart);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var cart = new CartB(context, this.HttpContext);
            var product = await context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await cart.AddToCartHandler(product);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var cart = new CartB(context, this.HttpContext);
            await cart.DeleteFromCartHandler(id);
            return RedirectToAction("Index");
        }
    }
}
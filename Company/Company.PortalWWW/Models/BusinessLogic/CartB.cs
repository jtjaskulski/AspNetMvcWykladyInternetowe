using Company.Data.Data;
using Company.Data.Data.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Company.PortalWWW.Models.BusinessLogic
{
    public class CartB
    {
        private readonly CompanyContext _context;
        private string IdCartSession;
        public CartB(CompanyContext companyContext, HttpContext httpContext)
        {
            _context = companyContext;
            IdCartSession = GetIdCartSession(httpContext);
        }

        private string GetIdCartSession(HttpContext httpContext)
        {
            //lets check if session is null
            if (string.IsNullOrWhiteSpace(httpContext.Session.GetString("IdCartSession")))
            {
                //If user is logged in on our website
                if (!string.IsNullOrEmpty(httpContext.User?.Identity?.Name))
                {
                    httpContext.Session.SetString("IdCartSession", httpContext.User.Identity.Name);
                }
                else
                {
                    //let's generate random GUID as IdCartSession
                    var tempIdCartSession = Guid.NewGuid();
                    //set this as idsession
                    httpContext.Session.SetString("IdCartSession", tempIdCartSession.ToString());
                }
            }
            return httpContext?.Session?.GetString("IdCartSession") ?? string.Empty;
        }

        /// <summary>
        /// It handles adding product to cart!
        /// </summary>
        /// <param name="product">Product that you want to add</param>
        public async Task AddToCartHandler(Product product)
        {
            //check if item is already in cart!
            var cartItem =
                (
                from cartitem in _context.CartItem
                where cartitem.IdSessionOfCart == this.IdCartSession && cartitem.IdProduct == product.IdProduct
                select cartitem
                ).FirstOrDefault();

            //check if cartitem is null
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    IdProduct = product.IdProduct,
                    IdSessionOfCart = this.IdCartSession,
                    Product = product,
                    Quantity = 1,
                    CreatedDate = DateTime.Now
                };
                _context.CartItem.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// It handles deleting cart item!
        /// </summary>
        /// <param name="idCartItem">Id of cart item to delete</param>
        public async Task DeleteFromCartHandler(int idCartItem)
        {
            //check if item is already in cart!
            var cartItem = await _context.CartItem.FindAsync(idCartItem);
            if (cartItem != null)
            {
                _context.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// It handles getting cart items!
        /// </summary>
        /// <returns></returns>
        public async Task<List<CartItem>> GetCartItems()
            => await _context.CartItem
                .Where(item => item.IdSessionOfCart == IdCartSession)
                .Include(item => item.Product)
                .ToListAsync();

        /// <summary>
        /// It handles getting cart total!
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetTotal()
        {
            var items =
                (
                from element in _context.CartItem
                where element.IdSessionOfCart == IdCartSession
                select (decimal?)element.Quantity * element.Product.Price
                );

            return await items.SumAsync() ?? decimal.Zero;
        }

        /// <summary>
        /// It converts cart into order with order items.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<Order> ConvertToOrder(Order order, HttpContext httpContext)
        {
            order.OrderDate = DateTime.Now;
            order.Login = httpContext.User?.Identity?.Name ?? "ArturKornatkaTheBest";

            var orderPositions = await GetCartItems();
            order.Total = await GetTotal();
            await _context.AddAsync(order);

            await _context.SaveChangesAsync();

            foreach (var item in orderPositions)
            {
                if (item == null)
                {
                    continue;
                }
                var orderPosition = new OrderPosition
                {
                    IdOrder = order.IdOrder,
                    CreatedDate = DateTime.Now,
                    IdProduct = item.IdProduct,
                    Price = item.Product?.Price ?? 0,
                    Quantity = item.Quantity
                };
                await _context.AddAsync(orderPosition);
            }

            await _context.SaveChangesAsync();
            return order;
        }
    }
}
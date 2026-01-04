using Company.Data.Data.Shop;

namespace Company.PortalWWW.Models.Shop
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; } = [];
        public decimal Total { get; set; }
    }
}
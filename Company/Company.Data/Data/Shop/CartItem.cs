using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.Data.Shop
{
    public class CartItem
    {
        [Key]
        public int IdCartItem { get; set; }

        public string? IdSessionOfCart { get; set; }

        public int IdProduct { get; set; }

        [ForeignKey("IdProduct")]
        public virtual Product? Product { get; set; }

        public decimal Quantity { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
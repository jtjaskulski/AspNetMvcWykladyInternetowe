using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.Data.Shop
{
    public class OrderPosition
    {
        [Key]
        public int IdOrderPosition { get; set; }

        public int IdProduct { get; set; }

        [ForeignKey("IdProduct")]
        public virtual Product? Product { get; set; }

        public int IdOrder { get; set; }

        [ForeignKey("IdOrder")]
        public virtual Order? Order { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
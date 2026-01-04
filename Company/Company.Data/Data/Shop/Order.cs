using System.ComponentModel.DataAnnotations;

namespace Company.Data.Data.Shop
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }
        public DateTime OrderDate { get; set; }
        public string Login { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        [StringLength(160)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [Display(Name = "Street")]
        [StringLength(70)]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        [StringLength(70)]
        public string City { get; set; }

        [Required(ErrorMessage = "Voivodeship is required")]
        [Display(Name = "Voivodeship")]
        [StringLength(70)]
        public string Voivodeship { get; set; }

        [Required(ErrorMessage = "ZipCode is required")]
        [Display(Name = "ZipCode")]
        [StringLength(10)]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        [StringLength(70)]
        public string Country { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        [Display(Name = "PhoneNumber")]
        [StringLength(24)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email is not correct.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public decimal Total { get; set; }
        public virtual ICollection<OrderPosition>? OrderPosition { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Data
{
    public class ShoppingCart
    {
        [Key]
        public string IdCart { get; set; }
        [Required]
        public string IdAcc { get; set; }

        public virtual AppUser User { get; set; }
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Data
{
    public class ShoppingCartItem
    {
        [Key]
        public string IdCartItem { get; set; }
        [Required]
        public string IdCart { get; set; }
        [Required]
        public string IdProItem { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string IdPro { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual ProductItem ProductItem { get; set; }
        public virtual Product Product { get; set; }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopAPI.Data
{
    public class ProductItem
    {
        [Key]
        public string IdProItem { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public int StatusProItem { get; set; }
        [ForeignKey(nameof(IdPro))]
        public string IdPro {  get; set; }
        public virtual Product product { get; set; }
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}

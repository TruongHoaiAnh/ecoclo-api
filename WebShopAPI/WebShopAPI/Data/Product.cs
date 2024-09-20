using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Data
{
    public class Product
    {
        [Key]
        public string IdPro { get; set; }
        public string IdCate { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? BestSeller { get; set; }
        [Required]
        public int StatusProduct { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<ProductItem> ProductItems { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<ImgPro> ImgPros { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }


    }
}

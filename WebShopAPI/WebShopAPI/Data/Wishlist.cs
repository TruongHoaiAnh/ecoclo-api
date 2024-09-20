using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Data
{
    public class Wishlist
    {
        [Key]
        public string IdWishlist { get; set; }
        [Required]
        public string IdPro { get; set; }
        [Required]
        public string IdAcc { get; set; }

        public virtual AppUser user { get; set; }
        public virtual Product product { get; set; }
    }
}

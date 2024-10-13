using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopAPI.Data
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public string? Address { get; set; }

        public int? Gender { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public string Avt { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

	}
}

using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Dtos
{
    public class ProductItemDto
    {
        public string IdProItem { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}

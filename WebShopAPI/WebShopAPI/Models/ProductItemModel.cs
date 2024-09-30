using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Models
{
    public class ProductItemModel
    {

        public string Size { get; set; }

        public string Color { get; set; }

        [Required]
        public int Quantity { get; set; }

    }
}

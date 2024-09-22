using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using WebShopAPI.Data;

namespace WebShopAPI.Models
{
    public class ProductModel
    {
        public string IdCate { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int StatusProduct { get; set; }
        public List<ProductItemModel> ProductItems { get; set; }

        [Required(ErrorMessage = "File img not null!!")]
        [DisplayName("Product img")]
        public List<IFormFile> ImgFiles { get; set; }
    }
}

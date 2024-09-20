using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Models
{
    public class CategoryModel
    {
        [Required(ErrorMessage = "Category name is required.")]
        public string NameCate { get; set; }
    }
}

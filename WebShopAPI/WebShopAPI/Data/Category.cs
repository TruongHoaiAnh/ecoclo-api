using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Data
{
    public class Category
    {
        [Key]
        public string IdCate { get; set; }
        [Required]
        public string NameCate { get; set; }
        public int StatusCate { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}

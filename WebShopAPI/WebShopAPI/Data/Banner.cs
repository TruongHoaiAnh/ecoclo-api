using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopAPI.Data
{
    public class Banner
    {
        [Key]
        public string IdBanner { get; set; }
        [Required]
        [StringLength(200)]
        [RegularExpression(@"\.(gif|jpe?g|tiff?|png|webp|bmp)$", ErrorMessage = "Invalid image file format.")]
        public string LinkImg { get; set; }
    }
}

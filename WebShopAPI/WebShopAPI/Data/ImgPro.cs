using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopAPI.Data
{
    public class ImgPro
    {
        [Key]
        public string IdImg { get; set; }
        
        //.gif, .jpg, .jpeg, .tif, .tiff, .png, .webp, .bmp
        [Required(ErrorMessage = "File img name not null!!")]
        [RegularExpression(@"\.(gif|jpe?g|tiff?|png|webp|bmp)$", ErrorMessage = "Invalid image file format.")]
        public string LinkImg { get; set; }
        [Required]
        public string IdPro { get; set; }
        public virtual Product product { get; set; }
    }
}

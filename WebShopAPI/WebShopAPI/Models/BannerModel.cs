using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebShopAPI.Models
{
    public class BannerModel
    {
        [Required(ErrorMessage = "File img name not  null!!")]
        public List<IFormFile> ImgFiles { get; set; }

    }
}

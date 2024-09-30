using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Models.Profiles
{
    public class UploadFile
    {
        [Required(ErrorMessage = "File img name not null!!")]
        [DataType(DataType.Upload)]

        [Display(Name = "Chọn File")]
        public IFormFile FileUp { get; set; }
    }
}

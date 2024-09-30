using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Models.Profiles
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải lớn hơn 6 ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhật mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu phải trùng nhau")]
        public string ConfirmPassword { get; set; }
    }
}

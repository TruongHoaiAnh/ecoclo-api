using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Bắt buộc phải có mật khẩu!")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu phải trùng nhau")]
        public string? ConformPassword { get; set; }
        public string? Email { get; set; }
        public string? Code { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Just input {0}")]
        [Display(Name = "Email address or username")]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

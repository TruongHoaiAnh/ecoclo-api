using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? ClientUrl { get; set; }

    }
}

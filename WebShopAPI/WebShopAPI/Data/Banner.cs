using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopAPI.Data
{
    public class Banner
    {
        [Key]
        public int IdBanner { get; set; }
        [Required]
        [StringLength(200)]
        public string Link { get; set; }
        [StringLength(450)]
        public string Text { get; set; }
    }
}

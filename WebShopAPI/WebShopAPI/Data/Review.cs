using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopAPI.Data
{
    public class Review
    {
        [Key]
        public string IdReview { get; set; }
        [Required]
        public string IdAcc { get; set; }
        [Required]
        [ForeignKey(nameof(IdPro))]
        public string IdPro { get; set; }
        [Required]
        public double RatingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime? ReviewDate { get; set; }
        public virtual AppUser user { get; set; }
        public virtual Product product { get; set; }
    }
}

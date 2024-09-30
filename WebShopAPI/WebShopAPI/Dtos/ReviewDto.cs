using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebShopAPI.Data;

namespace WebShopAPI.Dtos
{
    public class ReviewDto
    {
        public string Username { get; set; }
        public int QuantityAll { get; set; }
        public string IdReview { get; set; }
        public string IdAcc { get; set; }
        public string IdPro { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime? ReviewDate { get; set; }
        public int? Like { get; set; }
        public int? Dislike { get; set; }
    }
}

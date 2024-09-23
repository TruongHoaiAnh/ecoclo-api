using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebShopAPI.Data;

namespace WebShopAPI.Models
{
    public class ReviewModel
    {
        public int RatingValue { get; set; }
        public string Comment { get; set; }
    }
}

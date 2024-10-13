using System.ComponentModel.DataAnnotations;
using WebShopAPI.Helpers;

namespace WebShopAPI.Models
{
	public class DiscountModel
	{
		public string? Code { get; set; }      
		public string Description { get; set; } // Mô tả mã giảm giá
		[CustomDiscountValidation] // Áp dụng custom validation
		public float DiscountAmount { get; set; } // Số tiền giảm giá hoặc % giảm giá
		public DateTime? ExpiryDate { get; set; }    // Ngày hết hạn
		public float MinimumOrderAmount { get; set; } // Số tiền đơn hàng tối thiểu để áp dụng
	}
}

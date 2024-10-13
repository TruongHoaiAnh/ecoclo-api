using System.ComponentModel.DataAnnotations;
using WebShopAPI.Helpers;

namespace WebShopAPI.Data
{
	public class Discount
	{
		[Key]
		public string IdDiscount { get; set; }
		public string Code { get; set; }        // Mã giảm giá (Voucher code)
		public string Description { get; set; } // Mô tả mã giảm giá
		[CustomDiscountValidation] // Áp dụng custom validation
		public float DiscountAmount { get; set; } // Số tiền giảm giá hoặc % giảm giá
		public DateTime? ExpiryDate { get; set; }    // Ngày hết hạn
		public int Status { get; set; }       // Trạng thái kích hoạt
		public float MinimumOrderAmount { get; set; } // Số tiền đơn hàng tối thiểu để áp dụng
		[RegularExpression(@"^\d+$", ErrorMessage = "Quantity must be a positive integer")]
		public bool? IsUsed { get; set; }        
/*		public string IdAcc { get; set; }
		public virtual AppUser User { get; set; }*/
	}
}

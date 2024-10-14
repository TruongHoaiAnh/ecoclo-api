using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Models
{
	public class CheckoutModel
	{
		public string FullName { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		[Phone]
		public string Phone { get; set; }
		public string Address { get; set; }
		public string shippingMethod { get; set; }
		public string paymentMethod { get; set; }
		public string Note { get; set; }
		public string? DiscountCode { get; set; }
	}
}

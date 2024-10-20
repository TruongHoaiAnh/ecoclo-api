using WebShopAPI.Data;
using static WebShopAPI.Utils.Constant;

namespace WebShopAPI.Dtos
{
	public class OrderDto
	{
		public string IdOrder { get; set; }
		public DateTime OrderDate { get; set; }
		public float OrderTotal { get; set; }
		public string PaymentMethod { get; set; }//COD, Bank
		public string ShippingMethod { get; set; }//GHN, GHTK
		public OrderStatus? OrderInProgress { get; set; }
		public DateTime? PendingAt { get; set; }
		public DateTime? ProcessedAt { get; set; }
		public DateTime? ShippingAt { get; set; }
		public DateTime? DoneAt { get; set; }
		public string Fullname { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string? Note { get; set; }
		public float ShippingFee { get; set; }
		public IEnumerable<OrderDetailDto> OrderDetails { get; set; }

	}
	public class OrderDetailDto
	{
		public string OrderDetailId { get; set; }
		public string IdProItem { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		public float? DiscountAmount { get; set; }
		public ProductItemDto ProductItem { get; set; }
	}
}

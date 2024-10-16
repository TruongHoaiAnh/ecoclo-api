using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Data
{
	public class OrderDetail
	{
		[Key]
		public string IdOrderDetail { get; set; }
		public string IdProItem { get; set; }
		public string IdOrder { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		public float? DiscountAmount { get; set; }
		public virtual Order Order { get; set; }
		public virtual ProductItem ProductItem { get; set; }
	}
}

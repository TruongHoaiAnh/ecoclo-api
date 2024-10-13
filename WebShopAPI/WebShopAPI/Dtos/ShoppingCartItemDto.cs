using System.ComponentModel.DataAnnotations;
using WebShopAPI.Data;

namespace WebShopAPI.Dtos
{
	public class ShoppingCartItemDto
	{
		public string IdCartItem { get; set; }
		public string IdCart { get; set; }
		public string IdProItem { get; set; }
		public int Quantity { get; set; }
		public double? Price { get; set; }
		public string IdPro { get; set; }
		public virtual Product Product { get; set; }

	}
}

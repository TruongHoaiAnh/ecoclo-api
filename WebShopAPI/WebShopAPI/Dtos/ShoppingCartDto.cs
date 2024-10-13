using System.ComponentModel.DataAnnotations;
using WebShopAPI.Data;

namespace WebShopAPI.Dtos
{
	public class ShoppingCartDto
	{
		public string IdCart { get; set; }
		public string IdAcc { get; set; }

		public virtual ICollection<ShoppingCartItemDto> ShoppingCartItems { get; set; }
	}
}

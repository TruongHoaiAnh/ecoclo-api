using System.ComponentModel.DataAnnotations;
using static WebShopAPI.Utils.Constant;

namespace WebShopAPI.Data
{
	public class Order
	{
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        [Key]
		public string IdOrder { get; set; }
		public string? IdAcc { get; set; }
		public DateTime OrderDate { get; set; }
		public string PaymentMethod { get; set; }//COD, Bank
		public string ShippingMethod { get; set; }//GHN, GHTK
		public string Address { get; set; }
        public float OrderTotal { get; set; }
        public int Status { get; set; }
		public int? OrderStart { get; set; }
		public OrderStatus? OrderInProgress { get; set; }
		public DateTime? PendingAt { get; set; }
		public DateTime? ProcessedAt { get; set; }
		public DateTime? ShippingAt { get; set; }
		public DateTime? DoneAt { get; set; }
		public int? OrderEnd { get; set; }
		public string Phone { get; set; }
		public string Fullname { get; set; }
		public string Email { get; set; }
		public string Note { get; set; }
		public float ShippingFee { get; set; }
		public virtual AppUser User { get; set; }
		public virtual ICollection<OrderDetail> OrderDetails { get; set; }
	}
}

namespace WebShopAPI.Utils
{
    public class Constant
    {

        public enum OrderStatus
		{
			Pending,
			Processed,
			Shipping,
            Done,
			Cancel
		}
        public enum StatusUser
        {
            unBlock = 0,
            block = 1
        }
        public enum StatusProduct
        {
            unBlock = 0,
            block = 1
        }
        public enum StatusProductItem
        {
            unBlock = 0,
            block = 1
        }
    }
}

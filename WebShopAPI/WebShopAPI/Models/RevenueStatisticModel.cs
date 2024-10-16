namespace WebShopAPI.Models
{
    public class RevenueStatisticModel
    {
        public string Period { get; set; }
        public float TotalRevenue { get; set; }
        public int TotalOrders { get; set; }  // Số lượng đơn hàng
        public int TotalProductsSold { get; set; } // Số lượng sản phẩm đã bán
        public string Type { get; set; } // "daily", "monthly", "yearly"
    }
}

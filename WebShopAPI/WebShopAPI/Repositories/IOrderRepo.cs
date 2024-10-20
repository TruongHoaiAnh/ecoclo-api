using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Migrations;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface IOrderRepo
    {
        Task<IEnumerable<RevenueStatisticModel>> GetRevenueStatistic(string fromDate, string toDate, string statisticType);
		Task<IEnumerable<Order>> GetOrdersByStatus(Utils.Constant.OrderStatus status);
        Task<ApiResponse> UpdateOrderStatus(string orderId, Utils.Constant.OrderStatus status);
        Task<OrderDetail> GetOrderDetail(string orderId);
		Task<ApiResponse> GetUserOrdersByStatus(Utils.Constant.OrderStatus status);
        Task<Order> GetOrderById(string orderId);


	}
}

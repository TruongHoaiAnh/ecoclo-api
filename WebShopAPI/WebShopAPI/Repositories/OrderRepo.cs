using AutoMapper;
using MailKit.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Migrations;
using WebShopAPI.Models;
using WebShopAPI.Utils;

namespace WebShopAPI.Repositories
{
	public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
		public OrderRepo(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
		{
			_context = context;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_userManager = userManager;
		}

		public async Task<IEnumerable<Order>> GetOrdersByStatus(Utils.Constant.OrderStatus status)
		{
			var orders = await _context.orders
		.Where(o => o.Status == 0 && o.OrderInProgress == status)
        .Include(o => o.OrderDetails)
        .ThenInclude(od => od.ProductItem)
        .ThenInclude(pi => pi.product)
		.ToListAsync();
            if (orders == null) 
            {
                return null;
            }
			return orders;
		}   
        public async Task<ApiResponse> UpdateOrderStatus(string orderId, Utils.Constant.OrderStatus status)
		{
			var order = await _context.orders.FirstOrDefaultAsync(o => o.IdOrder == orderId);
			if (order != null)
			{
                switch (status)
                {
					case Utils.Constant.OrderStatus.Processed:
						order.ProcessedAt = DateTime.Now;
						break;
					case Utils.Constant.OrderStatus.Shipping:
						order.ShippingAt = DateTime.Now;
						break;
					case Utils.Constant.OrderStatus.Done:
						order.DoneAt = DateTime.Now;
						break;
				}
				order.OrderInProgress = status;
				await _context.SaveChangesAsync();
                return new ApiResponse { Success = true, Message = "Order status updated successfully" };
			}
            return new ApiResponse { Success = false, Message = "Order not found" };
            
		}
        public async Task<OrderDetail> GetOrderDetail(string orderId)
		{
			var orderDetail = await _context.orderDetails
		.Include(od => od.ProductItem)
		.ThenInclude(pi => pi.product)
		.Where(od => od.IdOrder == orderId)
		.FirstOrDefaultAsync();
			if (orderDetail != null)
			{
				return orderDetail;
			}
			return null;
		}


		public async Task<IEnumerable<RevenueStatisticModel>> GetRevenueStatistic(string fromDate, string toDate, string statisticType)
        {
            DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            IEnumerable<RevenueStatisticModel> revenueStatistics;

            // Query order details separately to calculate total products sold for each order
            var orderDetailsData = _context.orderDetails
                .GroupBy(od => od.IdOrder)
                .Select(group => new
                {
                    IdOrder = group.Key,
                    TotalProductsSold = group.Sum(od => od.Quantity) // Sum quantities for each order
                });

            // Thống kê theo ngày
            if (statisticType == "daily")
            {
                revenueStatistics = await  _context.orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Status == 0)
                    .Join(orderDetailsData, o => o.IdOrder, od => od.IdOrder, (o, od) => new { o, od.TotalProductsSold })
                    .GroupBy(joined => joined.o.OrderDate.Date)
                    .Select(group => new RevenueStatisticModel
                    {
                        Period = group.Key.ToString("yyyy-MM-dd"),
                        TotalRevenue = group.Sum(x => x.o.OrderTotal),
                        TotalOrders = group.Count(),
                        TotalProductsSold = group.Sum(x => x.TotalProductsSold),
                        Type = "daily"
                    })
                    .ToListAsync();
            }
            // Thống kê theo tháng
            else if (statisticType == "monthly")
            {
                revenueStatistics = await _context.orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Status == 0)
                    .Join(orderDetailsData, o => o.IdOrder, od => od.IdOrder, (o, od) => new { o, od.TotalProductsSold })
                    .GroupBy(joined => new { joined.o.OrderDate.Year, joined.o.OrderDate.Month })
                    .Select(group => new RevenueStatisticModel
                    {
                        Period = $"{group.Key.Year}-{group.Key.Month:D2}", // "yyyy-MM" 
                        TotalRevenue = group.Sum(x => x.o.OrderTotal),
                        TotalOrders = group.Count(),
                        TotalProductsSold = group.Sum(x => x.TotalProductsSold),
                        Type = "monthly"
                    })
                    .ToListAsync();
            }
            // Thống kê theo năm
            else if (statisticType == "yearly")
            {
                revenueStatistics = await _context.orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Status == 0)
                    .Join(orderDetailsData, o => o.IdOrder, od => od.IdOrder, (o, od) => new { o, od.TotalProductsSold })
                    .GroupBy(joined => joined.o.OrderDate.Year)
                    .Select(group => new RevenueStatisticModel
                    {
                        Period = group.Key.ToString(), // "yyyy"
                        TotalRevenue = group.Sum(x => x.o.OrderTotal),
                        TotalOrders = group.Count(),
                        TotalProductsSold = group.Sum(x => x.TotalProductsSold),
                        Type = "yearly"
                    })
                    .ToListAsync();
            }
            else
            {
                throw new ArgumentException("Invalid statistic type. Allowed values: daily, monthly, yearly.");
            }

            return  revenueStatistics;
        }

		public async Task<ApiResponse> GetUserOrdersByStatus(Constant.OrderStatus status)
		{
			var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            if(userId == null)
			{
                return new ApiResponse { Success = false, Message = "User not found", ErrorCode = "401" };
			}
			var orders = await _context.orders
		                .Where(o => o.Status == 0 && o.OrderInProgress == status && o.IdAcc == userId)
		                .Include(o => o.OrderDetails)
		                .ThenInclude(od => od.ProductItem)
		                .ThenInclude(pi => pi.product)
		                .ToListAsync();
			if (orders == null)
			{
                return new ApiResponse { Success = false, Message = "No orders found", ErrorCode = "404" };
			}
            return new ApiResponse { Success = true, Data = orders };
		}

		public async Task<Order> GetOrderById(string orderId)
		{
			var order = await _context.orders
				.Where(o => o.IdOrder == orderId && o.Status == 0)
				.FirstOrDefaultAsync();
            if(order == null)
            {
                return null;
            }
            return order;
		}
	}
}

using System.Globalization;
using WebShopAPI.Data;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;
        public OrderRepo(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<RevenueStatisticModel> GetRevenueStatistic(string fromDate, string toDate, string statisticType)
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
                revenueStatistics = _context.orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.OrderStatus == 0)
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
                    .ToList();
            }
            // Thống kê theo tháng
            else if (statisticType == "monthly")
            {
                revenueStatistics = _context.orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.OrderStatus == 0)
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
                    .ToList();
            }
            // Thống kê theo năm
            else if (statisticType == "yearly")
            {
                revenueStatistics = _context.orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.OrderStatus == 0)
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
                    .ToList();
            }
            else
            {
                throw new ArgumentException("Invalid statistic type. Allowed values: daily, monthly, yearly.");
            }

            return revenueStatistics;
        }


    }
}

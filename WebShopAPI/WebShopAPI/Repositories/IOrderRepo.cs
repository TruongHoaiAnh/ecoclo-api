using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface IOrderRepo
    {
        IEnumerable<RevenueStatisticModel> GetRevenueStatistic(string fromDate, string toDate, string statisticType);
    }
}

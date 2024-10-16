using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopAPI.Repositories;

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        public RevenueController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpGet("revenue-statistics")]
        public IActionResult GetRevenueStatistic(string fromDate, string toDate, string statisticType)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(fromDate) || string.IsNullOrWhiteSpace(toDate) || string.IsNullOrWhiteSpace(statisticType))
                {
                    return BadRequest("Invalid input parameters.");
                }
                var result = _orderRepo.GetRevenueStatistic(fromDate, toDate, statisticType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}

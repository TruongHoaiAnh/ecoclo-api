using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Models;
using WebShopAPI.Repositories;

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize(Roles = AppRole.Administrator)]

	public class RevenueController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        public RevenueController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpGet("revenue-statistics")]
        public async Task<IActionResult> GetRevenueStatistic(string fromDate, string toDate, string statisticType)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(fromDate) || string.IsNullOrWhiteSpace(toDate) || string.IsNullOrWhiteSpace(statisticType))
                {
                    return BadRequest("Invalid input parameters.");
                }
                var result = await _orderRepo.GetRevenueStatistic(fromDate, toDate, statisticType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
		[HttpGet("ExportRevenue")]
		public async Task<IActionResult> ExportRevenue(string fromDate, string toDate, string statisticType)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(fromDate) || string.IsNullOrWhiteSpace(toDate) || string.IsNullOrWhiteSpace(statisticType))
				{
					return BadRequest("Invalid input parameters.");
				}

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				// Giả sử GetRevenueStatistic trả về một danh sách RevenueStatisticModel
				var revenueData = await _orderRepo.GetRevenueStatistic(fromDate, toDate, statisticType);
				var revenueExportList = revenueData.ToList(); // Chuyển đổi sang danh sách nếu cần thiết

				var stream = new MemoryStream();
				using (var package = new ExcelPackage(stream))
				{
					var workSheet = package.Workbook.Worksheets.Add("SheetRevenue");

					// Thêm tiêu đề cho các cột
					workSheet.Cells[1, 1].Value = "Thời gian";
					workSheet.Cells[1, 2].Value = "Tổng doanh thu";
					workSheet.Cells[1, 3].Value = "Tổng đơn hàng";
					workSheet.Cells[1, 4].Value = "Tổng sản phẩm đã bán";
					workSheet.Cells[1, 5].Value = "Loại thống kê";

					// Ghi dữ liệu vào worksheet
					int row = 2; // Bắt đầu ghi dữ liệu từ hàng thứ 2
					foreach (var revenue in revenueExportList)
					{
						workSheet.Cells[row, 1].Value = revenue.Period;
						workSheet.Cells[row, 2].Value = revenue.TotalRevenue;
						workSheet.Cells[row, 3].Value = revenue.TotalOrders;
						workSheet.Cells[row, 4].Value = revenue.TotalProductsSold;
						workSheet.Cells[row, 5].Value = revenue.Type;

						row++;
					}

					package.Save();
				}

				// Reset the stream position to the beginning
				stream.Position = 0;

				var fileName = $"Revenue_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

	}
}

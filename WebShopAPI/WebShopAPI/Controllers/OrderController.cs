using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PagedList;
using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Repositories;

namespace WebShopAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = AppRole.Administrator)]

	public class OrderController : ControllerBase
	{
		private readonly IOrderRepo _orderRepo;

		public OrderController(IOrderRepo orderRepo)
		{
			_orderRepo = orderRepo;
		}

		// Lấy danh sách đơn hàng theo trạng thái
		[HttpGet("OrderProccess/{status}")]
		public async Task<IActionResult> GetOrdersByStatus(Utils.Constant.OrderStatus status, int? pageIndex)
		{
			var pageNumber = pageIndex == null || pageIndex <= 0 ? 1 : pageIndex.Value;
			var pageSize = 10;
			var orders = await _orderRepo.GetOrdersByStatus(status); 
			if (orders == null || !orders.Any())
			{
				return NotFound(new { Message = "Không có đơn hàng nào ở trạng thái này." });
			}
			PagedList<Order> orderPaging = new PagedList<Order>(orders.AsQueryable(), pageNumber, pageSize);

			return Ok(orderPaging);  
		}

		// Cập nhật trạng thái đơn hàng
		[HttpPut("UpdateOrderProccess/{orderId}")]
		public async Task<IActionResult> UpdateOrderStatus(string orderId, [FromBody] Utils.Constant.OrderStatus status)
		{
			try
			{
				var result = await _orderRepo.UpdateOrderStatus(orderId, status);
				if (result.Success)
				{
					return Ok(result);
				}

				return BadRequest(result);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}

		[HttpGet("OrderDetail/{orderId}")]
		public async Task<IActionResult> GetOrderDetail(string orderId)
		{
			var orderDetail = await _orderRepo.GetOrderDetail(orderId);
			if (orderDetail == null)
			{
				return NotFound();
			}
			return Ok(orderDetail);
		}

		[HttpGet("ExportOrder")]
		public async Task<IActionResult> ExportOrder()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			var orders = await _orderRepo.GetOrdersByStatus(Utils.Constant.OrderStatus.Shipping);
			var orderExportList = new List<OrderDto>();
			foreach (var order in orders)
			{
				var orderDto = new OrderDto
				{
					IdOrder = order.IdOrder,
					OrderDate = order.OrderDate,
					OrderTotal = order.OrderTotal,
					OrderInProgress = order.OrderInProgress,
					PendingAt = order.PendingAt,
					ProcessedAt = order.ProcessedAt,
					ShippingAt = order.ShippingAt,
					DoneAt = order.DoneAt,
					Fullname = order.Fullname,
					Email = order.Email,
					Phone = order.Phone,
					Note = order.Note,
					ShippingFee = order.ShippingFee,
					PaymentMethod = order.PaymentMethod,
					ShippingMethod = order.ShippingMethod,
					OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
					{
						OrderDetailId = od.IdOrderDetail,
						IdProItem = od.ProductItem.IdProItem,
						ProductName = od.ProductItem.product.Name,
						Quantity = od.Quantity,
						Price = od.Price
					}).ToList()
				};

				orderExportList.Add(orderDto);
			}
			var stream = new MemoryStream();
			using (var package = new ExcelPackage(stream))
			{
				var workSheet = package.Workbook.Worksheets.Add("SheetOrder");

				// Thêm tiêu đề cho các cột
				workSheet.Cells[1, 1].Value = "Mã đơn hàng";             // IdOrder
				workSheet.Cells[1, 2].Value = "Ngày đặt hàng";          // OrderDate
				workSheet.Cells[1, 3].Value = "Tổng tiền";              // OrderTotal
				workSheet.Cells[1, 4].Value = "Trạng thái đơn hàng";    // OrderInProgress
				workSheet.Cells[1, 5].Value = "Thời gian chờ";          // PendingAt
				workSheet.Cells[1, 6].Value = "Thời gian xử lý";        // ProcessedAt
				workSheet.Cells[1, 7].Value = "Thời gian giao hàng";    // ShippingAt
				workSheet.Cells[1, 8].Value = "Thời gian hoàn tất";     // DoneAt
				workSheet.Cells[1, 9].Value = "Họ tên";                  // Fullname
				workSheet.Cells[1, 10].Value = "Email";                 // Email
				workSheet.Cells[1, 11].Value = "Số điện thoại";          // Phone
				workSheet.Cells[1, 12].Value = "Ghi chú";                // Note
				workSheet.Cells[1, 13].Value = "Phí giao hàng";          // ShippingFee
				workSheet.Cells[1, 14].Value = "Phương thức thanh toán"; // PaymentMethod
				workSheet.Cells[1, 15].Value = "Phương thức vận chuyển"; // ShippingMethod
				workSheet.Cells[1, 16].Value = "Mã chi tiết đơn hàng";   // OrderDetailId
				workSheet.Cells[1, 17].Value = "Mã sản phẩm";            // IdProItem
				workSheet.Cells[1, 18].Value = "Tên sản phẩm";           // ProductName
				workSheet.Cells[1, 19].Value = "Số lượng";               // Quantity
				workSheet.Cells[1, 20].Value = "Giá";                    // Price

				int row = 2;

				// Ghi dữ liệu vào worksheet
				foreach (var order in orderExportList)
				{
					foreach (var detail in order.OrderDetails)
					{
						workSheet.Cells[row, 1].Value = order.IdOrder;
						workSheet.Cells[row, 2].Value = order.OrderDate.ToString("yyyy-MM-dd");
						workSheet.Cells[row, 3].Value = order.OrderTotal;
						workSheet.Cells[row, 4].Value = order.OrderInProgress;
						workSheet.Cells[row, 5].Value = order.PendingAt?.ToString("yyyy-MM-dd HH:mm:ss");
						workSheet.Cells[row, 6].Value = order.ProcessedAt?.ToString("yyyy-MM-dd HH:mm:ss");
						workSheet.Cells[row, 7].Value = order.ShippingAt?.ToString("yyyy-MM-dd HH:mm:ss");
						workSheet.Cells[row, 8].Value = order.DoneAt?.ToString("yyyy-MM-dd HH:mm:ss");
						workSheet.Cells[row, 9].Value = order.Fullname;
						workSheet.Cells[row, 10].Value = order.Email;
						workSheet.Cells[row, 11].Value = order.Phone;
						workSheet.Cells[row, 12].Value = order.Note;
						workSheet.Cells[row, 13].Value = order.ShippingFee;
						workSheet.Cells[row, 14].Value = order.PaymentMethod;
						workSheet.Cells[row, 15].Value = order.ShippingMethod;

						// Ghi chi tiết đơn hàng
						workSheet.Cells[row, 16].Value = detail.OrderDetailId;
						workSheet.Cells[row, 17].Value = detail.IdProItem;
						workSheet.Cells[row, 18].Value = detail.ProductName;
						workSheet.Cells[row, 19].Value = detail.Quantity;
						workSheet.Cells[row, 20].Value = detail.Price;

						row++;
					}
				}

				package.Save();
			}


			// Reset the stream position to the beginning
			stream.Position = 0;

			var fileName = $"Order_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
			return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
		}



	}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PagedList;
using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Repositories;

namespace WebShopAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = AppRole.User)]
	public class UserOrderController : ControllerBase
	{
		private readonly IOrderRepo _orderRepo;
		public UserOrderController(IOrderRepo orderRepo)
		{
			_orderRepo = orderRepo;
		}
		// Lấy danh sách đơn hàng theo trạng thái
		[HttpGet("OrderProccess/{status}")]
		public async Task<IActionResult> GetOrdersByStatus(Utils.Constant.OrderStatus status, int? pageIndex)
		{
			var pageNumber = pageIndex == null || pageIndex <= 0 ? 1 : pageIndex.Value;
			var pageSize = 10;
			var ordersResponse = await _orderRepo.GetUserOrdersByStatus(status);
			var orders = ordersResponse.Data as List<Order>;
			if (orders == null || !orders.Any())
			{
				return NotFound(new { Message = "Không có đơn hàng nào ở trạng thái này." });
			}
			PagedList<Order> orderPaging = new PagedList<Order>(orders.AsQueryable(), pageNumber, pageSize);

			return Ok(orderPaging);
		}

		// Hủy đơn hàng đơn hàng
		[HttpPut("CancelOrder/{orderId}")]
		public async Task<IActionResult> CancelOrder(string orderId)
		{
			try
			{
				var order = await _orderRepo.GetOrderById(orderId);
				if (order == null)
				{
					return NotFound(new { Message = "Không tìm thấy đơn hàng." });
				}
				var result = new ApiResponse();
				if(order.OrderInProgress == Utils.Constant.OrderStatus.Pending)
				{
					result = await _orderRepo.UpdateOrderStatus(orderId, Utils.Constant.OrderStatus.Cancel);
				}
				else
				{
					result.Success = false;
					result.Message = "Không thể hủy đơn hàng đã được xử lý.";
				}
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
	}
}

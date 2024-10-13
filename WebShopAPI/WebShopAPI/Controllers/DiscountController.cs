using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PagedList;
using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;
using WebShopAPI.Repositories;

namespace WebShopAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DiscountController : ControllerBase
	{
		private readonly IDiscountRepo _discountRepo;

		public DiscountController(IDiscountRepo discountRepo)
		{
			_discountRepo = discountRepo;
		}
		[HttpGet]
		[Authorize(Roles = AppRole.Administrator)]
		public async Task<IActionResult> GetAll(int? page)
		{
			var pageNumber = page == null || page <= 0 ? 1 : page.Value;
			var pageSize = 10;
			var discounts = await _discountRepo.GetAll();
			PagedList<Discount> discountPagelists = new PagedList<Discount>(discounts.AsQueryable(), pageNumber, pageSize);
			return Ok(discountPagelists);
		}
		[HttpPost]
		[Authorize(Roles = AppRole.Administrator)]
		public async Task<IActionResult> Create([FromBody] DiscountModel model)
		{
			var response = await _discountRepo.Create(model);
			if (response.Success)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}
		[HttpPut("{id}")]
		[Authorize(Roles = AppRole.Administrator)]
		public async Task<IActionResult> Update(string id, DiscountModel discount)
		{
			var response = await _discountRepo.Update(id, discount);
			if (response.Success)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}
		[HttpDelete("{id}")]
		[Authorize(Roles = AppRole.Administrator)]
		public async Task<IActionResult> DeleteById(string id)
		{
			var response = await _discountRepo.DeleteById(id);
			if (response.Success)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}
	}
}

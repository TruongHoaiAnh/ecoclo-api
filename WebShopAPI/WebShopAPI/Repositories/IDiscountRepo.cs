using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
	public interface IDiscountRepo
	{
		public Task<List<Discount>> GetAll();
		public Task<Discount> GetById(string id);
		public Task<ApiResponse> Create(DiscountModel model);
		public Task<ApiResponse> Update(string id, DiscountModel discount);
		public Task<ApiResponse> DeleteById(string id);
	}
}

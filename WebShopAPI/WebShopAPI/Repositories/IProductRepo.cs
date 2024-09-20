using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface IProductRepo
    {
        public Task<List<Product>> GetAll(string? searchString, string? IdCate, float? from, float? to);
        public Task<ApiResponse> Create(ProductModel model);
        public Task Update(ProductModel model, string id);
        public Task<ApiResponse> DeleteById(string id);
    }
}

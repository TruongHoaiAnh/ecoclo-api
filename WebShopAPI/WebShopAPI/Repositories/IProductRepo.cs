using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface IProductRepo
    {
        public Task<List<ProductDto>> GetAll(string? searchString, string? IdCate, float? from, float? to);
        public Task<ProductDto> GetById(string idPro);
        public Task<ApiResponse> Create(ProductModel model);
        public Task<ApiResponse> Update(string idPro, ProductModel model);
        public Task<ApiResponse> DeleteById(string id);
    }
}

using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface ICategoryRepo
    {
        public Task<List<Category>> GetAll();
        public Task<Category> GetById(string id);
        public Task<ApiResponse> Create(CategoryModel model);
        public Task<ApiResponse> Update(CategoryModel model, string id);
        public Task DeleteById(string id);
    }
}

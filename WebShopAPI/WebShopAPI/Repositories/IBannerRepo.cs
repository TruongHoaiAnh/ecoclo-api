using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface IBannerRepo
    {
        public Task<List<Banner>> GetAllBanner();
        public Task<ApiResponse> AddBanner(BannerModel model);
        public Task<ApiResponse> Remove(string idBanner);
    }
}

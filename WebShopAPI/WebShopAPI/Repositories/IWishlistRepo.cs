using System.Security.Claims;
using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface IWishlistRepo
    {
        public Task<List<Product>> GetAllWishlist(string idUser);
        public Task<ApiResponse> CreateWishlist(string idUser, string idPro);
        public Task<ApiResponse> DeleteWishlist(string idPro);
    }
}

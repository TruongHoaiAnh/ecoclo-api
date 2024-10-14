using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface IShoppingCartRepo
    {
        public Task<ApiResponse> GetAll();
        public Task<ApiResponse> AddToCart(string idPro, string idProItem, int quantity, float price);
        public Task<ApiResponse> RemoveFromCart(string idCartItem);
        public Task<ApiResponse> UpdateQuantity(string idProItem, int quantity);
        public Task<ApiResponse> CheckoutCOD(CheckoutModel model);
    }
}

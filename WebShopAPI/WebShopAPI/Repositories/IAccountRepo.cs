using Microsoft.AspNetCore.Identity;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface IAccountRepo
    {
        Task<IdentityResult> SignUpAsync(SignUpModel model);
        Task<ApiResponse> SignInAsync(SignInModel model);
    }
}

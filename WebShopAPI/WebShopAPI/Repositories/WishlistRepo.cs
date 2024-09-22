using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebShopAPI.Data;
using WebShopAPI.Helpers;

namespace WebShopAPI.Repositories
{
    public class WishlistRepo : IWishlistRepo
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public WishlistRepo(AppDbContext context, UserManager<AppUser> userManager) {
            _context = context;
            _userManager = userManager; 
        }
        public async Task<ApiResponse> CreateWishlist(string idUser, string idPro)
        {
            try
            {
                var existWishlistItem = await _context.wishlists.FirstOrDefaultAsync(x => x.IdAcc == idUser && x.IdPro == idPro);
                if (existWishlistItem == null)
                {
                    var wishlist = new Wishlist
                    {
                        IdWishlist = GenerateNextIdWishlist(),
                        IdPro = idPro,
                        IdAcc = idUser,
                    };
                    await _context.wishlists.AddAsync(wishlist);
                    await _context.SaveChangesAsync();
                    return new ApiResponse { Success = true, Message = "Wishlist created successfully." };
                }
                return new ApiResponse { Success = false, Message = "This product have already in wishlist" };
            }
            catch (Exception ex) {
                return new ApiResponse { Success = false, Message = $"Error: {ex.Message}" };
            }
            
        }

        public async Task<ApiResponse> DeleteWishlist(string idProduct)
        {
            var wishlist = await _context.wishlists.FirstOrDefaultAsync(p => p.IdPro == idProduct);

            if (wishlist == null)
            {
                return new ApiResponse { Success = false, Message = "There is no favorite product to delete" };
            }

            // Xóa sản phẩm khỏi wishlist
            _context.wishlists.Remove(wishlist);
            await _context.SaveChangesAsync(); 
            return new ApiResponse { Success = true, Message = "Wishlist removed successfully." };
        }


        public async Task<List<Product>> GetAllWishlist(string idUser)
        {
            var wishlist = await _context.wishlists
                .Where(w => w.IdAcc == idUser) // Lọc wishlist theo idUser
                .Select(w => w.product) // Chỉ lấy thông tin sản phẩm từ wishlist
                .ToListAsync();
            return wishlist; // Trả về danh sách sản phẩm đã yêu thích

        }

        public string GenerateNextIdWishlist()
        {
            // Retrieve the maximum existing Id_pro
            string maxIdWishlist = _context.wishlists
                .Select(p => p.IdWishlist)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            // Generate the next Id_pro
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdWishlist))
            {
                string numericPart = maxIdWishlist.Substring(2); // Extract numeric part
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + 1;
                }
            }

            string nextIdAcc = $"WL{nextNumber:D3}"; // Format the new Id_acc

            return nextIdAcc;
        }
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WebShopAPI.Data;
using WebShopAPI.Helpers;

namespace WebShopAPI.Repositories
{
    public class ShoppingCartRepo : IShoppingCartRepo
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartRepo(AppDbContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse> GetAll()
        {
            List<ShoppingCartItem> shopCartItems = null;

            string userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            if (userId != null)
            {
                ShoppingCart cart = _context.shoppingCarts.SingleOrDefault(c => c.IdAcc == userId);
                if (cart != null)
                {
                    shopCartItems = _context.shoppingCartItems.Include(p => p.Product).Include(e => e.Product.ImgPros).Include(pr => pr.Product.ProductItems)
                                .Where(item => item.IdCart == cart.IdCart).ToList();


                    return new ApiResponse { Success = true, Data = shopCartItems};
                }
                return new ApiResponse { Success = false, Message = "Gio hang rong" };

            }
            else
            {

                return new ApiResponse { Success = false, Message = "Chua dang nhap" };
            }
        }
        public async Task<ApiResponse> AddToCart(string idPro, string idProItem, int quantity, float price)
        {
            try
            {
                //Không chọn loại sp
                if (idProItem == null)
                {
                    return new ApiResponse { Success = false, Message = "Missing select productItem" };
                }
                //productItem không thuộc ve Product và status = 1(đã xóa)
                var productItem = _context.productItems.FirstOrDefault(p => p.IdProItem == idProItem && p.IdPro == idPro && p.StatusProItem == 0);
                if (productItem == null)
                {
                    return new ApiResponse { Success = false, Message = "ProductItem is not correct/exist" };
                }
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                if (user != null)
                {
                    string idCartNew = null;
                    var cart = _context.shoppingCarts
                            .Include(c => c.ShoppingCartItems)
                            .SingleOrDefault(c => c.IdAcc == user.Id);
                    if (cart == null)
                    {
                        var newCart = new ShoppingCart
                        {
                            IdCart = GenerateNextCartId(),
                            IdAcc = user.Id,
                            ShoppingCartItems = new List<ShoppingCartItem>()
                        };
                        _context.shoppingCarts.AddAsync(newCart);
                        await _context.SaveChangesAsync();
                        idCartNew = newCart.IdCart;
                        cart = newCart;
                    }
                    else
                    {
                        idCartNew = cart.IdCart;
                    }

                    // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
                    ShoppingCartItem existingItem = _context.shoppingCartItems.FirstOrDefault(item => item.IdProItem == idProItem && item.IdCart == idCartNew);
                    //ShoppingCartItem cartExit = cart.ShoppingCartItems.FirstOrDefault(item => item.IdProItem == idProItem && item.IdCart == idCartNew);
                    var proItem = _context.productItems.FirstOrDefault(p => p.IdProItem == idProItem);
                    if (existingItem != null)
                    {
                        var totalquan = existingItem.Quantity + quantity;
                        if (totalquan > proItem.Quantity)
                        {
                            return new ApiResponse { Success = false, Message = "Số lượng vượt quá số lượng sản phẩm" };
                        }

                        existingItem.Quantity += quantity;
                    }
                    else
                    {
                        if (quantity > proItem.Quantity)
                        {
                            return new ApiResponse { Success = false, Message = "Số lượng vượt quá số lượng sản phẩm" };
                        }
                        // Nếu chưa tồn tại, thêm mới vào giỏ hàng
                        ShoppingCartItem newItem = new ShoppingCartItem
                        {
                            IdCartItem = GenerateNextCartItemId(), // Tạo một id_cart_item duy nhất
                            IdCart = idCartNew,
                            IdProItem = idProItem,
                            Quantity = quantity,
                            Price = price,
                            IdPro = idPro,
                        };

                        cart.ShoppingCartItems.Add(newItem);
                    }
                    await _context.SaveChangesAsync();
                    return new ApiResponse { Success = true, Message = "Add to cart successfully" };
                }
                return new ApiResponse { Success = false, Message = "There are not login" };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, Message = ex.Message };
            }
        }
        public async Task<ApiResponse> UpdateQuantity(string idProItem, int quantity)
        {
            try
            {
                if (idProItem == null)
                {
                    return new ApiResponse { Success = false, Message = "idProItem null" };
                }
                var userid = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
                var cartitem = new ShoppingCartItem();
                var cart = await _context.shoppingCarts.FirstOrDefaultAsync(u => u.IdAcc == userid);
                if (cart != null)
                {
                    var proitem = _context.productItems.FirstOrDefault(e => e.IdProItem == idProItem);
                    cartitem = await _context.shoppingCartItems.FirstOrDefaultAsync(u => u.IdCart == cart.IdCart && u.IdProItem == idProItem);
                    if (cartitem != null)
                    {

                        if (quantity <= proitem.Quantity)
                        {

                            if (quantity <= 0 || quantity == null)
                            {
                                //Khi về 0 thì xóa sp ra khoi gio hang
                                _context.shoppingCartItems.Remove(cartitem);
                                await _context.SaveChangesAsync();
                                return new ApiResponse { Success = true, Message = "Xóa sp ra khoi gio hang vi so luong ve 0" };
                            }
                            else
                            {
                                cartitem.Quantity = quantity;
                            }
                            _context.shoppingCartItems.Update(cartitem);
                            await _context.SaveChangesAsync();
                            return new ApiResponse { Success = true, Message = "Update quantity successfully" };
                        }
                        return new ApiResponse { Success = false, Message = "Số lượng vượt quá số lượng sản phẩm" };
                    }
                    return new ApiResponse { Success = false, Message = "cartitem null" };
                }
                return new ApiResponse { Success = false, Message = "cart null" };
            }
            catch  (Exception ex)
            {
                return new ApiResponse { Success = false, Message = ex.Message };
            }
            
        }
        public async Task<ApiResponse> RemoveFromCart(string idCartItem)
        {
            try
            {

                var item = _context.shoppingCartItems.SingleOrDefault(p => p.IdCartItem == idCartItem);
                if (item != null)
                {
                    _context.shoppingCartItems.Remove(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return new ApiResponse { Success = false, Message = "Không tìm thấy mục trong giỏ hàng để xóa." };
                }


                return new ApiResponse { Success = true, Message = "Xóa thành công" };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, Message = ex.Message };
            }
        }
        public string GenerateNextCartId()
        {
            // Retrieve the maximum existing Id_pro
            string maxIdAcc = _context.shoppingCarts
                .Select(p => p.IdCart)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            // Generate the next Id_pro
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdAcc))
            {
                string numericPart = maxIdAcc.Substring(2); // Extract numeric part
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + 1;
                }
            }

            string nextIdAcc = $"CA{nextNumber:D3}"; // Format the new Id_acc

            return nextIdAcc;
        }
        public string GenerateNextCartItemId()
        {
            // Retrieve the maximum existing Id_pro
            string maxIdAcc = _context.shoppingCartItems
                .Select(p => p.IdCartItem)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            // Generate the next Id_pro
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdAcc))
            {
                string numericPart = maxIdAcc.Substring(2); // Extract numeric part
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + 1;
                }
            }

            string nextIdAcc = $"CI{nextNumber:D3}"; // Format the new Id_acc

            return nextIdAcc;
        }

     
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Migrations;
using WebShopAPI.Models;

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
			List<ShoppingCartItemDto> shopCartItems = new List<ShoppingCartItemDto>();

			string userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
			if (userId != null) // If user is logged in
			{
				var cart = await _context.shoppingCarts
					.Include(c => c.ShoppingCartItems)
					.ThenInclude(item => item.Product) // Include product details if needed
					.SingleOrDefaultAsync(c => c.IdAcc == userId);

				if (cart != null) // If cart exists
				{
					shopCartItems = cart.ShoppingCartItems.Select(item => new ShoppingCartItemDto
					{
						IdProItem = item.IdProItem,
						Quantity = item.Quantity,
						Price = item.Price,
						IdPro = item.IdPro,
						Product = item.Product
						// Map other properties as needed
					}).ToList();

					return new ApiResponse { Success = true, Data = shopCartItems };
				}

				return new ApiResponse { Success = false, Message = "Giỏ hàng rỗng" };
			}
			else // If user is not logged in
			{
				if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var existingCartData))
				{
					var cartDataString = Encoding.UTF8.GetString(existingCartData);
					var cart = JsonSerializer.Deserialize<ShoppingCart>(cartDataString, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
						ReferenceHandler = ReferenceHandler.Preserve
					});

					if (cart != null && cart.ShoppingCartItems != null)
					{
						shopCartItems = cart.ShoppingCartItems.Select(item => new ShoppingCartItemDto
						{
							IdCart = item.IdCart,
							IdCartItem = item.IdCartItem,
							IdProItem = item.IdProItem,
							Quantity = item.Quantity,
							Price = item.Price,
							IdPro = item.IdPro,
							Product = item.Product // Include product details if needed
						}).ToList();

						return new ApiResponse { Success = true, Data = shopCartItems };
					}
					else
					{
						return new ApiResponse { Success = false, Message = "Giỏ hàng rỗng trong session" };
					}
				}
				else
				{
					return new ApiResponse { Success = true, Message = "Khong co gio hang trong session" };
				}
			}
		}


		/*public async Task<ApiResponse> AddToCart(string idPro, string idProItem, int quantity, float price)
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
        }*/
		public async Task<ApiResponse> AddToCart(string idPro, string idProItem, int quantity, float price)
		{
			try
			{
				// Check if a product item is selected
				if (idProItem == null)
				{
					return new ApiResponse { Success = false, Message = "Missing select productItem" };
				}

				// Check if the product item exists and is active
				var productItem = await _context.productItems
					.FirstOrDefaultAsync(p => p.IdProItem == idProItem && p.IdPro == idPro && p.StatusProItem == 0);

				if (productItem == null)
				{
					return new ApiResponse { Success = false, Message = "ProductItem is not correct/exist" };
				}

				var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
				string idCartNew;
				ShoppingCart cart; // Declare cart here

				if (user != null) // If user is logged in
				{
					cart = await _context.shoppingCarts
						.Include(c => c.ShoppingCartItems)
						.SingleOrDefaultAsync(c => c.IdAcc == user.Id);

					if (cart == null) // If cart doesn't exist
					{
						cart = new ShoppingCart
						{
							IdCart = GenerateNextCartId(),
							IdAcc = user.Id,
							ShoppingCartItems = new List<ShoppingCartItem>()
						};
						await _context.shoppingCarts.AddAsync(cart);
						await _context.SaveChangesAsync();
						idCartNew = cart.IdCart;
					}
					else
					{
						idCartNew = cart.IdCart;
					}
				}
				else // If user is not logged in
				{
					// Check if there is a cart in session
					if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var existingCartData))
					{
						// Deserialize the cart from session
						cart = JsonSerializer.Deserialize<ShoppingCart>(existingCartData);

						// Check if the cart was successfully deserialized
						if (cart == null)
						{
							return new ApiResponse { Success = false, Message = "Failed to retrieve shopping cart from session." };
						}
						idCartNew = cart.IdCart;
					}
					else
					{
						// Create a new cart if none exists
						cart = new ShoppingCart
						{
							IdCart = GenerateNextCartId(),
							ShoppingCartItems = new List<ShoppingCartItem>()
						};
						// Serialize the new cart into session
						_httpContextAccessor.HttpContext.Session.SetString("ShoppingCart", JsonSerializer.Serialize(cart));
						idCartNew = cart.IdCart;
					}
				}

				// Check if the product item already exists in the cart
				ShoppingCartItem existingItem = cart.ShoppingCartItems
					.FirstOrDefault(item => item.IdProItem == idProItem);

				if (existingItem != null) // If product already exists in cart
				{
					var totalQuantity = existingItem.Quantity + quantity;
					if (totalQuantity > productItem.Quantity)
					{
						return new ApiResponse { Success = false, Message = "Quantity exceeds available stock." };
					}

					existingItem.Quantity += quantity; // Update quantity
				}
				else // If it doesn't exist, add a new item to the cart
				{
					if (quantity > productItem.Quantity)
					{
						return new ApiResponse { Success = false, Message = "Quantity exceeds available stock." };
					}

					// Create a new shopping cart item
					ShoppingCartItem newItem = new ShoppingCartItem
					{
						IdCartItem = GenerateNextCartItemId(), // Generate a unique item ID
						IdCart = idCartNew,
						IdProItem = idProItem,
						Quantity = quantity,
						Price = price,
						IdPro = idPro,
					};

					// Add the new item to the cart
					cart.ShoppingCartItems.Add(newItem);
				}
                

                if (user == null)
				{
					var shoppingCartDto = new ShoppingCartDto
					{
						IdCart = cart.IdCart,
						ShoppingCartItems = new List<ShoppingCartItemDto>()
					};

					foreach (var item in cart.ShoppingCartItems)
					{
						var product = await _context.products
							.Include(p => p.ProductItems) // Ensure you include ProductItems if necessary
							.Where(p => p.IdPro == item.IdPro)
							.FirstOrDefaultAsync();

						shoppingCartDto.ShoppingCartItems.Add(new ShoppingCartItemDto
						{
							IdCart = item.IdCart,
							IdCartItem = item.IdCartItem,
							IdProItem = item.IdProItem,
							Quantity = item.Quantity,
							Price = item.Price,
							IdPro = item.IdPro,
							Product = product
						});
					}

					// Serialize the updated cart back into session with ReferenceHandler.Preserve
					var options = new JsonSerializerOptions
					{
						ReferenceHandler = ReferenceHandler.Preserve,
						WriteIndented = true // Optional: for better readability
					};
					_httpContextAccessor.HttpContext.Session.SetString("ShoppingCart", JsonSerializer.Serialize(shoppingCartDto, options));
				}
				else
				{
					// Save changes to the database
					await _context.SaveChangesAsync();
				}

				return new ApiResponse { Success = true, Message = "Add to cart successfully" };
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
				if (string.IsNullOrEmpty(idProItem))
				{
					return new ApiResponse { Success = false, Message = "idProItem null" };
				}

				var userid = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
				ShoppingCartItem cartitem;

				// Nếu người dùng đã đăng nhập
				if (userid != null)
				{
					var cart = await _context.shoppingCarts.FirstOrDefaultAsync(u => u.IdAcc == userid);
					if (cart != null)
					{
						var proitem = await _context.productItems.FirstOrDefaultAsync(e => e.IdProItem == idProItem);
						cartitem = await _context.shoppingCartItems.FirstOrDefaultAsync(u => u.IdCart == cart.IdCart && u.IdProItem == idProItem);

						if (cartitem != null)
						{
							if (quantity <= proitem.Quantity)
							{
								if (quantity <= 0)
								{
									// Khi về 0 thì xóa sản phẩm ra khỏi giỏ hàng
									_context.shoppingCartItems.Remove(cartitem);
									await _context.SaveChangesAsync();
									return new ApiResponse { Success = true, Message = "Xóa sản phẩm khỏi giỏ hàng vì số lượng về 0" };
								}
								else
								{
									cartitem.Quantity = quantity;
								}

								_context.shoppingCartItems.Update(cartitem);
								await _context.SaveChangesAsync();
								return new ApiResponse { Success = true, Message = "Cập nhật số lượng thành công" };
							}
							return new ApiResponse { Success = false, Message = "Số lượng vượt quá số lượng sản phẩm" };
						}
						return new ApiResponse { Success = false, Message = "Giỏ hàng rỗng" };
					}
					return new ApiResponse { Success = false, Message = "Giỏ hàng không tồn tại" };
				}
				// Nếu người dùng chưa đăng nhập
				else
				{
					// Kiểm tra xem giỏ hàng có trong session không
					if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var existingCartData))
					{
						var options = new JsonSerializerOptions
						{
							ReferenceHandler = ReferenceHandler.Preserve,
							WriteIndented = true
						};

						var cart = JsonSerializer.Deserialize<ShoppingCartDto>(existingCartData, options);
						var cartitemDto = cart.ShoppingCartItems.FirstOrDefault(u => u.IdProItem == idProItem);

						if (cartitemDto != null)
						{
							var proitem = await _context.productItems.FirstOrDefaultAsync(e => e.IdProItem == idProItem);
							if (quantity <= proitem.Quantity)
							{
								if (quantity <= 0)
								{
									// Xóa sản phẩm ra khỏi giỏ hàng trong session
									cart.ShoppingCartItems.Remove(cartitemDto);
									// Cập nhật giỏ hàng vào session
									_httpContextAccessor.HttpContext.Session.SetString("ShoppingCart", JsonSerializer.Serialize(cart, options));
									return new ApiResponse { Success = true, Message = "Xóa sản phẩm khỏi giỏ hàng vì số lượng về 0" };
								}
								else
								{
									cartitemDto.Quantity = quantity;
								}

								// Cập nhật giỏ hàng vào session
								_httpContextAccessor.HttpContext.Session.SetString("ShoppingCart", JsonSerializer.Serialize(cart, options));
								return new ApiResponse { Success = true, Message = "Cập nhật số lượng thành công" };
							}
							return new ApiResponse { Success = false, Message = "Số lượng vượt quá số lượng sản phẩm" };
						}
						return new ApiResponse { Success = false, Message = "Sản phẩm không tồn tại trong giỏ hàng" };

					}
					return new ApiResponse { Success = false, Message = "Giỏ hàng không tồn tại trong session" };
				}
			}
			catch (Exception ex)
			{
				return new ApiResponse { Success = false, Message = ex.Message };
			}
		}

		public async Task<ApiResponse> RemoveFromCart(string idCartItem)
		{
			try
			{

				var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

				if (userId != null) // Người dùng đã đăng nhập
				{
					// Tìm item trong giỏ hàng trong cơ sở dữ liệu
					var item = await _context.shoppingCartItems.SingleOrDefaultAsync(p => p.IdCartItem == idCartItem);
					if (item != null)
					{
						_context.shoppingCartItems.Remove(item);
						await _context.SaveChangesAsync();
						return new ApiResponse { Success = true, Message = "Xóa thành công" };
					}
					else
					{
						return new ApiResponse { Success = false, Message = "Không tìm thấy mục trong giỏ hàng để xóa." };
					}
				}
				else // Người dùng chưa đăng nhập
				{
					// Kiểm tra xem giỏ hàng có trong session không
					if (_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var existingCartData))
					{
						var options = new JsonSerializerOptions
						{
							ReferenceHandler = ReferenceHandler.Preserve,
							WriteIndented = true
						};
						// Deserialize giỏ hàng từ session
						var cart = JsonSerializer.Deserialize<ShoppingCartDto>(existingCartData, options);
						var itemToRemove = cart.ShoppingCartItems.SingleOrDefault(p => p.IdCartItem == idCartItem);

						if (itemToRemove != null)
						{
							cart.ShoppingCartItems.Remove(itemToRemove); // Sử dụng cart.ShoppingCartItems để xóa
							_httpContextAccessor.HttpContext.Session.SetObjectAsJson("ShoppingCart", cart); // Cập nhật lại giỏ hàng trong session
							return new ApiResponse { Success = true, Message = "Xóa thành công từ giỏ hàng tạm." };
						}
						else
						{
							return new ApiResponse { Success = false, Message = "Không tìm thấy mục trong giỏ hàng tạm để xóa." };
						}
					}
					else
					{
						return new ApiResponse { Success = false, Message = "Không có giỏ hàng trong session" };
					}
				}
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
		public string GenerateNextOrderId()
		{
			// Retrieve the maximum existing Id_pro
			string maxIdOrder = _context.orders
				.Select(p => p.IdOrder)
				.OrderByDescending(id => id)
				.FirstOrDefault();

			// Generate the next Id_pro
			int nextNumber = 1;
			if (!string.IsNullOrEmpty(maxIdOrder))
			{
				string numericPart = maxIdOrder.Substring(2); // Extract numeric part
				if (int.TryParse(numericPart, out int numericValue))
				{
					nextNumber = numericValue + 1;
				}
			}

			string nextIdOrder = $"OR{nextNumber:D3}"; // Format the new Id_acc

			return nextIdOrder;
		}

		public string GenerateNextOrderDetailId()
		{
			// Retrieve the maximum existing Id_pro
			string maxIdOrderDetail = _context.orderDetails
				.Select(p => p.IdOrderDetail)
				.OrderByDescending(id => id)
				.FirstOrDefault();

			int nextNumber = 1;
			if (!string.IsNullOrEmpty(maxIdOrderDetail))
			{
				string numericPart = maxIdOrderDetail.Substring(2); 
				if (int.TryParse(numericPart, out int numericValue))
				{
					nextNumber = numericValue + 1;
				}
			}

			string nextIdOrderDetail = $"OD{nextNumber:D3}"; 

			return nextIdOrderDetail;
		}


        // public async Task<ApiResponse> CheckoutCOD(CheckoutModel model)
        // {
        //     if (model == null)
        //     {
        //         return new ApiResponse { Success = false, Message = "Invalid request data" };
        //     }

        //     var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

        //     if (model.paymentMethod == "Bank")
        //     {
        //         return new ApiResponse { Success = false, Message = "Method payment is bank" };
        //     }

        //     var cart = _context.shoppingCarts.Include(c => c.ShoppingCartItems)
        //                                      .FirstOrDefault(c => c.IdAcc == user.Id);
        //     if (cart == null)
        //     {
        //         return new ApiResponse { Success = false, Message = "Cart is empty" };
        //     }

        //     var shopCartItems = _context.shoppingCartItems.Where(item => item.IdCart == cart.IdCart)
        //                                                   .ToList();

        //     if (!shopCartItems.Any())
        //     {
        //         return new ApiResponse { Success = false, Message = "No items in cart" };
        //     }

        //     float orderTotal = (float)shopCartItems.Sum(item => item.Quantity * item.Price);
        //     Discount discount = null;

        //     // Handle discount code
        //     if (!string.IsNullOrEmpty(model.DiscountCode))
        //     {
        //         discount = _context.discounts.FirstOrDefault(d => d.Code == model.DiscountCode && d.Status == 0);
        //         if (discount == null)
        //         {
        //             return new ApiResponse { Success = false, Message = "Invalid discount code" };
        //         }

        //         if (discount.ExpiryDate.HasValue && discount.ExpiryDate.Value < DateTime.Now)
        //         {
        //             return new ApiResponse { Success = false, Message = "Discount code has expired" };
        //         }

        //         // Apply percentage or fixed discount
        //         if (discount.DiscountAmount <= 1) // Percentage-based discount
        //         {
        //             orderTotal -= orderTotal * discount.DiscountAmount;
        //         }
        //         else // Fixed amount discount
        //         {
        //             orderTotal -= discount.DiscountAmount;
        //         }

        //         // Ensure order total doesn't go negative
        //         orderTotal = Math.Max(orderTotal, 0);
        //     }

        //     using (var transaction = await _context.Database.BeginTransactionAsync())
        //     {
        //         try
        //         {
        //             var order = new Order
        //             {
        //                 IdOrder = GenerateNextOrderId(),
        //                 IdAcc = user.Id,
        //                 Fullname = model.FullName,
        //                 Email = model.Email,
        //                 Phone = model.Phone,
        //                 Address = model.Address,
        //                 Note = model.Note,
        //                 OrderDate = DateTime.Now,
        //                 PaymentMethod = model.paymentMethod,
        //                 ShippingMethod = model.shippingMethod,
        //                 OrderStatus = 0,
        //                 OrderDetails = new List<OrderDetail>()
        //             };

        //             foreach (var item in cart.ShoppingCartItems)
        //             {
        //                 var orderItem = new OrderDetail
        //                 {
        //                     IdOrderDetail = GenerateNextOrderDetailId(),
        //                     IdOrder = order.IdOrder,
        //                     IdProItem = item.IdProItem,
        //                     Quantity = item.Quantity,
        //                     Price = item.Price,
        //OrderTotal = orderTotal,
        //                     DiscountAmount = discount?.DiscountAmount ?? 0,
        //                 };
        //                 order.OrderDetails.Add(orderItem);
        //             }

        //             _context.orders.Add(order);
        //             _context.shoppingCarts.Remove(cart);
        //             _context.shoppingCartItems.RemoveRange(cart.ShoppingCartItems);

        //             await _context.SaveChangesAsync();
        //             await transaction.CommitAsync();

        //             return new ApiResponse { Success = true, Message = "Checkout successfully" };
        //         }
        //         catch (Exception ex)
        //         {
        //             await transaction.RollbackAsync();
        //             return new ApiResponse { Success = false, Message = $"Checkout failed: {ex.Message}" };
        //         }
        //     }
        // }

        public async Task<ApiResponse> CheckoutCOD(CheckoutModel model)
        {
            if (model == null)
            {
                return new ApiResponse { Success = false, Message = "Invalid request data" };
            }
			float shippingFee = 0;
			if(model.shippingMethod == "GHTK")
			{
                shippingFee = 35000;

            } else if (model.shippingMethod == "GHN")
            {
                shippingFee = 40000;
            }

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            ShoppingCart cart = null;


			if(user != null)
			{
                // Lấy giỏ hàng theo userId
                cart = _context.shoppingCarts.Include(c => c.ShoppingCartItems)
                                             .FirstOrDefault(c => c.IdAcc == user.Id);
                if (cart == null)
                {
                    return new ApiResponse { Success = false, Message = "Cart is empty" };
                }
            }
			else
			{
                // Người dùng không đăng nhập, lấy giỏ hàng từ Session
				_httpContextAccessor.HttpContext.Session.TryGetValue("ShoppingCart", out var existingCartData);
                var cartDataString = Encoding.UTF8.GetString(existingCartData);
                // Deserialize giỏ hàng từ session
                cart = JsonSerializer.Deserialize<ShoppingCart>(cartDataString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
                
                if (cart == null && cart.ShoppingCartItems == null)
                {
                    return new ApiResponse { Success = false, Message = "No items in cart" };
                }
            }


            if (model.paymentMethod == "Bank")
            {
                return new ApiResponse { Success = false, Message = "Method payment is bank" };
            }



            /*var shopCartItems = _context.shoppingCartItems.Where(item => item.IdCart == cart.IdCart)
                                                          .ToList();

            if (!shopCartItems.Any())
            {
                return new ApiResponse { Success = false, Message = "No items in cart" };
            }*/

            // Tính tổng tiền đơn hàng
            float orderTotal = (float)cart.ShoppingCartItems.Sum(item => item.Quantity * item.Price) - shippingFee;
            Discount discount = null;

            // Handle discount code
            if (!string.IsNullOrEmpty(model.DiscountCode))
            {
                discount = _context.discounts.FirstOrDefault(d => d.Code == model.DiscountCode && d.Status == 0);
                if (discount == null)
                {
                    return new ApiResponse { Success = false, Message = "Invalid discount code" };
                }

                if (discount.ExpiryDate.HasValue && discount.ExpiryDate.Value < DateTime.Now)
                {
                    return new ApiResponse { Success = false, Message = "Discount code has expired" };
                }

				if (orderTotal < discount.MinimumOrderAmount)
				{
                    return new ApiResponse { Success = false, Message = "Discount not applicable to this product" };
                }

                // Apply percentage or fixed discount
                if (discount.DiscountAmount <= 1) // Percentage-based discount
                {
                    orderTotal -= orderTotal * discount.DiscountAmount;
                }
                else // Fixed amount discount
                {
                    orderTotal -= discount.DiscountAmount;
                }

                // Ensure order total doesn't go negative
                orderTotal = Math.Max(orderTotal, 0);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = new Order
                    {
                        IdOrder = GenerateNextOrderId(),
                        IdAcc = user?.Id,  // Nếu người dùng không đăng nhập, để trống IdAcc
                        Fullname = model.FullName,
                        Email = model.Email,
                        Phone = model.Phone,
                        Address = model.Address,
                        Note = model.Note,
                        OrderDate = DateTime.Now,
                        PaymentMethod = model.paymentMethod,
                        ShippingMethod = model.shippingMethod,
						OrderInProgress = Utils.Constant.OrderStatus.Pending,
						PendingAt = DateTime.Now,
                        Status = 0,
                        OrderTotal = orderTotal,
                        OrderDetails = new List<OrderDetail>()
                    };

                    foreach (var item in cart.ShoppingCartItems)
                    {
                        var productItem = await _context.productItems.FirstOrDefaultAsync(p => p.IdProItem == item.IdProItem);

                        if (productItem == null)
                        {
                            return new ApiResponse { Success = false, Message = $"Product item {item.IdProItem} does not exist" };
                        }

                        if (productItem.Quantity < item.Quantity)
                        {
                            return new ApiResponse { Success = false, Message = $"Not enough stock for product {productItem.IdProItem}" };
                        }

                        // Deduct stock
                        productItem.Quantity -= item.Quantity;

                        var orderItem = new OrderDetail
                        {
                            IdOrderDetail = GenerateNextOrderDetailId(),
                            IdOrder = order.IdOrder,
                            IdProItem = item.IdProItem,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            DiscountAmount = discount?.DiscountAmount ?? 0,
                        };
                        order.OrderDetails.Add(orderItem);
                    }

                    _context.orders.Add(order);
                    if (user != null)
                    {
                        _context.shoppingCarts.Remove(cart);
                        _context.shoppingCartItems.RemoveRange(cart.ShoppingCartItems);
                    }
                    else
                    {
                        // Xóa giỏ hàng khỏi session
                        _httpContextAccessor.HttpContext.Session.Remove("ShoppingCart");
                    }

                    await _context.SaveChangesAsync();
                    // Lưu thông tin đơn hàng vào session
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, // Dùng ReferenceHandler.Preserve để hỗ trợ tuần tự hóa với vòng lặp
                        WriteIndented = true // Tùy chọn này để có định dạng JSON đẹp hơn, có thể bỏ qua nếu không cần
                    };

                    var orderJson = JsonSerializer.Serialize(order, options);
                    _httpContextAccessor.HttpContext.Session.SetString("Order", orderJson);

                    await transaction.CommitAsync();
                    return new ApiResponse { Success = true, Message = "Checkout successfully" };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponse { Success = false, Message = $"Checkout failed: {ex.Message}" };
                }
            }
        }

        public async Task<ApiResponse> GetCheckoutInfo()
        {
            var orderJson = _httpContextAccessor.HttpContext.Session.GetString("Order");
            if (string.IsNullOrEmpty(orderJson))
            {
                return new ApiResponse { Success = false, Message = "No order information found in session." };
            }

			// Deserialize chuỗi JSON thành đối tượng Order
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				ReferenceHandler = ReferenceHandler.Preserve // Giữ lại thông tin tham chiếu nếu có
			};
            var order = JsonSerializer.Deserialize<Order>(orderJson, options);
            // Tìm các chi tiết đơn hàng từ cơ sở dữ liệu (nếu cần)
            var orderDetails = await _context.orderDetails
                                              .Where(od => od.IdOrder == order.IdOrder)
                                              .ToListAsync();

            return new ApiResponse
            {
                Success = true,
                Data = new
                {
                    Order = order,
                    OrderDetails = orderDetails
                },
                Message = "Order information retrieved successfully."
            };
        }
    }
}

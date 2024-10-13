using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
	public class DiscountRepo : IDiscountRepo
	{
		private readonly AppDbContext _context;
		public DiscountRepo(AppDbContext context)
		{
			_context = context;
		}
		public async Task<ApiResponse> Create(DiscountModel model)
		{
			var duplicateCode = await _context.discounts.Where(d => d.Code == model.Code && d.Status == 0).FirstOrDefaultAsync();
			if (duplicateCode != null)
			{
				return new ApiResponse
				{
					Success = false,
					Message = "Code is already exist",
				};
			}
			if(model != null)
			{
				var discount = new Discount
				{
					IdDiscount = GenerateNextDiscountId(),
					Code = model.Code.IsNullOrEmpty()?randomVoucherCode() : model.Code,
					Description = model.Description,
					DiscountAmount = model.DiscountAmount,
					ExpiryDate = model.ExpiryDate,
					MinimumOrderAmount = model.MinimumOrderAmount,
					IsUsed = false,
					Status = 0
				};
				_context.discounts.Add(discount);
				await _context.SaveChangesAsync();

				return new ApiResponse
				{
					Success = true,
					Message = "Add discount successfully",
				};
			}
			return new ApiResponse
			{
				Success = false,
				Message = "Data is invalid",
			};
		}

		public async Task<ApiResponse> DeleteById(string id)
		{
			var discountToDelete = await GetById(id);
			if (discountToDelete != null)
			{
				discountToDelete.Status = 1;
				_context.discounts.Update(discountToDelete);
				await _context.SaveChangesAsync();
				return new ApiResponse
				{
					Success = true,
					Message = "Delete discount successfully",
				};
			}
			return new ApiResponse
			{
				Success = false,
				Message = "Discount not found",
			};
		}

		public async Task<List<Discount>> GetAll()
		{
			var listDiscount = await _context.discounts.Where(d => d.Status == 0).ToListAsync();
			return listDiscount;
		}

		public async Task<Discount> GetById(string id)
		{
			var discount = await _context.discounts.Where(d => d.Status == 0 && d.IdDiscount == id).FirstOrDefaultAsync();
			return discount;
		}

		public async Task<ApiResponse> Update(string id, DiscountModel discount)
		{
			var discountToUpdate = await GetById(id);
			if(discountToUpdate != null)
			{
				discountToUpdate.Code = discount.Code;
				discountToUpdate.Description = discount.Description;
				discountToUpdate.DiscountAmount = discount.DiscountAmount;
				discountToUpdate.ExpiryDate = discount.ExpiryDate;
				discountToUpdate.MinimumOrderAmount = discount.MinimumOrderAmount;

				_context.discounts.Update(discountToUpdate);
				await _context.SaveChangesAsync();
				return new ApiResponse
				{
					Success = true,
					Message = "Update discount successfully",
				};
			}
			return new ApiResponse
			{
				Success = false,
				Message = "Discount not found",
			};
		}
		public string GenerateNextDiscountId()
		{
			// Retrieve the maximum existing Id_pro
			string maxIdDiscount = _context.discounts
				.Select(p => p.IdDiscount)
				.OrderByDescending(id => id)
				.FirstOrDefault();

			// Generate the next Id_pro
			int nextNumber = 1;
			if (!string.IsNullOrEmpty(maxIdDiscount))
			{
				string numericPart = maxIdDiscount.Substring(2); // Extract numeric part
				if (int.TryParse(numericPart, out int numericValue))
				{
					nextNumber = numericValue + 1;
				}
			}

			string nextIdCate = $"DI{nextNumber:D3}"; // Format the new Id_pro

			return nextIdCate;
		}
		private string randomVoucherCode()
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var stringChars = new char[10];
			var random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			var finalString = new String(stringChars);
			return finalString;
		}
	}
}

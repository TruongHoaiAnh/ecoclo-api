using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;
        public CategoryRepo(AppDbContext context) {
            _context = context;
        }
        public async Task<ApiResponse> Create(CategoryModel model)
        {
            if (model == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "INVALID_DATA",
                    Message = "Category model cannot be null."
                };
            }

            var cate = new Category();
            cate.IdCate = GenerateNextCateId(); ;
            cate.NameCate = model.NameCate;
            cate.StatusCate = 0;
            _context.categories.Add(cate);
            await _context.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Create category successfully",
                Data = cate
            };

        }

        public async Task DeleteById(string id)
        {
            var cate = _context.categories.FirstOrDefault(c => c.IdCate == id);
            //xóa product có category đã xóa 
            foreach(var p in cate.Products)
            {
                p.StatusProduct = 1;
            }
            cate.StatusCate = 1;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAll()
        {
            var lsCate = await _context.categories.Where(x => x.StatusCate == 0).ToListAsync();
            return lsCate;
        }

        public async Task<Category> GetById(string id)
        {
            var cate = await _context.categories
               .Where(x => x.StatusCate == 0 && x.IdCate == id) // Add the condition to filter by Id
               .FirstOrDefaultAsync();
            return cate;
        }

        public async Task<ApiResponse> Update(CategoryModel model, string id)
        {
            if (model == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "INVALID_DATA",
                    Message = "Category model cannot be null."
                };
            }
            var cate = _context.categories.FirstOrDefault(c => c.IdCate == id);
            cate.NameCate = model.NameCate;
            _context.categories.Update(cate);
            await _context.SaveChangesAsync();
            return new ApiResponse
            {
                Success = true,
                Message = "Update successfully",
                Data = cate.IdCate
            };
        }

        public string GenerateNextCateId()
        {
            // Retrieve the maximum existing Id_pro
            string maxIdCate = _context.categories
                .Select(p => p.IdCate)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            // Generate the next Id_pro
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdCate))
            {
                string numericPart = maxIdCate.Substring(2); // Extract numeric part
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + 1;
                }
            }

            string nextIdCate = $"CA{nextNumber:D3}"; // Format the new Id_pro

            return nextIdCate;
        }

    }
}

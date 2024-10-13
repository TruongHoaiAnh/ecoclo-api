using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public class BannerRepo : IBannerRepo
    {
        private readonly AppDbContext _context;
        public BannerRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> AddBanner(BannerModel banners)
        {
            try
            {
                var allowedExtensions = new[] { ".gif", ".jpeg", ".jpg", ".tiff", ".png", ".webp", ".bmp" };

                foreach (var img in banners.ImgFiles)
                {
                    var extension = Path.GetExtension(img.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        return new ApiResponse
                        {
                            Success = false,
                            ErrorCode = "INVALID_IMAGE_FORMAT",
                            Message = $"Invalid image file format for file: {img.FileName}. Allowed formats are: {string.Join(", ", allowedExtensions)}"
                        };
                    }

                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(img.FileName);
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Img", "Banner");
                    var filePath = Path.Combine(folderPath, fileName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    var banner = new Banner
                    {
                        IdBanner = GenerateNextBanner(),
                        LinkImg = fileName,
                    };
                    await _context.banners.AddAsync(banner);

                    await _context.SaveChangesAsync();
                }
                return new ApiResponse
                {
                    Success = true,
                    Message = "Add banner successfully",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "ADD_BANNER_ERROR",
                    Message = ex.Message
                };
            }
        }

        public async Task<List<Banner>> GetAllBanner()
        {
            var banner = await _context.banners.ToListAsync();
            return banner;
        }

        public async Task<ApiResponse> Remove(string idBanner)
        {
            var banner = _context.banners.FirstOrDefault(c => c.IdBanner == idBanner);
            if (banner == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "BANNER_NOT_FOUND",
                    Message = "Banner not found"
                };
            }
            _context.banners.Remove(banner);
            await _context.SaveChangesAsync();
            return new ApiResponse
            {
                Success = true,
                Message = "Remove banner successfully"
            };
        }

        public string GenerateNextBanner()
        {
            // Retrieve the maximum existing Id_pro
            string maxIdBanner = _context.banners
                .Select(p => p.IdBanner)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdBanner))
            {
                string numericPart = maxIdBanner.Substring(2);
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + 1;
                }
            }

            string nextIdImg = $"BA{nextNumber:D3}";

            return nextIdImg;
        }
    }
}

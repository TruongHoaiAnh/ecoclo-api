using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly AppDbContext _context;

        public ReviewRepo(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
        }
        public async Task<ApiResponse> DeleteReview(string idReview)
        {
            var findReivew = await _context.reviews.FindAsync(idReview);
            if (findReivew == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "RIVIEW_NOTFOUND",
                };
            }
            findReivew.statusReview = 1; //Xóa mềm
            await _context.SaveChangesAsync();
            return new ApiResponse
            {
                Success = true,
                Message = "Delete review successful"
            };
        }

        public async Task<ApiResponse> InsertReview(ReviewModel model, string idAcc, string idPro)
        {
            if (string.IsNullOrWhiteSpace(model.Comment))
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "MISSING_COMMENT",
                };
            }
            if (model.RatingValue == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "MISSING_RATING",
                };
            }
            try
            {
                var review = new Review
                {
                    IdReview = GenerateNextReviewId(),
                    IdAcc = idAcc,
                    IdPro = idPro,
                    RatingValue = model.RatingValue,
                    Comment = model.Comment,
                    ReviewDate = DateTime.Now,
                    statusReview = 0
                };
                await _context.reviews.AddAsync(review);
                await _context.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = $"Insert comment successful"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "INTERNAL_ERROR",
                    Message = $"An error occurred while comment: {ex.Message}"
                };

            }
        }

        public async Task<List<Review>> GetAllReview()
        {
            var lsReview = await _context.reviews.Where(x => x.statusReview == 0).OrderByDescending(x => x.Like).ToListAsync();
            return lsReview;
        }

        public string GenerateNextReviewId()
        {
            // Retrieve the maximum existing Id_pro
            string maxIdReview = _context.reviews
                .Select(p => p.IdReview)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            // Generate the next Id_pro
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdReview))
            {
                string numericPart = maxIdReview.Substring(2); // Extract numeric part
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + 1;
                }
            }

            string nextIdPro = $"RV{nextNumber:D3}"; // Format the new Id_pro

            return nextIdPro;
        }
    }
}

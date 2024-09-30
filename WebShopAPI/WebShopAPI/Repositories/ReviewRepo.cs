using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly IMapper _mapper;

        public ReviewRepo(AppDbContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _usermanager = userManager;
            _mapper = mapper;
        }
        public async Task<ApiResponse> DeleteReview(string idReview)
        {
            var findReivew = await _context.reviews.Where(x => x.statusReview == 0).FirstOrDefaultAsync(x => x.IdReview == idReview);
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

        public async Task<List<ReviewDto>> GetAllReview()
        {
            var quantity = _context.reviews.Where(x => x.statusReview == 0).Count();
            var lsReview = await _context.reviews
               .Where(x => x.statusReview == 0)
               .Include(r => r.user) 
               .OrderByDescending(x => x.Like)
               .ToListAsync();
            var lsReviewDto = lsReview.Select(r =>
            {
                var reviewDto = new ReviewDto
                {
                    Username = r.user.FullName, 
                    QuantityAll = quantity 
                };

                // Sử dụng mapper để ánh xạ các thuộc tính còn lại
                _mapper.Map(r, reviewDto);

                return reviewDto;
            }).ToList();

            return lsReviewDto;
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

        //update like or dislike
        public async Task<ApiResponse> UpdateReview(bool isLike, string idReview)
        {
            var review = await _context.reviews.Where(x => x.statusReview == 0).FirstOrDefaultAsync(x => x.IdReview == idReview);
            if (review == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "RIVIEW_NOTFOUND",
                };
            }
            if (isLike)
            {
                if(!review.Like.HasValue || review.Like == null)
                {
                    review.Like = 1;
                }
                else
                {
                    review.Like += 1;
                }
            }
            else
            {
                if (!review.Dislike.HasValue || review.Dislike == null)
                {
                    review.Dislike = 1;
                }
                else
                {
                    review.Dislike += 1;
                }
            }

            await _context.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Review updated successfully."
            };
        }
    }
}

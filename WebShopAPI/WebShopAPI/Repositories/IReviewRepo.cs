using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface IReviewRepo
    {
        public Task<ApiResponse> InsertReview(ReviewModel model, string idAcc, string idPro);
        public Task<List<ReviewDto>> GetAllReview();
        public Task<ApiResponse> DeleteReview(string idReview);
        public Task<ApiResponse> UpdateReview(bool isLike, string idReview);
    }
}

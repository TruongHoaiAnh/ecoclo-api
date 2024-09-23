using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public interface IReviewRepo
    {
        public Task<ApiResponse> InsertReview(ReviewModel model, string idAcc, string idPro);
        public Task<List<Review>> GetAllReview();
        public Task<ApiResponse> DeleteReview(string idReview);
    }
}

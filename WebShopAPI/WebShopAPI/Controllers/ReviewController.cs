using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PagedList;
using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Models;
using WebShopAPI.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IReviewRepo _reviewRepo;
        public ReviewController(UserManager<AppUser> userManager, IReviewRepo reviewRepo)
        {
            _userManager = userManager;
            _reviewRepo = reviewRepo;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(int? page)
        {
            List<ReviewDto> lsReview = new List<ReviewDto>();
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;

            try
            {
                lsReview = await _reviewRepo.GetAllReview();
                PagedList<ReviewDto> models = new PagedList<ReviewDto>(lsReview.AsQueryable(), pageNumber, pageSize);
                return Ok(models);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("{idPro}")]
        [Authorize(Roles = AppRole.User)]
        public async Task<IActionResult> Create([FromBody] ReviewModel model, string idPro)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _reviewRepo.InsertReview(model, userId, idPro);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{idReview}")]
        [Authorize(Roles = AppRole.Administrator)]
        public async Task<IActionResult> Delete(string idReview)
        {
            var result = await _reviewRepo.DeleteReview(idReview);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("{idReview}")]
        [Authorize(Roles = AppRole.User)]
        public async Task<IActionResult> UpdateLike(bool isLike, string idReview)
        {
            var result = await _reviewRepo.UpdateReview(isLike, idReview);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}

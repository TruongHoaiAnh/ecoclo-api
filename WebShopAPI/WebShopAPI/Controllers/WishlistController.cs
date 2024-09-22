using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWishlistRepo _wishlistRepo;


        public WishlistController(UserManager<AppUser> userManager, IWishlistRepo wishlistRepo)
        {
            _userManager = userManager;
            _wishlistRepo = wishlistRepo;
        }
        [HttpGet]
        [Authorize(Roles = AppRole.User)]
        public async Task<IActionResult> Get()
        {
            var userId = _userManager.GetUserId(User);
            var result = await _wishlistRepo.GetAllWishlist(userId);
            if (result.Count == 0)
            {
                return NotFound(new ApiResponse { Success = true, Message = "There is no favorite product in wishlist"});
            }
            return Ok(result);
        }


        [HttpPost]
        [Authorize(Roles = AppRole.User)]
        public async Task<IActionResult> Create([FromBody] string IdPro)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new ApiResponse { Success = false, Message = "User is not authenticated" });
            }
            var result = await _wishlistRepo.CreateWishlist(userIdClaim, IdPro);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpDelete("{idPro}")]
        [Authorize(Roles = AppRole.User)]
        public async Task<IActionResult> Delete(string idPro)
        {
            var result = await _wishlistRepo.DeleteWishlist(idPro);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

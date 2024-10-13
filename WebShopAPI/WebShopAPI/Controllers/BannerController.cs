using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopAPI.Data;
using WebShopAPI.Models;
using WebShopAPI.Repositories;

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerRepo _bannerRepo;
        public BannerController(IBannerRepo bannerRepo)
        {
            _bannerRepo = bannerRepo;
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllBanner()
        {
            var result = await _bannerRepo.GetAllBanner();
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost("Upload")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdaloadBanner([FromForm]BannerModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _bannerRepo.AddBanner(model);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Delete")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteBanner(string idBanner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _bannerRepo.Remove(idBanner);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

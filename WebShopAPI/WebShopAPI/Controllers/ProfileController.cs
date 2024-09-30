using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShopAPI.Data;
using WebShopAPI.Models.Profiles;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // GET: api/<ProfileController>
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound();
            }
            var result = new IndexProfile
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                profile = new EditExtraProfileModel()
                {
                    BirthDate = user.DateOfBirth,
                    HomeAdress = user.Address,
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Avt = user.Avt,
                    Gender = user.Gender,
                    FullName = user.FullName
                }
            };
            return Ok(result);
        }


        private Task<AppUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound(new {Message = "Không tìm thấy user"});
            }
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(new { Message = "Change password successful"});
        }

        [HttpPost("UploadAvatar")]
        public async Task<IActionResult> UploadImgAsync([FromForm] UploadFile file)
        {
            if (!Validate(file.FileUp))
            {
                ModelState.AddModelError("FileUp", "Vui lòng chọn file có định dạng .jpg, .jpeg, .png");
                return BadRequest(ModelState);
            }

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound(new { Message = "Không tìm thấy user" });
            }

            if (ModelState.IsValid)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(file.FileUp.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Img", "Avt");
                var filePath = Path.Combine(folderPath, fileName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.FileUp.CopyToAsync(fileStream);
                }

                user.Avt = fileName;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                await _signInManager.RefreshSignInAsync(user);
                return Ok(new { Message = "Update Avatar Successfully!" });
            }

            return BadRequest(ModelState);
        }

        private bool Validate(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

    }
}

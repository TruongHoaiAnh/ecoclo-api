using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopAPI.Helpers;
using WebShopAPI.Models;
using WebShopAPI.Repositories;

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepo _accountRepo;
        public AccountController(IAccountRepo accountRepo) 
        {
            _accountRepo = accountRepo;
        }

        [HttpPost("SignUp")]

        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await _accountRepo.SignUpAsync(model);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            // Trả về mã lỗi và thông điệp dựa trên mã lỗi từ IdentityResult
            var error = result.Errors.FirstOrDefault();
            if (error != null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorCode = error.Code,
                    Message = error.Description
                });
            }

            // Trường hợp khác
            return BadRequest(new ApiResponse { Success = false, ErrorCode = "SIGNUP_FAILED", Message = "Đăng ký không thành công." });

        }

        [HttpPost("SignIn")]

        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var result  = await _accountRepo.SignInAsync(model);
            if (result.Success)
            {
                return Ok(result);
            }

            return Ok(result);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _accountRepo.ForgotPasswordAsync(model);
            if (result)
            {
                return Ok(new { Message = "Yêu cầu đặt lại mật khẩu đã được gửi, vui lòng kiểm tra email của bạn." });
            }

            return BadRequest(new { Error = "Yêu cầu đặt lại mật khẩu không thành công." });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _accountRepo.ResetPasswordAsync(model);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}

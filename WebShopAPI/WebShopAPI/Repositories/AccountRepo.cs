using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using WebShopAPI.Controllers;
using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

namespace WebShopAPI.Repositories
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IEmailSender _emailSender;
        private readonly IUrlHelper _urlHelper;



        public AccountRepo(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor,
            IEmailSender emailSender,
            IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
        /*
                public async Task<ApiResponse> LogOffAsync()
                {
                    var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    if (token != null)
                    {
                        using (var scope = _httpContextAccessor.HttpContext.RequestServices.CreateScope())
                        {
                            var tokenBlacklist = scope.ServiceProvider.GetRequiredService<ITokenBlacklist>();
                            await tokenBlacklist.InvalidateTokenAsync(token);
                        }
                    }
                    await signInManager.SignOutAsync();
                    return new ApiResponse
                    {
                        Success = true,
                        Message = "Logoff successful"
                    };
                }*/

        public async Task<ApiResponse> SignInAsync(SignInModel model)
        {

            //Tìm Tài khoản theo Username
            var Results = await signInManager.PasswordSignInAsync(model.UserNameOrEmail, model.Password, model.RememberMe, lockoutOnFailure: true);
            var users = await userManager.FindByEmailAsync(model.UserNameOrEmail);
            //Tìm user by name check block
            if (users != null && users.Status == 1)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "ACCOUNT_LOCKED",
                    Message = "Tài khoản đã bị khóa."
                };

            }
            // Tìm UserName theo Email, đăng nhập lại
            if ((!Results.Succeeded) && AppUntiilities.IsValidEmail(model.UserNameOrEmail))
            {
                var user = await userManager.FindByEmailAsync(model.UserNameOrEmail);
                if (user != null)
                {
                    Results = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);
                }
            }

            if (Results.Succeeded)
            {
                //add role vào claim
                //tìm user by user name
                var userByName = await userManager.FindByNameAsync(model.UserNameOrEmail);
                if (userByName != null && userByName.Status == 1)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        ErrorCode = "ACCOUNT_LOCKED",
                        Message = "Tài khoản đã bị khóa."
                    };

                }
                if (userByName == null)
                {
                    //tìm user by email
                    userByName = await userManager.FindByEmailAsync(model.UserNameOrEmail);
                }
                var userRoles = await userManager.GetRolesAsync(userByName);
                var authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userByName.Email),
                    new Claim(ClaimTypes.NameIdentifier, userByName.Id), // Add user ID here
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach (var role in userRoles)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                        issuer: configuration["JWT:ValidIssuer"],
                        audience: configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddMinutes(20),
                        claims: authClaim,
                        signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                    );
                return new ApiResponse
                {
                    Success = true,
                    Data = new { Token = new JwtSecurityTokenHandler().WriteToken(token) }
                };
            }
            return new ApiResponse
            {
                Success = false,
                ErrorCode = "INVALID_SIGNIN",
                Message = "Tên người dùng hoặc mật khẩu không chính xác."
            };


        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new AppUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.Phone,
                Avt = "default-avatar.png",
                Status = 0,
            };

            // Kiểm tra xem tên người dùng hoặc email đã tồn tại chưa
            if (await userManager.FindByNameAsync(model.UserName) != null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "USERNAME_EXIT", Description = "Tên người dùng đã tồn tại." });
            }
            if (await userManager.FindByEmailAsync(model.Email) != null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "EMAIL_EXIT", Description = "Email đã tồn tại." });
            }
            if (await userManager.Users.AnyAsync(u => u.PhoneNumber == model.Phone))
            {
                return IdentityResult.Failed(new IdentityError { Code = "PHONE_EXIT", Description = "Số điện thoại đã tồn tại." });
            }



            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(AppRole.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(AppRole.User));
                }
                await userManager.AddToRoleAsync(user, AppRole.User);
            }
            return result;

        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null /*|| !(await userManager.IsEmailConfirmedAsync(user))*/)
                {
                    return false;
                }

                // Tạo mã đặt lại mật khẩu (reset token)
                var code = await userManager.GeneratePasswordResetTokenAsync(user); 
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // Tạo URL đặt lại mật khẩu (được gửi qua email)
                /*var callbackUrl = _urlHelper.Action(
                    action: "ResetPassword",
                    controller: "Account",
                    values: new { userId = user.Id, code },
                    protocol: "https");*/


                var param = new Dictionary<string, string?>
                {
                    {"token", code },
                    {"email", model.Email}
                };
                var callbackUrl = QueryHelpers.AddQueryString(model.ClientUrl, param);

                // Gửi email xác thực
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Đặt lại mật khẩu",
                    $"Xin chào {user.UserName},<br><br>" +
                    $"Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản của mình. Vui lòng <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>nhấp vào đây</a> để đặt lại mật khẩu.<br><br>" +
                    $"Nếu bạn không yêu cầu hành động này, vui lòng bỏ qua email này."
                );

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "RESET_PASSWORD_FAILED",
                    Message = "Đặt lại mật khẩu không thành công."
                };
            }
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await userManager.ResetPasswordAsync(user, code, model.Password);

            if (result.Succeeded)
            {
                return new ApiResponse
                {
                    Success = true,
                    Message = "Đặt lại mật khẩu thành công."
                };
            }
            else
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "RESET_PASSWORD_FAILED",
                    Message = "Đặt lại mật khẩu không thành công."
                };
            }
        }
    }
}

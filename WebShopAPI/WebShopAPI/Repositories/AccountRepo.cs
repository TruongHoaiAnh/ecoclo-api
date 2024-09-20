using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public AccountRepo(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
        }

        public async Task<ApiResponse> SignInAsync(SignInModel model)
        {
            
            //Tìm Tài khoản theo Username
            var Results = await signInManager.PasswordSignInAsync(model.UserNameOrEmail, model.Password, model.RememberMe, lockoutOnFailure: false);
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

       
    }
}

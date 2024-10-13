using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebShopAPI.Data;
using WebShopAPI.MailUtils;
using WebShopAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; // Optional: Makes the JSON output more readable
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
 {
     option.SwaggerDoc("v1", new OpenApiInfo { Title = "EcoShop API", Version = "v1" });
     option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
     {
         In = ParameterLocation.Header,
         Description = "Please enter a valid token",
         Name = "Authorization",
         Type = SecuritySchemeType.Http,
         BearerFormat = "JWT",
         Scheme = "Bearer"
     });
     option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

 });
//Auto Mapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Cho phép các nguồn tài nguyên khác truy cập vào api
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
// Thêm dịch vụ Session
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Thay đổi thời gian tùy ý
	options.Cookie.HttpOnly = true; // Bảo mật cookie
	options.Cookie.IsEssential = true; // Cần thiết cho ứng dụng
});
// Cấu hình In-Memory Cache
builder.Services.AddDistributedMemoryCache();
// Đăng ký các dịch vụ của Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//JWT
builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft
                .IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                    (builder.Configuration["JWT:Secret"]))
     };
       
    });



// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions>(options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 6; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

        // Cấu hình Lockout - khóa user
        /*options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
        options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
        options.Lockout.AllowedForNewUsers = true;*/

        // Cấu hình về User.
        options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true; // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = false; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại

});
//đăng ký dịch vụ SendMailService và hệ thống với tên giao diện IEmailSender
builder.Services.AddOptions();                                        // Kích hoạt Options
var mailsettings = builder.Configuration.GetSection("MailSettings");  // đọc config
builder.Services.Configure<MailSettings>(mailsettings);               // đăng ký để Inject
builder.Services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2));
//DI
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IWishlistRepo, WishlistRepo>();
builder.Services.AddScoped<IReviewRepo, ReviewRepo>();
builder.Services.AddScoped<IShoppingCartRepo, WebShopAPI.Repositories.ShoppingCartRepo>();
builder.Services.AddScoped<IBannerRepo, BannerRepo>();
builder.Services.AddScoped<IDiscountRepo, DiscountRepo>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

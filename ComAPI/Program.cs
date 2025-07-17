using System.Security.Claims;
using System.Text;
using COM.BUS;
using COM.DAL;
using COM.DAL.SanPham;
using COM.Services;
using COM.Services.Donhang;
using COM.Services.Vnpay;
using ComAPI.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);



// Set appConnectionStrings before building the app
COM.ULT.SQLHelper.appConnectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
//cấu hình sercet trong db
var secretDal = new ProviderSettingDAL(builder.Configuration.GetConnectionString("DefaultConnection"));
var facebookSecret = secretDal.GetByProvider("Facebook");
var googleSecret = secretDal.GetByProvider("Google");



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var services = builder.Services;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DonHangServices>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<DonHangDAL>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

// JWT Authentication Configuration
var jwtKey = Encoding.UTF8.GetBytes("YourSuperSecretKey");
builder.Services.AddAuthentication(options =>
{
    // Dùng JWT để bảo vệ API
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    // Dùng Cookie để hỗ trợ tạm lưu login với Google
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

})
.AddCookie()
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RoleClaimType = ClaimTypes.Role, // hoặc "role" nếu bạn đặt claim là "role"

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
})
.AddFacebook(options =>
{
    // Gán AppId của ứng dụng Facebook (lấy từ file cấu hình appsettings.json)
    //options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppId = facebookSecret.ClientId;


    // Gán AppSecret (khóa bí mật) của ứng dụng Facebook từ file cấu hình
    //options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];

    options.AppSecret = facebookSecret.ClientSecret;
    // Đường dẫn nội bộ trong backend để Facebook redirect user về sau khi xác thực
    options.CallbackPath = "/signin-facebook";

    // Thêm quyền (scope) yêu cầu người dùng đồng ý khi login
    // "public_profile" cho phép lấy thông tin cơ bản của người dùng (name, avatar, id, ...)

    options.Scope.Add("public_profile"); //Scope là những gì xin phép lấy từ user khi dăng nhập Facebook

    // Đăng ký yêu cầu lấy trường "email" từ Facebook user
    options.Fields.Add("email");

    // Đăng ký yêu cầu lấy trường "name" từ Facebook user
    options.Fields.Add("name");

    // Sau khi đăng nhập thành công, lưu lại access_token vào cookie/session
    // Giúp lần sau gọi API Facebook không cần đăng nhập lại
    options.SaveTokens = true;

})
.AddGoogle(options =>
{
    //options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    //options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.ClientId = googleSecret.ClientId;
    options.ClientSecret = googleSecret.ClientSecret;
    //options.CallbackPath = "/api/Google/google-callback"; // Đúng với Redirect URI
    options.CallbackPath = "/login-google";
    options.SaveTokens = true; // Lưu token sau login
    options.Scope.Add("profile");
    options.Scope.Add("email");
});

// Cấu hình phân quyền theo Role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    //options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

//})
//.AddCookie()
//.AddFacebook(options =>
//{
//    // Gán AppId của ứng dụng Facebook (lấy từ file cấu hình appsettings.json)
//    //options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
//    options.AppId = facebookSecret.ClientId;


//    // Gán AppSecret (khóa bí mật) của ứng dụng Facebook từ file cấu hình
//    //options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];

//    options.AppSecret = facebookSecret.ClientSecret;
//    // Đường dẫn nội bộ trong backend để Facebook redirect user về sau khi xác thực
//    options.CallbackPath = "/signin-facebook";

//    // Thêm quyền (scope) yêu cầu người dùng đồng ý khi login
//    // "public_profile" cho phép lấy thông tin cơ bản của người dùng (name, avatar, id, ...)

//    options.Scope.Add("public_profile"); //Scope là những gì xin phép lấy từ user khi dăng nhập Facebook

//    // Đăng ký yêu cầu lấy trường "email" từ Facebook user
//    options.Fields.Add("email");

//    // Đăng ký yêu cầu lấy trường "name" từ Facebook user
//    options.Fields.Add("name");

//    // Sau khi đăng nhập thành công, lưu lại access_token vào cookie/session
//    // Giúp lần sau gọi API Facebook không cần đăng nhập lại
//    options.SaveTokens = true;

//})
//.AddGoogle(options =>
// {
//     //options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//     //options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//     options.ClientId = googleSecret.ClientId;
//     options.ClientSecret = googleSecret.ClientSecret;
//     //options.CallbackPath = "/api/Google/google-callback"; // Đúng với Redirect URI
//     options.CallbackPath = "/login-google";
//     options.SaveTokens = true; // Lưu token sau login
//     options.Scope.Add("profile");
//     options.Scope.Add("email");
// });

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Configure JWT authentication for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddCors(options =>
{
    //file:///C:/Users/Admin/Desktop/chathub.html
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5500/")
                   .AllowCredentials()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .WithExposedHeaders("Set-Cookie")
                   .SetIsOriginAllowedToAllowWildcardSubdomains()
                   .SetIsOriginAllowed(_ => true);
        });

    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:2222") // frontend chạy ở đây
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
//builder.Services.AddScoped<AuthService>();
var app = builder.Build();
app.MapGet("/", () => "Welcome to COMTH_API!");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.MapHub<ChatHub>("/chatHub");


app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins); // Đừng gọi Cors Middleware!
app.UseAuthentication(); // cần trước UseAuthorization

app.UseAuthorization();

app.MapControllers();
app.Run();

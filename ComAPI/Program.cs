using COM.BUS;
using COM.DAL;
using COM.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;



var builder = WebApplication.CreateBuilder(args);
//cấu hình sercet trong db
var secretDal = new ProviderSettingDAL(builder.Configuration.GetConnectionString("DefaultConnection"));
var facebookSecret = secretDal.GetByProvider("Facebook");
var googleSecret = secretDal.GetByProvider("Google");

// Set appConnectionStrings before building the app
COM.ULT.SQLHelper.appConnectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var services = builder.Services;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddScoped<AuthService>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

})
.AddCookie()
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
     options.CallbackPath = "/signin-google"; // Đúng với Redirect URI
     options.SaveTokens = true; // Lưu token sau login
     options.Scope.Add("profile");
     options.Scope.Add("email");
 });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:2222")
                   .AllowCredentials()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .WithExposedHeaders("Set-Cookie")
                   .SetIsOriginAllowedToAllowWildcardSubdomains()
                   .SetIsOriginAllowed(_ => true);
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


app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins); // Đừng gọi Cors Middleware!
app.UseAuthorization();

app.MapControllers();
app.Run();

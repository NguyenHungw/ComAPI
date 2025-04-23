using COM.BUS;
using COM.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;


var builder = WebApplication.CreateBuilder(args);

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
    options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
})
.AddCookie()
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    options.CallbackPath = "/signin-facebook";
    options.Scope.Add("public_profile");
    options.Fields.Add("email");
    options.Fields.Add("name");
    options.SaveTokens = true;
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
app.UseCors(MyAllowSpecificOrigins); // Đừng quên gọi Cors Middleware!
app.UseAuthorization();

app.MapControllers();
app.Run();

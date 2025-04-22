using COM.BUS;
using COM.Services;


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

using COM.MOD.Jwt;
using COM.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using COM.Services;
using COM.ULT;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Data;
using System.Net;

[ApiController]
[Route("api/[controller]")]
public class TaiKhoanController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AuthService _authService;

    public TaiKhoanController(IConfiguration configuration, AuthService authService)
    {
        _configuration = configuration;
       _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] TaiKhoanMOD item)
    {
        try
        {
            List<Claim> claims;
            string role;
            bool isAuthenticated;
            int userID = 0;

            var user = TaiKhoanDAL.DangNhap(item, out userID, out claims, out role, out isAuthenticated);

            if (!isAuthenticated)
            {
                return Ok(new BaseResultMOD
                {
                    Status = 0,
                    Message = "Tài khoản hoặc mật khẩu không đúng hoặc bị vô hiệu hóa"
                });
            }

            var (jwtToken, refreshToken) = _authService.GenerateJwtAndRefreshToken(item,userID, role, claims);
            

            var chucNangClaims = claims.Where(c => c.Type == "CN").Select(c => c.Value).ToList();
            var time = claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            var R = claims.FirstOrDefault(r => r.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            var result = new jwtmod
            {
                Status = 1,
                Message = "Đăng nhập thành công",
                ID = user.ID,
                Username = user.Username,
                Role = R,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                TimeOut = time,
                ChucNangVaQuyen = chucNangClaims,
                Token = jwtToken,
                RefreshToken = refreshToken
            };

            if ((user.ID !=0) && !string.IsNullOrEmpty(refreshToken))
            {
                TaiKhoanDAL.LuuRefreshToken(user.ID, refreshToken);

                Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
                {
                    HttpOnly = true, // tránh JS đọc được
                    Secure = true,   // cần HTTPS
                    SameSite = SameSiteMode.None, // nếu dùng frontend ở domain khác
                    Expires = DateTimeOffset.UtcNow.AddDays(1) // thời gian hết hạn

                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi login: " + ex.Message);
            return Ok(new BaseResultMOD
            {
                Status = 500,
                Message = "lỗi login"
            });
        }
    }
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("refresh_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",  // áp dụng cho tất cả các site
        });

        return Ok(new BaseResultMOD
        {
            Status = 200,
            Message = "Đăng xuất thành công"
        });

    }
    [HttpGet("refresh")]
    public IActionResult RefreshToken()
    {
        // Đọc cookie tên "refreshToken"
        if (Request.Cookies.TryGetValue("refresh_token", out string refreshToken))
        {
          

        var claims = new List<Claim>(); // Initialize the claims variable
            //string role;
            //bool isAuthenticated;
            //int userID = 0;
            //string email;

         
        var refreshTokenFromDb = _authService.GetRefreshTokenFromDatabaseNoID(refreshToken);

        if (refreshTokenFromDb == null)
        {
            return Unauthorized(new BaseResultMOD
            {
                Status = 0,
                Message = "Refresh Token không tồn tại hoặc không hợp lệ."
            });
        }

        // Kiểm tra tính hợp lệ của Refresh Token
        if (refreshTokenFromDb.ExpirationDate < DateTime.UtcNow)
        {
            return Unauthorized(new BaseResultMOD
            {
                Status = 0,
                Message = "Refresh Token đã hết hạn."
            });
            }
           
            var user = _authService.GetUserInfoFromRefreshToken(refreshToken, out claims); // Use 'out' keyword for the second argument
            var chucNangClaims = claims.Where(c => c.Type == "CN").Select(c => c.Value).ToList();
           

            //var time = claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            //var R = claims.FirstOrDefault(r => r.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            var (jwtToken, newRefreshToken) = _authService.GenerateJwtAndRefreshTokenNoID(claims,user.UserID,user.Email,user.Role);
            //var tokeninf = claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            var time = claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;

            // Cập nhật Refresh Token mới vào cơ sở dữ liệu
            _authService.UpdateRefreshTokenInDatabaseNoID(refreshToken, newRefreshToken);
            Response.Cookies.Append("refresh_token", newRefreshToken, new CookieOptions
            {
                HttpOnly = true, // tránh JS đọc được
                Secure = true,   // cần HTTPS
                SameSite = SameSiteMode.None, // nếu dùng frontend ở domain khác
                Expires = DateTimeOffset.UtcNow.AddDays(1) // thời gian hết hạn

            });

            var result = new jwtmod
            {
                Status = 1,
                Message = "Đăng nhập thành công",
                ID = user.UserID,
                Username = null,
                Role = user.Role,
                PhoneNumber = null,
                Email = user.Email,
                TimeOut = time,
                ChucNangVaQuyen = chucNangClaims,
                Token = jwtToken,
                RefreshToken = newRefreshToken
            };

            return Ok(result);
        }
        else
        {
            // Cookie không tồn tại
            return Unauthorized(new { message = "Refresh token not found" });
        }
    }

    [HttpGet("refresh2")]
    public IActionResult RefreshToken(string refreshToken)
    {
        // Đọc cookie tên "refreshToken"
        if (refreshToken != null)
        {


            var claims = new List<Claim>(); // Initialize the claims variable
                                            //string role;
                                            //bool isAuthenticated;
                                            //int userID = 0;
                                            //string email;


            var refreshTokenFromDb = _authService.GetRefreshTokenFromDatabaseNoID(refreshToken);

            if (refreshTokenFromDb == null)
            {
                return Unauthorized(new BaseResultMOD
                {
                    Status = 0,
                    Message = "Refresh Token không tồn tại hoặc không hợp lệ."
                });
            }

            // Kiểm tra tính hợp lệ của Refresh Token
            if (refreshTokenFromDb.ExpirationDate < DateTime.UtcNow)
            {
                return Unauthorized(new BaseResultMOD
                {
                    Status = 0,
                    Message = "Refresh Token đã hết hạn."
                });
            }

            var user = _authService.GetUserInfoFromRefreshToken(refreshToken, out claims); // Use 'out' keyword for the second argument
            var chucNangClaims = claims.Where(c => c.Type == "CN").Select(c => c.Value).ToList();


            //var time = claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            //var R = claims.FirstOrDefault(r => r.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            var (jwtToken, newRefreshToken) = _authService.GenerateJwtAndRefreshTokenNoID(claims, user.UserID, user.Email, user.Role);
            //var tokeninf = claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            var time = claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;

            // Cập nhật Refresh Token mới vào cơ sở dữ liệu
            _authService.UpdateRefreshTokenInDatabaseNoID(refreshToken, newRefreshToken);

            var result = new jwtmod
            {
                Status = 1,
                Message = "Đăng nhập thành công",
                ID = user.UserID,
                Username = null,
                Role = user.Role,
                PhoneNumber = null,
                Email = user.Email,
                TimeOut = time,
                ChucNangVaQuyen = chucNangClaims,
                Token = jwtToken,
                RefreshToken = newRefreshToken
            };

            return Ok(result);
        }
        else
        {
            // Cookie không tồn tại
            return Unauthorized(new { message = "Refresh token not found" });
        }
    }


}

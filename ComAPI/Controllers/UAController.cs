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
    [HttpPost("refresh")]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        // Kiểm tra tính hợp lệ của AccessToken và lấy Principal
        List<Claim> claims = new List<Claim>();
        var principal = _authService.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);
        var cvk = new jwtmod();
        if (principal == null)
        {
            return Unauthorized(new BaseResultMOD
            {
                Status = 0,
                Message = "AccessToken hết hạn hoặc không hợp lệ."
            });
        }

        // Lấy secret key từ cấu hình hoặc từ nơi khác
        string secretKey = _configuration["Jwt:Key"];
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

        var handler = new JwtSecurityTokenHandler();
        SecurityToken token;

        principal = handler.ValidateToken(refreshTokenRequest.AccessToken, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false, // Chỉnh sửa cài đặt theo yêu cầu
            ValidateAudience = false, // Chỉnh sửa cài đặt theo yêu cầu
        }, out token);
        // Lấy UserID từ AccessToken

        var userId = principal.Claims.FirstOrDefault(c => c.Type == "ID")?.Value;

        // Kiểm tra Refresh Token có tồn tại và hợp lệ trong cơ sở dữ liệu
        var refreshTokenFromDb = _authService.GetRefreshTokenFromDatabase(userId, refreshTokenRequest.RefreshToken);

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


        var (jwtToken, newRefreshToken) = _authService.GenerateJwtAndRefreshToken2(refreshTokenFromDb.UserId, principal.Claims.ToList());

        // Cập nhật Refresh Token mới vào cơ sở dữ liệu
        _authService.UpdateRefreshTokenInDatabase(userId, refreshTokenRequest.RefreshToken, newRefreshToken);

        var aidi = principal.Claims.FirstOrDefault(n => n.Type == "idUser")?.Value;

        var name = principal.Claims.FirstOrDefault(n => n.Type == "Username")?.Value;
        var time = principal.Claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
        var R = principal.Claims.FirstOrDefault(r => r.Type == "NhomNguoiDung")?.Value;


        var response = new jwtRefreshMod
        {
            Status = 1,
            Message = "Refresh Token thành công",
            Token = jwtToken,
            RefreshToken = newRefreshToken,
        };

        return Ok(response);
    }

    
}

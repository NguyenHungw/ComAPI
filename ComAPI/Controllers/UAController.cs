using COM.MOD.Jwt;
using COM.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using COM.Services;

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

            var user = TaiKhoanDAL.DangNhap(item, out claims, out role, out isAuthenticated);

            if (!isAuthenticated)
            {
                return Unauthorized(new BaseResultMOD
                {
                    Status = 0,
                    Message = "Tài khoản hoặc mật khẩu không đúng hoặc bị vô hiệu hóa"
                });
            }

            var (jwtToken, refreshToken) = _authService.GenerateJwtAndRefreshToken(item, role, claims);

            var chucNangClaims = claims.Where(c => c.Type == "CN").Select(c => c.Value).ToList();
            var time = claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            var R = claims.FirstOrDefault(r => r.Type == "NhomNguoiDung")?.Value;

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
}

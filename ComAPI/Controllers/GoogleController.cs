using COM.BUS;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using COM.MOD;
using COM.Services;
using COM.MOD.Jwt;

namespace ComAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GoogleController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AuthService _authService;

        public GoogleController(IConfiguration config, AuthService authService)
        {
            _config = config;
            _authService = authService;
        }
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return Unauthorized();

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var user = new GoogleUserMOD
            {
                Name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                Email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                //ProviderKey = claims?.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value
            };

            var providerKey = claims?.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;

            var bus = new ExternalLoginBUS(_config);
            int userId = bus.HandleGoogleLogin(user, providerKey);

            var GoogleJWT = new { userId, user.Name, user.Email };
            bool isAuthenticated = true;

            // Fix for CS0165: Initialize 'role'  
            string role = string.Empty;
            List<Claim> claimsFB;
            var userCheckRole = TaiKhoanDAL.DangNhapFB(GoogleJWT.Email, out claimsFB, out role, out isAuthenticated);

            if (!isAuthenticated)
            {
                return Unauthorized(new BaseResultMOD
                {
                    Status = 0,
                    Message = "Tài khoản hoặc mật khẩu không đúng hoặc bị vô hiệu hóa"
                });
            }
            // Fix for CS1503 (Argument 3): Convert 'claims' to List<Claim>  
            //List<Claim> claimsList = claims?.ToList() ?? new List<Claim>();

            // Fix for CS1503 (Argument 1): Create a TaiKhoanMOD object  
            var taiKhoan = new TaiKhoanMOD
            {
                Email = GoogleJWT.Email,

            };

            var (jwtToken, refreshToken) = _authService.GenerateJwtAndRefreshTokenFB(taiKhoan.Email, role, claimsFB);

            var chucNangClaims = claimsFB.Where(c => c.Type == "CN").Select(c => c.Value).ToList();
            var time = claimsFB.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            var R = claimsFB.FirstOrDefault(r => r.Type == "NhomNguoiDung")?.Value;

            var jwtresult = new jwtmod
            {
                Status = 1,
                Message = "Đăng nhập thành công",
                ID = userId,
                Username = GoogleJWT.Name,
                Role = R,
                Email = GoogleJWT.Email,
                TimeOut = time,
                ChucNangVaQuyen = chucNangClaims,
                Token = jwtToken,
                RefreshToken = refreshToken
            };

            if ((userId != 0) && !string.IsNullOrEmpty(refreshToken))
            {
                TaiKhoanDAL.LuuRefreshToken(userId, refreshToken);
            }

            return Ok(jwtresult);
        }

        [HttpGet("login-google")]
        public IActionResult LoginGoogle()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/Google/google-callback" // Khi login thành công sẽ về đây
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }





       
    }

}

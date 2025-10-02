using COM.BUS;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using COM.MOD;
using COM.MOD.Jwt;
using COM.Services;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ComAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AuthService _authService;

        public AuthController(IConfiguration config, AuthService authService)
        {
            _config = config;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet("facebook-callback")]
        public async Task<IActionResult> FacebookCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return Unauthorized();

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var user = new FacebookUserMOD
            {
                Name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                Email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            };
            var providerKey = claims?.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;
            var bus = new ExternalLoginBUS(_config);
            int userId = bus.HandleFacebookLogin(user, providerKey);
            var FacebookJWT = new { userId, user.Name, user.Email };
            bool isAuthenticated = true;

            // Fix for CS0165: Initialize 'role'  
            string role = string.Empty;
            int userID = 0;
            List<Claim> claimsFB;
            var userCheckRole = TaiKhoanDAL.DangNhapFB(FacebookJWT.Email, out userID, out claimsFB, out role, out isAuthenticated);

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
            //var taiKhoan = new TaiKhoanMOD
            //{
            //    Email = FacebookJWT.Email,
               
            //};

            var (jwtToken, refreshToken) = _authService.GenerateJwtAndRefreshTokenFB(FacebookJWT.Name,FacebookJWT.Email, FacebookJWT.userId, role, claimsFB);

            var chucNangClaims = claimsFB.Where(c => c.Type == "CN").Select(c => c.Value).ToList();
            var time = claimsFB.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            var R = claimsFB.FirstOrDefault(r => r.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            var jwtresult = new jwtmod
            {
                Status = 1,
                Message = "Đăng nhập thành công",
                ID = userId,
                Username = FacebookJWT.Name,
                Role = R,
                Email = FacebookJWT.Email,
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

        // Đánh dấu đây là API GET, đường dẫn là /api/auth/login-facebook
        [AllowAnonymous]

        [HttpGet("login-facebook")]
        public IActionResult LoginFacebook()
        {
            // Tạo một đối tượng AuthenticationProperties
            // Đây là nơi bạn cấu hình các tuỳ chọn cho quá trình xác thực
            var props = new AuthenticationProperties
            {
                // RedirectUri: Khi Facebook xác thực thành công, sẽ chuyển hướng user về API callback này
                RedirectUri = "/api/auth/facebook-callback"
            };

            // Challenge: Yêu cầu hệ thống chuyển hướng tới trang login Facebook
            // props: cấu hình RedirectUri sẽ tự đính kèm trong luồng xác thực
            // FacebookDefaults.AuthenticationScheme: chỉ định đây là xác thực bằng Facebook

            return Challenge(props, FacebookDefaults.AuthenticationScheme); // gửi thách thức đăng nhập tới facebook
        }
    }
}







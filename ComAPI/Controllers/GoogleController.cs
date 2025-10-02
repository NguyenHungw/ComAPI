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
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using COM.ULT;

namespace ComAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]

    public class GoogleController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AuthService _authService;

        public GoogleController(IConfiguration config, AuthService authService)
        {
            _config = config;
            _authService = authService;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var claims = HttpContext.User.Identities.FirstOrDefault()?.Claims;

            int.TryParse(claims?.FirstOrDefault(i => i.Type == "ID")?.Value, out var id);
            var role = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
           // var claimsIdentity = (ClaimsIdentity)User.Identity;

            var name = claims?.FirstOrDefault(c => c.Type == "FullName")?.Value;

            //var name = claims?.FirstOrDefault(name => name.Type == "username")?.Value;

            var phone = claims?.FirstOrDefault(name => name.Type == "username")?.Value;

            var email =claims?.FirstOrDefault(name => name.Type == "Email")?.Value;
            var time = claims?.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            var chucNangClaims = claims?.Where(c => c.Type == "CN").Select(c => c.Value).ToList();


            if (HttpContext.Response.StatusCode==200&&role !=null)
            {
                var result = new jwtmod
                {
                    Status = 1,
                    Message = "Đăng nhập thành công",
                    ID = id,
                    Username = name,
                    Role = role,
                    Email = email,
                    TimeOut = time,
                    ChucNangVaQuyen = chucNangClaims,
                    Token = null,
                    RefreshToken = null
                };
                return Ok(new { data = result });
            }
            //return Ok(new { Status = 401, Mess = "Không có dữ liệu" });
            return Unauthorized(new
            {
                Status=401,
                Message="Bạn cần AccessToken để truy cập API"

            });
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
            int userID = 0;
            // Fix for CS0165: Initialize 'role'  
            string role = string.Empty;
            List<Claim> claimsFB;
            var userCheckRole = TaiKhoanDAL.DangNhapFB(GoogleJWT.Email,out userID, out claimsFB, out role, out isAuthenticated);

            if (!isAuthenticated)
            {
                return Unauthorized(new BaseResultMOD
                {
                    Status = 0,
                    Message = "Tài khoản hoặc mật khẩu không đúng hoặc bị vô hiệu hóa"
                });
            }
            var (jwtToken, refreshToken) = _authService.GenerateJwtAndRefreshTokenFB(GoogleJWT.Name, GoogleJWT.Email, GoogleJWT.userId, role, claimsFB);

            var chucNangClaims = claimsFB.Where(c => c.Type == "CN").Select(c => c.Value).ToList();
            var time = claimsFB.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            var R = claimsFB.FirstOrDefault(r => r.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            var jwtresult = new jwtmod
            {
                Status = 1,
                Message = "Đăng nhập thành công",
                ID = userId,
                Username = GoogleJWT.Name,
                Role = role,
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
            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true, // tránh JS đọc được
                Secure = true,   // cần HTTPS
                SameSite = SameSiteMode.None, // nếu dùng frontend ở domain khác
                Expires = DateTimeOffset.UtcNow.AddDays(1) // thời gian hết hạn

            });

            //return Ok(jwtresult);
            return Redirect($"http://localhost:8888/");
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("login-google")]
        public IActionResult LoginGoogle()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/Google/google-callback" // Khi login thành công sẽ về đây
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("login-google-link")]
        public IActionResult GetGoogleLoginLink()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/api/Google/google-callback"
            };

            // Gọi scheme Google để lấy URL sẽ chuyển hướng tới
            var challengeResult = Challenge(properties, GoogleDefaults.AuthenticationScheme) as ChallengeResult;

            // Lấy URL login Google thông qua AuthenticationHandler
            // Nhưng do nó không expose ra URL trực tiếp, ta làm workaround:
            // Trả về URL thủ công (Google OAuth URL + clientId + redirectUri...)

            // Cách 1: Build URL thủ công (nếu không dùng middleware)
            var clientId = "34380660482-4lcr5s6j4o75eha13a8ov38kus0p3686.apps.googleusercontent.com";
            var redirectUri = "http://localhost:2222/api/Google/google-callback";
            var scope = "openid%20email%20profile";

            var url = $"https://accounts.google.com/o/oauth2/v2/auth" +
                      $"?client_id={clientId}" +
                      $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                      $"&response_type=code" +
                      $"&scope={scope}" +
                      $"&access_type=offline" +
                      $"&prompt=consent";

            return Ok(new
            {
                LoginUrl = url
            });
        }

    }

}

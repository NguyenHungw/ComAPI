using COM.BUS;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using COM.MOD;

namespace ComAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

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

            // Gọi DAL để xử lý
            int userId = bus.HandleFacebookLogin(user, providerKey);

            return Ok(new { userId, user.Name, user.Email });
        }

        [HttpGet("login-facebook")]
        public IActionResult LoginFacebook()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/facebook-callback"
            };

            return Challenge(props, FacebookDefaults.AuthenticationScheme);
        }
    }
}

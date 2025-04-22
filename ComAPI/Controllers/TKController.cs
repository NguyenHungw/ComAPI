
using COM.ULT;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using COM.BUS;
using COM.MOD;

namespace COM.Controllers.PhanQuyenVaTaiKhoan
{
    [Route("api/[controller]")]
    [ApiController]

    public class TKController : ControllerBase
    {
      
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]


        public IActionResult Register([FromBody] RegisterUSER item)
        {
            if (item == null) return BadRequest();
            var Result = new UserBUS().DangKy(item);
            if (Result != null) return Ok(Result);
            else return NotFound();

        }

        
    }
}

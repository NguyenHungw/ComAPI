
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
        [HttpGet]
        [Route("DanhSachUser")]
        [AllowAnonymous]
        public IActionResult DanhSachUser(int page, int size)
        {
            if (page == 0 || size == 0) return BadRequest();
            var Result = new UserBUS().DanhSachUserBUS(page, size);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpPut]
        [Route("SuaUser")]
        //[AllowAnonymous]
        public IActionResult UpdateUser([FromBody] UserUpdateMOD item)
        {
            if (item.UserID == 0 || item.UserID == null) return BadRequest();
            var Result = new UserBUS().SuaUserBUS(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpGet("ChiTietUser")]
        public IActionResult ChiTietUser(int id)
        {
            if(id == 0) return BadRequest();
            var result = new UserBUS().ChiTietUserBUS(id);
            if (result != null) return Ok(result);
            else return NotFound();
        }
        
    }
}

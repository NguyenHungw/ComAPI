using COM.BUS;
using COM.BUS.SanPham;
using COM.MOD.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComAPI.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {

        [HttpGet]
        [Route("DSDonHang")]
        // [Authorize]

        public IActionResult dsGioHang(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new DonHangBUS().dsDonHang(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpGet("getChiTietDonHang")]
        public IActionResult getChiTietDH(string orderID)
        {
            var Result = new DonHangBUS().ChiTietDonHangIMGBUS(orderID);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        [HttpPut("UpdateTrangThaiDH")]
        public IActionResult UpdateTrangThaiDH (string orderID,int id)
        {
            var Result = new DonHangBUS().CapNhatTrangThaiDonHang1BUS(orderID, id);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }
        
        /*        [HttpGet]
                [Route("DSGioHangUser{UserID}")]
                public IActionResult dsGioHangUser(int UserID)
                {
                    if (UserID < 1) return BadRequest();
                    else
                    {
                        var Result = new GioHangBUS().dsGioHangUserThanhToan(UserID);
                        if (Result != null) return Ok(Result);
                        else return NotFound();
                    }
                }

                [HttpPost]
                [Route("ThemGioHang")]
                //[Authorize]

                public IActionResult ThemGH([FromBody] GioHangMOD item)
                {
                    if (item.UserID == null || item.UserID <=0) return BadRequest();
                    else
                    {
                        var Result = new GioHangBUS().ThemGH(item);
                        if (Result != null) return Ok(Result);
                        else return NotFound();
                    }
                }

                [HttpPut]
                [Route("SuaGioHang")]
                //[Authorize]

                public IActionResult SuaDV([FromBody] GioHangMOD item)
                {
                    if (item == null) return BadRequest();
                    else
                    {
                        var Result = new GioHangBUS().SuaGH(item);
                        if (Result != null) return Ok(Result);
                        else return NotFound();

                    }
                }
                [HttpDelete]
                [Route("XoaGioHang")]
                //[Authorize]

                public IActionResult XoaCN(int id)
                {
                    if (id == null) return BadRequest();
                    else
                    {
                        var Result = new GioHangBUS().XoaGH(id);
                        if (Result != null) return Ok(Result);
                        else return NotFound();
                    }
                }*/
    }
}

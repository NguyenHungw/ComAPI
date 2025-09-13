using COM.BUS;
using COM.BUS.SanPham;
using COM.MOD.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComAPI.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangController : ControllerBase
    {

        [HttpGet]
        [Route("DSGioHang")]
        // [Authorize]

        public IActionResult dsGioHang(int page)
        {
            //var userclaim = User.Claims;
            //bool check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLCN") && claim.Value.Contains("Xem"))
            //    {
            //        check = true;
            //        break;
            //    }
            //}

            //if (check)
            //{
            if (page < 1) return BadRequest();
            else
            {
                var Result = new GioHangBUS().dsGioHang(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }

            //}
            //else
            //{
            //    return NotFound(new BaseResultMOD
            //    {
            //        Status = -99,
            //        Message = ULT.Constant.NOT_ACCESS
            //    });
            //}



        }
        [HttpGet]
        [Route("DSGioHangUser{UserID}")]
        public IActionResult dsGioHangUser(int UserID)
        {
            //var userclaim = User.Claims;
            //bool check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLCN") && claim.Value.Contains("Xem"))
            //    {
            //        check = true;
            //        break;
            //    }
            //}

            //if (check)
            //{
            if (UserID < 1) return BadRequest();
            else
            {
                var Result = new GioHangBUS().dsGioHangUserThanhToan(UserID);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }

            //}
            //else
            //{
            //    return NotFound(new BaseResultMOD
            //    {
            //        Status = -99,
            //        Message = ULT.Constant.NOT_ACCESS
            //    });
            //}



        }

        [HttpPost]
        [Route("ThemGioHang")]
        //[Authorize]

        public IActionResult ThemGH([FromBody] GioHangMOD item)
        {

            //var userclaim = User.Claims;
            //var check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLCN") && claim.Value.Contains("Them"))
            //    {
            //        check = true;
            //        break;
            //    }
            //}
            //if (check)
            //{
            if (item.UserID == null || item.UserID <=0) return BadRequest();
            else
            {
                var Result = new GioHangBUS().ThemGH(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            //}
            //else
            //{
            //    return NotFound(new BaseResultMOD
            //    {
            //        Status = -99,
            //        Message = ULT.Constant.NOT_ACCESS
            //    });
            //}
        }

        [HttpPut]
        [Route("SuaGioHang")]
        //[Authorize]

        public IActionResult SuaDV([FromBody] GioHangMOD item)
        {
            //var userclaim = User.Claims;
            //var check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLCN") && claim.Value.Contains("Sua"))
            //    {
            //        check = true;
            //        break;
            //    }
            //}
            //if (check)
            //{
            if (item == null) return BadRequest();
            else
            {
                var Result = new GioHangBUS().SuaGH(item);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }
            //}
            //else
            //{
            //    return NotFound(new BaseResultMOD
            //    {
            //        Status = -99,
            //        Message = ULT.Constant.NOT_ACCESS
            //    });
            //}






        }
        [HttpDelete]
        [Route("XoaGioHang")]
        //[Authorize]

        public IActionResult XoaCN(int id)
        {
            //var userclaim = User.Claims;
            //var check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLCN") && claim.Value.Contains("Xoa"))
            //    {
            //        check = true;
            //        break;
            //    }
            //}

            //if (check)
            //{

            if (id == null) return BadRequest();
            else
            {
                var Result = new GioHangBUS().XoaGH(id);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            //    }

            //    else
            //    {
            //        return NotFound(new BaseResultMOD
            //        {
            //            Status = -99,
            //            Message = ULT.Constant.NOT_ACCESS
            //        }); ;
            //    }

        }
    }
}

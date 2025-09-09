using COM.BUS;
using COM.BUS.SanPham;
using COM.MOD.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComAPI.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaSanPhamController : ControllerBase
    {

        [HttpGet]
        [Route("DanhSachDanhGiaSanPham")]
        // [Authorize]

        public IActionResult dsDanhGiaSanPham(int page)
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
                var Result = new DanhGiaSanPhamBUS().dsDanhGiaSanPham(page);
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
        [Route("ThemDanhGiaSanPham")]
        //[Authorize]

        public IActionResult ThemDanhGiaSanPham([FromBody] DanhGiaSanPhamMOD item)
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
            if (item==null) return BadRequest();
            else
            {
                var Result = new DanhGiaSanPhamBUS().ThemDanhGiaSanPhamBUS(item);
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
        [Route("SuaDanhGiaSP")]
        //[Authorize]

        public IActionResult SuaDanhGiaSP([FromBody] DanhGiaSanPhamMOD item)
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
                var Result = new DanhGiaSanPhamBUS().SuaDanhGiaSanPhamBUS(item);
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
        [Route("XoaDanhGiaSP")]
        //[Authorize]

        public IActionResult XoaDanhGia(int id)
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
                var Result = new DanhGiaSanPhamBUS().XoaDanhGia(id);
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

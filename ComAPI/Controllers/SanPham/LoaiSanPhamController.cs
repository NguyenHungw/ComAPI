using COM.BUS;
using COM.BUS.SanPham;
using COM.MOD.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComAPI.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSanPhamiController : ControllerBase
    {

        [HttpGet]
        [Route("DSLoaiSP")]
        // [Authorize]

        public IActionResult dsLSP(int page)
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
                var Result = new LoaiSanPhamBUS().dsLoaiSanPham(page);
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
        [Route("ThemLSP")]
        //[Authorize]

        public IActionResult ThemLSP([FromBody] LoaiSanPhamMOD item)
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
            if (item.TenLoaiSanPham == null || item.TenLoaiSanPham == "") return BadRequest();
            else
            {
                var Result = new LoaiSanPhamBUS().ThemLSP(item);
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
        [Route("SuaLSP")]
        //[Authorize]

        public IActionResult SuaDV([FromBody] LoaiSanPhamMOD item)
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
                var Result = new LoaiSanPhamBUS().SuaLSP(item);
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
        [Route("XoaLSP")]
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
                var Result = new LoaiSanPhamBUS().XoaLoaiSanPham(id);
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

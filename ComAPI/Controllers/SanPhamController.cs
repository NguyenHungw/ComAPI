using COM.BUS;
using COM.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace COM.Controllers.PhanQuyenVaTaiKhoan
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {

        [HttpGet]
        [Route("DSSanPham")]
       // [Authorize]

        public IActionResult dsSanPham(int page)
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
                    var Result = new SanPhamBUS().dsSanPham(page);
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
        [Route("ThemSanPham")]
        //[Authorize]

        public IActionResult ThemCN([FromBody] SanPhamMOD item )
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
                if (item == null) return BadRequest();
                else
                {
                    var Result = new SanPhamBUS().ThemSP(item);
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
        [Route("SuaSanPham")]
        //[Authorize]

        public IActionResult SuaCN([FromBody] SanPhamMOD item)
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
                    var Result = new SanPhamBUS().SuaSP(item);
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
        [Route("XoaSanPham")]
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
                    var Result = new SanPhamBUS().XoaSP(id);
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

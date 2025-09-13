using COM.BUS;
using COM.BUS.SanPham;
using COM.DAL.SanPham;
using COM.MOD;
using COM.MOD.SanPham;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ComAPI.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamPhamImageController : ControllerBase
    {

        [HttpGet]
        [Route("DSImageSanPham")]
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
                var Result = new SanPhamImageBUS().dsImageSP(page);
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
        [Route("ThemHinhAnhSP")]
        //[Authorize]

        public IActionResult ThemIMG(List<IFormFile> files, int id)
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
            if (id == null || id <= 0) return BadRequest();
            else
            {
                var Result = new SanPhamImageDAL().ThemImage(files, id);
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
        [Route("DoiViTri")]
        //[Authorize]

        public IActionResult SuaIMG(int id, int currentIndex, int nextIndex)
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
            if (id == null|| currentIndex == null || nextIndex == null) return BadRequest();
            else
            {
                var Result = new SanPhamImageBUS().DoiViTriBUS(id,currentIndex,nextIndex);
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
        [Route("DoiNhieuViTri")]
        //[Authorize]
        public IActionResult SuaNhieuIMG([FromBody] List<ImageOrderUpdateModel> list)
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
            if (list == null) return BadRequest();
            else
            {
                var Result = new SanPhamImageBUS().DoiNhieuViTri(list);
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
        [Route("XoaHinhAnh")]
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
                var Result = new SanPhamImageBUS().XoaHinhAnh(id);
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

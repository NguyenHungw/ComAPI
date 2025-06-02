using COM.BUS;
using COM.BUS.SanPham;
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

        public IActionResult ThemCN([FromBody] SanPhamMOD item)
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
        [HttpPost]
        [Route("ThemSanPhamAnhVaGia")]
        //[Authorize]

        public IActionResult ThemSPAnhVaGia(List<IFormFile> files, [FromForm] SanPhamAnhVaGiaMOD item)
        {


            if (item == null)
                return BadRequest(new BaseResultMOD { Status = -1, Message = "Thiếu thông tin sản phẩm" });

            if (files == null || files.Count == 0)
                return BadRequest(new BaseResultMOD { Status = -1, Message = "Chưa có file đính kèm" });

            var result = new SanPhamBUS().ThemSPAnhVaGia(files, item);
            if (result != null && result.Status == 1)
                return Ok(result);
            else
                return BadRequest(result);
        }
        [HttpPut]
        [Route("SuaSanPhamAnhVaGia")]
        //[Authorize]

        public IActionResult SuaSanPhamAnhVaGia(List<IFormFile> files, [FromForm] SanPhamAnhVaGiaMOD item)
        {


            if (item == null)
                return BadRequest(new BaseResultMOD { Status = -1, Message = "Thiếu thông tin sản phẩm" });

            if (files == null || files.Count == 0)
                return BadRequest(new BaseResultMOD { Status = -1, Message = "Chưa có file đính kèm" });

            var result = new SanPhamBUS().SuaSanPhamAnhVaGia(files, item);
            if (result != null && result.Status == 1)
                return Ok(result);
            else
                return BadRequest(result);
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
        [Route("XoaSanPhamAnhGia{id}")]
        //[Authorize]

        public IActionResult XoaSP_Anh_Gia(int id)
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
                var Result = new SanPhamBUS().XoaSP_Anh_Gia(id);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
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

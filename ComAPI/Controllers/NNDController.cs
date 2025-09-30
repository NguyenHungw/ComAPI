using COM.BUS;
using COM.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace COM.Controllers.PhanQuyenVaTaiKhoan
{
    [Route("api/[controller]")]
    [ApiController]
    public class NNDController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachNND")]
        //[Authorize]
        public IActionResult DanhSachNND(int page)
        {
                if (page < 1) return BadRequest();
                else
                {
                    var Result = new NhomNguoiDungBUS().DanhSachNND(page);
                    if (Result != null) return Ok(Result);
                    else return NotFound();
                }
        }
        [HttpGet]
        [Route("DanhSachNNDPage")]
        //[Authorize]
        public IActionResult DanhSachNNDPage(int page , int size)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new NhomNguoiDungBUS().DanhSachNNDPage(page, size);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpGet]
        [Route("ChiTietNNDPage")]
        //[Authorize]
        public IActionResult ChiTietNND(int page, int size,int id)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new NhomNguoiDungBUS().ChiTietNNDPage(page, size,id);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpPost]
        [Route("ThemNND")]
        //[Authorize]
        public IActionResult ThemNND([FromBody] ThemMoiNND item)
        {
            //var userclaim = User.Claims;
            //var check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLNND") && claim.Value.Contains("Them"))
            //    {
            //        check = true;
            //        break;
            //    }
            //}
            //if (check)
            //{
                if (item == null) return BadRequest();
                var Result = new NhomNguoiDungBUS().ThemNND(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
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
        [Route("SuaNND")]
        //[Authorize]
        public IActionResult SuaNND([FromForm] DanhSachNhomNDMOD item)
        {
            //var userclaim = User.Claims;
            //var check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLNND") && claim.Value.Contains("Sua"))
            //    {
            //        check = true;
            //        break;
            //    }
            //}
            //if (check)
            //{
                if (item == null) return BadRequest();
                var Result = new NhomNguoiDungBUS().SuaNND(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
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
        [Route("DeleteNND")]
        //[Authorize]

        public IActionResult DeleteNND(int id)
        {
            //var userclaim = User.Claims;
            //var check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLNND") && claim.Value.Contains("Xoa"))
            //    {
            //        check = true;
            //        break;
            //    }
            //}
            //if (check)
            //{
                if (id == null) return BadRequest();
                var Result = new NhomNguoiDungBUS().XoaNND(id);
                if (Result != null) return Ok(Result);
                else return NotFound();
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
    }
}

using COM.BUS;
using COM.BUS.SanPham;
using COM.MOD.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComAPI.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonViController : ControllerBase
    {

        [HttpGet]
        [Route("DSDonVi")]
        // [Authorize]

        public IActionResult dsDonVi()
        {

                var Result = new DonViBUS().dsDonVi();
                if (Result != null) return Ok(Result);
                else return NotFound();

        }
        [HttpGet]
        [Route("DSDonViPage")]
        // [Authorize]

        public IActionResult dsDonViPage( int p,int s)
        {

            var Result = new DonViBUS().dsDonViPage(p,s);
            if (Result != null) return Ok(Result);
            else return NotFound();

        }

        [HttpPost]
        [Route("ThemDonVi")]
        //[Authorize]

        public IActionResult ThemCN([FromBody] DonViMOD model)
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
            if (model.TenDonVi == null || model.TenDonVi == "") return BadRequest();
            else
            {
                var Result = new DonViBUS().ThemDV(model);
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
        [Route("SuaDonVi")]
        //[Authorize]

        public IActionResult SuaDV([FromBody] DonViMOD item)
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
                var Result = new DonViBUS().SuaDV(item);
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
        [Route("XoaDonVi")]
        //[Authorize]

        public IActionResult XoaDV(int id)
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
                var Result = new DonViBUS().XoaDV(id);
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

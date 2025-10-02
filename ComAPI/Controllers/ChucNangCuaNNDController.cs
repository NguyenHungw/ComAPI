﻿using COM.BUS;
using COM.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Net.WebSockets;
using System.Security.Claims;

namespace CT.Controllers.PhanQuyenVaTaiKhoan
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChucNangCuaNNDController : ControllerBase
    {

        [HttpGet]
        [Route("DanhSachCNCuaNND")]
        //[Authorize]
        public IActionResult dscncuannd(int page)
        {
            //var userclaim = User.Claims;
            //var check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLQuyen") && claim.Value.Contains("Xem"))
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
                    var Result = new ChucNangCuaNNDBUS().dsCNCuannd(page);
                    if (Result != null) return Ok(Result);
                    else return NotFound();
                }
            //}
            //else
            //{
            //    return NotFound(new BaseResultMOD
            //    {
            //        Status = -99,
            //        Message = COM.ULT.Constant.NOT_ACCESS
            //    });
            //}



        }
        [HttpGet]
        [Route("DanhSachCNCuaNND2")]
        //[Authorize]
        public IActionResult dscncuannd2(int page)
        {
            //var userclaim = User.Claims;
            //var check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLQuyen") && claim.Value.Contains("Xem"))
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
                    var Result = new ChucNangCuaNNDBUS().dsCNCuannd2(page);
                    if (Result != null) return Ok(Result);
                    else return NotFound();
                }
        //    }
        //    else
        //    {
        //        return NotFound(new BaseResultMOD
        //        {
        //            Status = -99,
        //            Message = COM.ULT.Constant.NOT_ACCESS
        //        });
        //    }



        }
        [HttpPost]
        [Route("ThemChucNangCuaNND")]
        //[Authorize]
        public IActionResult ThemCN([FromBody] ThemChucNangCuaNNDMOD item)
        {
            //var userclaim = User.Claims;
            //var check = false;
            //foreach (var claim in userclaim)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLQuyen") && claim.Value.Contains("Them"))
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
                    var result = new ChucNangCuaNNDBUS().Them(item);
                    if (result != null) return Ok(result);
                    else return NotFound();
                }
            //}
            //else
            //{
            //    return NotFound(new BaseResultMOD
            //    {
            //        Status = -99,
            //        Message = COM.ULT.Constant.NOT_ACCESS
            //    });
            //}


        }
        [HttpPut]
        [Route("SuaCNCN")]
        //[Authorize]
        public IActionResult SuaCN([FromBody] ChucNangCuaNNDMOD item)
        {

            //var userclain = User.Claims;
            //var check = false;
            //foreach (var claim in userclain)
            //{
            //    if (claim.Type == "CN" && claim.Value.Contains("QLQuyen") && claim.Value.Contains("Sua"))
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
                    var result = new ChucNangCuaNNDBUS().Sua(item);
                    if (result != null) return Ok(result);
                    else return NotFound();

                }
            //    }
            //    else
            //    {
            //        return NotFound(new BaseResultMOD
            //        {
            //            Status = -99,
            //            Message = COM.ULT.Constant.NOT_ACCESS
            //        });
            //    }
        }

        [HttpDelete]
        [Route("XoaCNCN")]
        //[Authorize]

        public IActionResult XoaCN(int id)
        {

        
                if (id == null) return BadRequest();
                else
                {
                    var result = new ChucNangCuaNNDBUS().Xoa(id);
                    if (result != null) return Ok(result);
                    else return NotFound();

                }
       

        }
        [HttpDelete]
        [Route("XoaCNcuaNND")]
        public IActionResult XoaCNcuaNND(int idNND,int idCN)
        {


            if (idNND == null || idCN ==null) return BadRequest();
            else
            {
                var result = new ChucNangCuaNNDBUS().XoaCNcuaNND(idNND,idCN);
                if (result != null) return Ok(result);
                else return NotFound();

            }


        }

        [HttpPost]
        [Route("ChiTietCNCNND")]
        //[Authorize]

        public IActionResult ChiTietCNCNND(int id)
        {

          
                if (id == null || id <= 0) return BadRequest();
                var Result = new ChucNangCuaNNDBUS().ChiTCNCN(id);
                if (Result != null) return Ok(Result);
                else return NotFound();
        

        }
    }
}

using COM.BUS.SanPham;
using COM.DAL.SanPham;
using COM.MOD.SanPham;
using COM.Services.Donhang;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly DonHangServices _donHangService;

        public ThanhToanController(DonHangServices donHangService)
        {
            _donHangService = donHangService;
        }

        //[HttpPost("ThanhToan")]
        //public IActionResult ThanhToan(int UserID)
        //{
        //    var gioHangBUS = new GioHangBUS();
        //    var data = gioHangBUS.dsGioHangUser(UserID);
        //    var url =  _donHangService.TaoDonHangVaThanhToan(data);
        //    return Redirect(url);
        //}
    }
}

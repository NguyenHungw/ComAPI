using System.Text.Json;
using COM.BUS.SanPham;
using COM.DAL.SanPham;
using COM.MOD.SanPham;
using COM.Services.Donhang;
using COM.Services.Vnpay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VNPAY.NET.Utilities;

namespace ComAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly DonHangServices _donHangService;
        private readonly IVnPayService _vnPayService;


        public ThanhToanController(DonHangServices donHangService, IVnPayService vnPayService)
        {
            _donHangService = donHangService;
            _vnPayService = vnPayService;
        }

        [HttpPost("ThanhToan/{UserID}")]

        public IActionResult ThanhToan(int UserID)
        {
            //var ipAddress = NetworkHelper.GetIpAddress(HttpContext);
            var gioHangBUS = new GioHangBUS();
            var data = gioHangBUS.dsGioHangUserThanhToan(UserID);

            if (data.Status != 1 || data.Data == null)
            {
                return BadRequest(new { message = "Giỏ hàng không hợp lệ hoặc rỗng." });
            }

            //dynamic rawData = data.Data;

            var danhSachGioHang = data.Data as List<GioHangUserThanhToanMOD>; //Ép Data về đúng kiểu danh sách sản phẩm trong giỏ hàng.
            if (danhSachGioHang == null || !danhSachGioHang.Any())
            {
                return BadRequest(new { message = "Giỏ hàng trống." });
            }
            //var order = new

            // Serialize danhSachGioHang to a JSON string to fix the type mismatch
            //string result = JsonSerializer.Serialize(danhSachGioHang);
            var donhang = new DonHangRequestModel
            {

                UserID = UserID,
                PhuongThucThanhToan = "VNPAY", // Mặc định là VNPAY, có thể thay đổi thành MOMO, COD, PAYPAL
                ChiTiet = danhSachGioHang.Select(item => new ChiTietDonHangMOD
                {
                    SanPhamID = item.SanPhamID,
                    SoLuong = item.GioSoLuong,
                    DonGia = item.DonGia,
                    TrietKhau = item.TrietKhau,
                    ThanhTien = item.TongTien
                }).ToList(),
                //Amount = (int?)danhSachGioHang.Sum(item => item.TongTien ?? 0),
                //Name = "Thanh toán đơn hàng",
                //OrderDescription = "Mô tả đơn hàng",
                //OrderType = "billpayment"

            };
            var url = _donHangService.TaoDonHangVaThanhToan(donhang);
            var url2 = _donHangService.TaoUrlThanhToan(url);
            //return Ok(new { message = "Thanh toán thành công", data = donhang });
            //return Ok(new { message = "Thanh toán thành công", data = result });
            return Created(url, url2);
            //return Ok(new { message = "Thanh toán thành công", data = danhSachGioHang });
            //return Redirect(result);
        }

        //[ApiExplorerSettings(IgnoreApi = true)]


        /// <summary>
        /// Thực hiện hành động sau khi thanh toán. URL này cần được khai báo với VNPAY để API này hoạt đồng (ví dụ: http://localhost:1234/api/Vnpay/IpnAction)
        /// </summary>
        /// <returns></returns>
        [HttpGet("IpnAction")]
        public IActionResult IpnAction()
        {
            if (Request.Query.Count == 0)
                return BadRequest(new { message = "Thiếu dữ liệu từ VNPay" });

            try
            {
                var result = _vnPayService.PaymentExecute(Request.Query);

                // VNPay yêu cầu bạn trả mã như sau:
                // Nếu thành công: RspCode = "00"
                // Nếu lỗi: RspCode = "97" hoặc "99"

                if (result.Success)
                {
                    new DonHangDAL().CapNhatTrangThaiDonHang(result.OrderId);
                    return Ok(new { RspCode = "00", Message = "Thanh toán thành công" });
                }

                return Ok(new { RspCode = "97", Message = "Thanh toán thất bại hoặc bị từ chối" });
            }
            catch (Exception ex)
            {
                return Ok(new { RspCode = "99", Message = "Lỗi xử lý: " + ex.Message });
            }
        }



        [HttpGet("Callback")]
        public IActionResult Callback()
        {
            try
            {
                if (Request.Query.Count == 0)
                    return BadRequest(new { message = "Thiếu dữ liệu từ VNPay" });

                var response = _vnPayService.PaymentExecute(Request.Query);

                if (response == null)
                    return BadRequest(new { message = "Không xác thực được thanh toán" });
                if (response.Success)
                {
                    // Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu
                    new DonHangDAL().CapNhatTrangThaiDonHang(response.OrderId);
                    var data = new DonHangDAL().LayChiTietDonHangTheoID(response.OrderId);
                    //ép kiểu data.Data thành 1 list vì data.data là 1 danh sách các obj
                    if (data?.Data is List<ChiTietDonHangMOD> chiTietList)
                    {
                        foreach (var result in chiTietList)
                        {
                            new SanPhamDAL().TruSoLuongSanPham(result?.SanPhamID ?? 0, result?.SoLuong ?? 0);
                        }
                    }
                    var firstChar = new GioHangDAL().XoaGioHang(response.OrderDescription[0]-'0');
                    Console.WriteLine($"Đã xóa giỏ hàng với mã: {response.OrderDescription[0]}");
                    return Ok(new
                    {
                        message = "Thanh toán thành công",
                        data = response
                    });
                }
                else
                {
                    // Xử lý trường hợp thanh toán không thành công
                    return BadRequest(new { message = "Thanh toán thất bại hoặc bị từ chối" });


                    return Ok(new
                    {
                        message = "Thanh toán thành công",
                        data = response
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi xử lý callback",
                    error = ex.Message
                });
            }
        }



    }
}

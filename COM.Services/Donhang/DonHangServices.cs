using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using COM.DAL.SanPham;
using COM.MOD;
using COM.MOD.SanPham;
using COM.MOD.Vnpay;
using COM.Services.Vnpay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace COM.Services.Donhang
{
    public class DonHangServices : IDonHangService
    {
        private readonly IVnPayService _vnPayService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly DonHangDAL _donHangDAL;
        public DonHangServices(IHttpContextAccessor httpContextAccessor, IVnPayService vnPayService)
        {
            _httpContextAccessor = httpContextAccessor;
            _vnPayService = vnPayService;
        }
        public string TaoDonHangVaThanhToan(DonHangRequestModel item)
        {
            var orderID = DateTime.Now.Ticks.ToString();

            //var orderId = Guid.NewGuid().ToString(); // <-- sinh OrderID tại đây

            var donHang = new DonHangMOD
            {
                OrderID = orderID,
                UserID = item.UserID,
                NgayMua = DateTime.Now,
                Status = 0,  // 0: Chờ xử lý, 1: Đang giao hàng, 2: Đã giao hàng, 3: Đã hủy
                PhuongThucThanhToan = item.PhuongThucThanhToan
            };

            //if (_donHangDAL == null)
            //{
            //    throw new Exception("_donHangDAL chưa được khởi tạo!");
            //}

            //_donHangDAL.ThemDonHang(donHang);
            new DonHangDAL().ThemDonHang(donHang);

            foreach (var ct in item.ChiTiet)
            {
                ct.OrderID = orderID;
                //_donHangDAL.ThemChiTietDonHang(ct);
                new DonHangDAL().ThemChiTietDonHang(ct);
            }
            return orderID; // Trả về OrderID (tick) để sử dụng trong việc tạo URL thanh toán

        }
        public string TaoUrlThanhToan(string orderID)
        {
            var donHangResult = new DonHangDAL().LayDonHangTheoID(orderID);
            //var donhang2 = donHangResult.Data as DonHangMOD;
            //ép data sang json
            var jsonDH = JsonSerializer.Serialize(donHangResult.Data);
            List<DonHangMOD> danhSach = JsonSerializer.Deserialize<List<DonHangMOD>>(jsonDH);

            //var list = JsonSerializer.Deserialize<List<DonHangMOD>>(jsonDH); // ✅ deserialize đúng kiểu
            //var donhang2 = jsonDH?.FirstOrDefault();

            //Deserialize lại về đúng kiểu mong muốn
            // var donHang = JsonSerializer.Deserialize<DonHangMOD>(jsonDH);
            //var result = jsonDH as DonHangMOD;
            //var json = JsonSerializer.Serialize(result);

            // Fix for CS0029: Return a string instead of an anonymous type  

            //return $"{jsonDH}";
            //lấy đơn hàng đầu tiên từ danh sách
            var donhang = danhSach?.FirstOrDefault();
            //return $"{donHang2}";
            //var donhang = donHangResult.Data as DonHangMOD;
            Console.WriteLine($"PhuongThucThanhToan: '{donhang.PhuongThucThanhToan}'");
            Console.WriteLine($"Length: {donhang.PhuongThucThanhToan?.Length}");
            if (string.Equals(donhang.PhuongThucThanhToan, "VNPAY", StringComparison.OrdinalIgnoreCase))
            {
                var chiTietDonHangResult = new DonHangDAL().LayChiTietDonHangTheoID(orderID);
                var chitietdonhang = chiTietDonHangResult.Data as List<ChiTietDonHangMOD>;
                if (chitietdonhang == null)
                    throw new Exception("Không thể ép kiểu sang List<ChiTietDonHangMOD>");

                var amount = chitietdonhang.Sum(x => (x.ThanhTien * (1 - x.TrietKhau / 100m)) * x.SoLuong);
                string orderId = donhang.OrderID;
                string userID = donhang.UserID.ToString();
                var orderInfo = $"Thanh toán đơn hàng #{orderId}";

                var paymentModel = new DonHangRequestModel
                {
                    OrderID = orderId,
                    Amount = (int?)(amount ?? 0),
                    Name = userID,
                    OrderDescription = orderInfo,
                    OrderType = "billpayment"
                };

                return _vnPayService.CreatePaymentUrl(paymentModel, _httpContextAccessor.HttpContext);
            }
            else if (string.Equals(donhang.PhuongThucThanhToan, "MOMO", StringComparison.OrdinalIgnoreCase))
            {
                // Handle MOMO payment logic here if needed  
            }

            return "/thank-you?orderId=" + orderID;
        }
    }
    };

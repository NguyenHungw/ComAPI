using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.DAL.SanPham;
using COM.MOD.SanPham;
using COM.MOD.Vnpay;
using COM.Services.Vnpay;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace COM.Services.Donhang
{
    public class DonHangServices : IDonHangService
    {
        private readonly VnPayService _vnPayService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DonHangDAL _donHangDAL;

        public string TaoDonHangVaThanhToan(DonHangRequestModel item)
        {
            var orderId = Guid.NewGuid().ToString(); // <-- sinh OrderID tại đây

            var donHang = new DonHangMOD
            {
                OrderID = orderId,
                UserID = item.UserID,
                NgayMua = DateTime.Now,
                Status = 0,  // 0: Chờ xử lý, 1: Đang giao hàng, 2: Đã giao hàng, 3: Đã hủy
                PhuongThucThanhToan = item.PhuongThucThanhToan
            };

            _donHangDAL.ThemDonHang(donHang);

            foreach(var ct in item.ChiTiet)
            {
                ct.OrderID = orderId;
                _donHangDAL.ThemChiTietDonHang(ct);
            }

            if (item.PhuongThucThanhToan == "VNPAY")
            {
                var paymentModel = new PaymentInformationMOD
                {
                    Amount = item.Amount ?? 0,
                    Name = item.Name,
                    OrderDescription = item.OrderDescription,
                    OrderType = item.OrderType
                };

                return _vnPayService.CreatePaymentUrl(paymentModel, _httpContextAccessor.HttpContext);
            }

            // Thêm xử lý MOMO nếu có
            return "/DonHang/ThanhCong";
        }
    }
    }


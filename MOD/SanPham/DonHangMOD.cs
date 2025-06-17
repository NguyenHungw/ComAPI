using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace COM.MOD.SanPham
{
    public class DonHangMOD
    {
        public int? ID { get; set; }
        public string? OrderID { get; set; }
        public int? UserID { get; set; }
        public string? PhuongThucThanhToan { get; set; } // Mặc định là VNPAY, có thể thay đổi thành MOMO, COD, PAYPAL
        public DateTime? NgayMua { get; set; } = DateTime.Now;
        public int? Status { get; set; } // 0: Chờ xử lý, 1: Đang giao hàng, 2: Đã giao hàng, 3: Đã hủy
    }
    public class ChiTietDonHangMOD
    {
        public int? ID { get; set; }
        public string? OrderID { get; set; }
        public int? SanPhamID { get; set; }
        public int? SoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public decimal? TrietKhau { get; set; }
        public decimal? ThanhTien { get; set; }

    }
    public enum PaymentMethod
    {
        VNPAY,
        MOMO,
        COD,
        PAYPAL
    }
    public class DonHangRequestModel
    {
        public int? UserID { get; set; }
        public string? PhuongThucThanhToan { get; set; }
        public List<ChiTietDonHangMOD> ChiTiet { get; set; }

        // Thông tin thanh toán (cho VNPAY, MOMO...)
        public int? Amount { get; set; }
        public string? Name { get; set; }
        public string? OrderDescription { get; set; }
        public string? OrderType { get; set; }
    }

}

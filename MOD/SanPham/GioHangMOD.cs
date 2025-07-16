using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.MOD.SanPham
{
    public class GioHangMOD
    {
        public int? ID { get; set; }
        public int? SanPhamID { get; set; }
        public int? UserID { get; set; }
        public int? GioSoLuong { get; set; }
    }
    public class GioHangUserMOD
    {
        public int? SanPhamID { get; set; }
        public string? MSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public string? HinhAnh { get; set; }
        public int? GioSoLuong { get; set; }
        public decimal? GiaSauGiam { get; set; }
        public decimal? TongTien { get; set; }
        
    }
    public class GioHangUserThanhToanMOD
    {
        public int? SanPhamID { get; set; }
        public string? MSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public string? HinhAnh { get; set; }
        public int? GioSoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public decimal? TrietKhau { get; set; }
        public decimal? GiaSauGiam { get; set; }
        public decimal? TongTien { get; set; }

    }

}

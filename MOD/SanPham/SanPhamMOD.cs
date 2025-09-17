using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace COM.MOD.SanPham
{
    public class SanPhamMOD
    {
        public string? MSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public int? LoaiSanPhamID { get; set; }
        public int? DonViTinhID { get; set; }
        public string? MoTa { get; set; }
        public int? SoLuong { get; set; }


    }
    public class SanPhamTrangChuMOD
    {
        public int? ID { get; set; }
        public string? FilePath { get; set; }
        public string? TenSanPham { get; set; }
        public int? DiemDanhGia { get; set; }

        public decimal? GiaBan { get; set; }
        public decimal? SalePercent { get; set; }
        public decimal? GiaSauGiam { get; set; }
        public DateOnly? NgayBatDau { get; set; }
    }
    public class SanPhamAdminMOD
    {
        public int? ID { get; set; }
        public string? FilePath { get; set; }
        public string? TenSanPham { get; set; }

        public decimal? GiaBan { get; set; }
        public decimal? SalePercent { get; set; }
        public decimal? GiaSauGiam { get; set; }
        public string? TenLoaiSP { get;set; }
        public string? TenDonVi { get; set; }
        public string? Mota { get; set; }
        public DateOnly? NgayBatDau { get; set; }
    }
    public class SanPhamAdminNhieuIMGMOD
    {
        public int? ID { get; set; }
        public string? TenSanPham { get; set; }
        public decimal? GiaBan { get; set; }
        public decimal? SalePercent { get; set; }
        public decimal? GiaSauGiam { get; set; }
        public string? MoTa { get; set; }
        public string? TenLoaiSP { get; set; }
        public string? DonViTinh { get; set; }
        public string? AnhChinh { get; set; }
        public List<string> DanhSachAnh { get; set; } = new();
        public DateOnly? NgayBatDau { get; set; }
    }
    public class ChiTietSanPhamTrangChuMOD
    {
        public string? FilePath { get; set; }
        public string? TenSanPham { get; set; }
        public decimal? GiaBan { get; set; }

        public decimal? SalePercent { get; set; }
        public decimal? GiaSauGiam { get; set; }
        public DateOnly? NgayBatDau { get; set; }
        public string? MoTa { get; set; }
    }
    public class SanPhamAnhVaGiaMOD  
    {
        public string? MSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public int? LoaiSanPhamID { get; set; }
        public int? DonViTinhID { get; set; }
        public string? MoTa { get; set; }
        public int? SoLuong { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public decimal? GiaBan { get; set; }
        public decimal? SalePercent { get; set; }
        public decimal? GiaSauGiam { get; set; }
        //public List<SanPhamImage> HinhAnh { get; set; }
    }
    public class SanPhamImage
    {
        public string FilePath { get; set; }
        public int IndexOrder { get; set; }
    }
    public class GiaBanSanPham
    {
        public DateTime NgayBatDau { get; set; }
        public SqlMoney GiaBan { get; set; }
        public decimal SalePercent { get; set; }

    }
    public class SanPhamCombo
    {
        public string? TenCombo { get; set; }
        public string? GiaCombo { get; set; }
        public string? SoLuongCombo { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public int TrangThai { get; set; } // 0: Chưa kích hoạt, 1: Đang hoạt động, 2: Kết thúc
    }

    public class ChiTietComboSanPham
    {
        public string TenCombo { get; set; }
        public string? MSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public int? SoLuongSP { get; set; }
        public string? MoTa { get; set; }
        public decimal? GiaCombo { get; set; }
        public int TrangThai { get; set; } // 0: Chưa kích hoạt, 1: Đang hoạt động, 2: Kết thúc

    }
    
}

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
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public DateTime NgayMua { get; set; } = DateTime.Now;
    }
    public class ChiTietDonHangMOD
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int SanPhamID { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal TrietKhau { get; set; }
        public decimal ThanhTien { get; set; }

    }
}

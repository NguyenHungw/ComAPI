using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace COM.MOD.SanPham
{
    public class DanhGiaSanPhamMOD
    {
        public int? ID { get; set; }
        public int? UserID { get; set; }
        public int? SanPhamID { get; set; }
        public int? DiemDanhGia { get; set; }
        public string? NhanXet { get; set; }
        public DateTime? NgayDanhGia { set; get; } = DateTime.Now;
    }
}
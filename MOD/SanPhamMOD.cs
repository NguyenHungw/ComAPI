using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.MOD
{
    public class SanPhamMOD
    {
        public string? MSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public int? LoaiSanPhamID { get; set; }
        public int? DonViTinhID { get; set; }

    }
    public class  SanPhamAnhVaGiaMOD
    {

     public string? MSanPham { get; set; }
     public string? TenSanPham { get; set; }
        public int? LoaiSanPhamID { get; set; }
        public int? DonViTinhID { get; set; }
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
}

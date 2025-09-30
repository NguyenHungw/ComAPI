using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.MOD
{
    public class DanhSachNhomNDMOD
    {
        public int NNDID { get; set; }
        public string TenNND { get; set; }
        public string GhiChu { get; set; }

    }
    public class ThemMoiNND
    {
        public string TenNND { get; set; }
        public string GhiChu { get; set; }
    }
    public class ChiTietNND
    {
        public int? ID { get; set; }
        public int? NNDID { get; set; }
        public string? TenNND { get;set; }
        public int? ChucNangID { get; set; }
        public string? TenChucNang { get; set; } 
        public bool? Xem { get; set; }
        public bool? Them { get; set; } 
        public bool? Sua { get; set; }
        public bool? Xoa { get; set; }
    }
   
}

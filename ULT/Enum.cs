using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COM.ULT
{
    public enum Role
    {
        [EnumMember(Value = "Admin")]
        Admin,
        [EnumMember(Value = "QLSanpham")]
        QLSP,
        [EnumMember(Value = "QLDonhang")]
        QLDH,
        [EnumMember(Value = "User")]
        User,
    }

    public enum isActive
    {
        Active = 1,
        Inactive = 0,
       // Banned =2,
    }
    public enum TrangThaiThanhToan
    {
        ChuaThanhToan = 0,
        DaThanhToan = 1
    }

    public enum TrangThaiDonHang
    {
        ChoXuLy = 0,
        DangVanChuyen = 1,
        HoanTat = 2,
        DaHuy = 3,
        HoanTra =4,
    }



}

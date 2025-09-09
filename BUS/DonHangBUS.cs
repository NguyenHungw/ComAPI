using COM.DAL;
using COM.ULT;
using COM.MOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.MOD;
using COM.DAL.SanPham;

namespace COM.BUS
{
    public class DonHangBUS
    {
        public BaseResultMOD dsDonHang(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new DonHangDAL().getdsDonHang(page); }
                else
                {
                    result.Status = 0;
                    result.Message = "lỗi page";
                }
            }
            catch (Exception)
            {
                result.Status = -1;
                result.Message = " Lỗi hệ thống ";
                result.Data = null;
                throw;

            }
            return result;

        }
        //public BaseResultMOD ThemCN(string namecn)
        //{
        //    var result = new BaseResultMOD();
        //    try
        //    {
        //        if (namecn == null || namecn == "")
        //        {
        //            result.Status = 0;
        //            result.Message = " Tên chức năng không được để trống";
        //        }
        //        else
        //        {
        //            result = new DonHangDAL().ThemDonHang(namecn);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Data = -1;
        //        result.Message = Constant.API_Error_System;

        //    }
        //    return result;
        //}
        public BaseResultMOD CapNhatTrangThaiDonHangBUS(string orderID)
        {
            var result = new BaseResultMOD();
            try
            {
                if(orderID == null || orderID == "")
                {
                    result.Status = 0;
                    result.Message = " Mã đơn hàng không được để trống";
                }
                else
                {
                    result = new DonHangDAL().CapNhatTrangThaiDonHang(orderID);
                }
            }
            catch
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;

        }
        public BaseResultMOD XoaCN(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                if (id == null || id <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID không hợp lệ";
                }
                else
                {
                    result = new ChucNangDAL().XoaChucNang(id);
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }

            return result;
        }

    }
}

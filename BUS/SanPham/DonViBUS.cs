using COM.DAL;
using COM.ULT;
using COM.MOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.MOD.SanPham;
using COM.DAL.SanPham;

namespace COM.BUS.SanPham
{
    public class DonViBUS
    {
        public BaseResultMOD dsDonVi(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new DonViDAL().getdsDonVi(page); }
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
        public BaseResultMOD ThemDV(DonViMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item.TenDonVi == null || item.TenDonVi == "")
                {
                    result.Status = 0;
                    result.Message = " Tên đơn vị không được để trống";
                }
                else
                {
                    result = new DonViDAL().ThemDonVi(item);
                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD SuaDV(DonViMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item == null || item.DonViTinhID == null || item.DonViTinhID <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID tên đơn vị không hợp lệ";
                }
                else if (item == null || item.TenDonVi == null || item.TenDonVi == "")
                {
                    result.Status = 0;
                    result.Message = "Tên Chức năng không được để trống";

                }
                else
                {
                    result = new DonViDAL().SuaDonVi(item);
                }
            }
            catch
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;

        }
        public BaseResultMOD XoaDV(int id)
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
                    result = new DonViDAL().XoaDonVi(id);
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

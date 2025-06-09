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
    public class GioHangBUS
    {
        public BaseResultMOD dsGioHang(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new GioHangDAL().getdsGioHang(page); }
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
        public BaseResultMOD ThemGH(GioHangMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item == null || item.SanPhamID == null || item.SanPhamID <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID sản phẩm không hợp lệ";
                }
                else if (item == null || item.UserID == null || item.UserID <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID người dùng không hợp lệ";

                }
                else if (item == null || item.GioSoLuong == null)
                {
                    result.Status = 0;
                    result.Message = "Số lượng không hợp lệ";

                }
                else
                {
                    result = new GioHangDAL().ThemGioHang(item);
                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD SuaGH(GioHangMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item == null || item.SanPhamID == null || item.SanPhamID <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID sản phẩm không hợp lệ";
                }
                else if (item == null || item.UserID == null || item.UserID <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID người dùng không hợp lệ";

                }
                else if (item == null || item.GioSoLuong == null || item.GioSoLuong <= 0)
                {
                    result.Status = 0;
                    result.Message = "Số lượng không hợp lệ";

                }
                else
                {
                    result = new GioHangDAL().SuaGioHang(item);
                }
            }
            catch
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;

        }
        public BaseResultMOD XoaGH(int id)
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
                    result = new GioHangDAL().XoaGioHang(id);
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

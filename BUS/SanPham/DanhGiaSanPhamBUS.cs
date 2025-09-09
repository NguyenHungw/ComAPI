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
    public class DanhGiaSanPhamBUS
    {
        public BaseResultMOD dsDanhGiaSanPham(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new DanhGiaSanPhamDAL().getdsDanhGiaSanPham(page); }
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
        public BaseResultMOD ThemDanhGiaSanPhamBUS(DanhGiaSanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
               
                if (item.UserID == null || item.UserID <= 0)
                {
                    result.Status = 0;
                    result.Message = " UserID không được để trống";
                }
                else if (item.SanPhamID == null || item.SanPhamID <= 0)
                {
                    result.Status = 0;
                    result.Message = " SanPhamID không được để trống";
                }
                else if (item.DiemDanhGia == null || item.DiemDanhGia <= 0)
                {
                    result.Status = 0;
                    result.Message = " DiemDanhGia không được để trống";
                }
                else
                {
                    result = new DanhGiaSanPhamDAL().ThemDanhGiaSanPham(item);
                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD SuaDanhGiaSanPhamBUS(DanhGiaSanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item.ID == null || item.ID <= 0)
                {
                    result.Status = 0;
                    result.Message = " ID không được để trống";
                }
                else if (item.UserID == null || item.UserID <= 0)
                {
                    result.Status = 0;
                    result.Message = " UserID không được để trống";
                }
                else if (item.SanPhamID == null || item.SanPhamID <= 0)
                {
                    result.Status = 0;
                    result.Message = " SanPhamID không được để trống";
                }
                else if (item.DiemDanhGia == null || item.DiemDanhGia <= 0)
                {
                    result.Status = 0;
                    result.Message = " DiemDanhGia không được để trống";
                }

                else
                {
                    result = new DanhGiaSanPhamDAL().SuaDanhGiaSanPham(item);
                }
            }
            catch
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;

        }
        public BaseResultMOD XoaDanhGia(int id)
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
                    result = new DanhGiaSanPhamDAL().XoaDanhGiaSanPham(id);
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

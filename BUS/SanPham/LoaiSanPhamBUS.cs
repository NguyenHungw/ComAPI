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
    public class LoaiSanPhamBUS
    {
        public BaseResultMOD dsLoaiSanPham()
        {
            var result = new BaseResultMOD();
            try
            {
                 result = new LoaiSanPhamDAL().getdsLoaiSanPham(); 
              
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
        public BaseResultMOD dsLoaiSanPhamPage(int p, int s)
        {
            var result = new BaseResultMOD();
            try
            {
                if(p>0) { result = new LoaiSanPhamDAL().getdsLoaiSanPhamPage(p, s); }
                else
                {
                    result.Status = 0;
                    result.Message = "Lỗi Page";
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
        public BaseResultMOD ThemLSP(LoaiSanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item.TenLoaiSanPham == null || item.TenLoaiSanPham == "")
                {
                    result.Status = 0;
                    result.Message = " Tên loại sản phẩm không được để trống";
                }
                else
                {
                    result = new LoaiSanPhamDAL().ThemLoaiSanPham(item);
                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD SuaLSP(LoaiSanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item == null || item.LoaiSanPhamID == null || item.LoaiSanPhamID <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID loại sản phẩm không hợp lệ";
                }
                else if (item == null || item.TenLoaiSanPham == null || item.TenLoaiSanPham == "")
                {
                    result.Status = 0;
                    result.Message = "Tên loại sản phẩm không được để trống";

                }
                else if (item == null || item.MoTaLoaiSP == null || item.MoTaLoaiSP == "")
                {
                    result.Status = 0;
                    result.Message = "Tên loại sản phẩm không được để trống";

                }
                else if (item == null || item.TrangThai == null || item.TrangThai < 0)
                {
                    result.Status = 0;
                    result.Message = "Tên loại sản phẩm không được để trống";

                }
                else
                {
                    result = new LoaiSanPhamDAL().SuaLoaiSanPham(item);
                }
            }
            catch
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;

        }
        public BaseResultMOD XoaLoaiSanPham(int id)
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
                    result = new LoaiSanPhamDAL().XoaLoaiSanPham(id);
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

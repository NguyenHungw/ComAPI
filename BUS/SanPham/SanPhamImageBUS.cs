using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.DAL;
using COM.DAL.SanPham;
using COM.MOD;
using COM.MOD.SanPham;
using Microsoft.AspNetCore.Http;

namespace COM.BUS.SanPham
{
    public class SanPhamImageBUS
    {
        public BaseResultMOD dsImageSP(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new SanPhamImageDAL().getdsSanPhamImage(page); }
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


        public BaseResultMOD ThemIMG(List<IFormFile> files, int id)
        {
            var result = new BaseResultMOD();
            try
            {
                //if (item.MSanPham == null || item.MSanPham == "")
                //{
                //    result.Status = 0;
                //    result.Message = " Tên sản phẩm không được để trống";
                //}
                if (id == null || id <= 0)
                {
                    result.Status = 0;
                    result.Message = " ID không được để trống";
                }
                if (files == null)
                {
                    result.Status = 0;
                    result.Message = " Ảnh không được để trống";
                }

                //else if (item.LoaiSanPhamID == null || item.LoaiSanPhamID < 0)
                //{
                //    result.Status = 0;
                //    result.Message = " Tên loại sản phẩm không được để trống";

                //}
                //else if (item.DonViTinhID == null || item.DonViTinhID < 0)
                //{
                //    result.Status = 0;
                //    result.Message = " Tên đơn vị không được để trống";
                //}
                else
                {
                    result = new SanPhamImageDAL().ThemImage(files,id);
                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = ULT.Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD ThemSPAnhVaGia(List<IFormFile> files, SanPhamAnhVaGiaMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                //if (item.MSanPham == null || item.MSanPham == "")
                //{
                //    result.Status = 0;
                //    result.Message = " Tên sản phẩm không được để trống";
                //}
                if (item.TenSanPham == null || item.TenSanPham == "")
                {
                    result.Status = 0;
                    result.Message = " Tên sản phẩm không được để trống";
                }
                else if (item.LoaiSanPhamID == null || item.LoaiSanPhamID < 0)
                {
                    result.Status = 0;
                    result.Message = " Tên loại sản phẩm không được để trống";

                }
                else if (item.DonViTinhID == null || item.DonViTinhID < 0)
                {
                    result.Status = 0;
                    result.Message = " Tên đơn vị không được để trống";
                }
                else
                {
                    if (files == null || files.Count == 0)
                    {
                        return new BaseResultMOD
                        {
                            Status = -1,
                            Message = "Chưa có file đính kèm"
                        };
                    }
                    if (files.Count > 0)
                    {
                        new SanPhamDAL().ThemSanPhamAnhVaGia(files, item);
                        result.Status = 1;
                        result.Message = "Thêm thành công";
                        result.Data = 1;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = ULT.Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD DoiViTriBUS(int id, int currentIndex, int nextIndex)
        {
            var result = new BaseResultMOD();
            try
            {
                if (id == null || id <= 0)
                {
                    result.Status = 0;
                    result.Message = "id không được để trống";
                }
                else if (currentIndex == null )
                {
                    result.Status = 0;
                    result.Message = "currentIndex không được để trống";
                }
                else if (nextIndex == null )
                {
                    result.Status = 0;
                    result.Message = "nextIndex không được để trống";
                }
                else
                {
                    new SanPhamImageDAL().DoiViTriIMG(id,currentIndex,nextIndex);
                    result.Status = 1;
                    result.Message = "Đổi thành công";
                    result.Data = 1;
                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD DoiNhieuViTri(List<ImageOrderUpdateModel> list)
        {
            var result = new BaseResultMOD();
            try
            {
             
                    if (list.Count > 0)
                    {
                        new SanPhamImageDAL().UpdateImageOrder(list);
                        result.Status = 1;
                        result.Message = "Thêm thành công";
                        result.Data = 1;
                    }
                
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD XoaHinhAnh(int id)
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
                    result = new SanPhamImageDAL().XoaSanPhamIMG(id);
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }

            return result;
        }
        public BaseResultMOD SuaSP(SanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item == null || item.MSanPham == null)
                {
                    result.Status = 0;
                    result.Message = item.MSanPham + " không hợp lệ";
                }
                else if (item == null || item.TenSanPham == null || item.TenSanPham == "")
                {
                    result.Status = 0;
                    result.Message = "Tên sản phẩm không được để trống";

                }
                else
                {
                    result = new SanPhamDAL().SuaSanPham(item);
                }
            }
            catch
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;

        }
        public BaseResultMOD XoaSP(int id)
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
                    result = new SanPhamDAL().XoaSanPham(id);
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }

            return result;
        }

    }
}

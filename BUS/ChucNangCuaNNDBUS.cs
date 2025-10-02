﻿using COM.DAL;
using COM.MOD;
using COM.ULT;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace COM.BUS
{
    public class ChucNangCuaNNDBUS
    {
        
        public BaseResultMOD dsCNCuannd(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new ChucNangCuaNNDDAL().getDSChucNangCuaNND(page); }
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
        public BaseResultMOD dsCNCuannd2(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new ChucNangCuaNNDDAL().getDSChucNangCuaNND2(page); }
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
        public BaseResultMOD Them(ThemChucNangCuaNNDMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item == null || item.ChucNang == null || item.ChucNang <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID chức năng không hợp lệ";
                }
                else if (item == null || item.NNDID <= 0 ||item.Xem == null || item.Them == null || item.Sua == null || item.Xoa == null)
                {
                    result.Status = 0;
                    result.Message = "ID nhóm người dùng hoặc giá trị quyền không hợp lệ";

                }
                else
                {
                    result = new ChucNangCuaNNDDAL().ThemQuyenCNCNND(item);
                }

            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD Sua(ChucNangCuaNNDMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item == null || item.ChucNang == null || item.ChucNang <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID chức năng không hợp lệ";
                }
                else if (item == null || item.NNDID <= 0 || item.Xem == null || item.Them == null || item.Sua == null || item.Xoa == null)
                {
                    result.Status = 0;
                    result.Message = "ID nhóm người dùng hoặc giá trị quyền không hợp lệ";

                }
                else
                {
                    result = new ChucNangCuaNNDDAL().SuaCNCNND(item);
                }

            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD Xoa(int id)
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
                    result = new ChucNangCuaNNDDAL().XoaCNCNND(id);
                }
            }
            catch
            {
                result.Status = 0;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD XoaCNcuaNND(int idNND, int idCN)
        {
            var result = new BaseResultMOD();
            try
            {
                if (idNND == null || idNND <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID nhóm người dùng không hợp lệ";
                }
                else if(idCN == null || idCN <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID chức năng không hợp lệ";
                }
                else
                {
                    result = new ChucNangCuaNNDDAL().XoaChucNangCuaNND(idNND,idCN);
                }
            }
            catch
            {
                result.Status = 0;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }

        public ChucNangCuaNNDMOD2 ChiTCNCN(int id )
        {
            var item = new ChucNangCuaNNDMOD2();
            var Result = new BaseResultMOD();
            try
            {
                if (id == null || id <=0)
                {
                    Result.Status = 0;
                    Result.Message = "ID không được để trống";

                }
                else
                {
                    var check = new ChucNangCuaNNDDAL().ChiTietCNCN(id);
                    if (check == null)
                    {
                        Result.Status = 0;
                        Result.Message = "ID không tồn tại";
                    }
                    else
                    {
                        return check;
                    }
                }

            }
            catch (Exception)
            {
                throw;

            }
            return item;
        }

    }

}



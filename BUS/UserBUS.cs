using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.MOD;
using COM.DAL;
using COM.DAL.SanPham;
using Microsoft.AspNetCore.Http.HttpResults;


namespace COM.BUS
{
    public class UserBUS

    {

        public BaseResultMOD DangKy(RegisterUSER item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item.FullName == null || item.FullName == "")
                {
                    Result.Status = 0;
                    Result.Message = "FullName hoặc không được để trống";
                    return Result;
                }
                else if (item.Password == null || item.Password == "")
                {
                    Result.Status = 0;
                    Result.Message = "Password không được để trống";
                    return Result;
                }
                else if (item.Email == null || item.Email == "")
                {
                    Result.Status = 0;
                    Result.Message = "Password không được để trống";
                    return Result;
                }

                else { return new UserDAL().RegisterDAL(item); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception: " + ex.Message);
                throw;
            }
        }
        public BaseResultMOD DanhSachUserBUS(int page, int s)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new UserDAL().DanhSachUser(page, s); }
                else
                {
                    result.Status = 0;
                    result.Message = "lỗi page";
                }
            }
            catch (Exception)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
                result.Data = null;
                throw;
            }
            return result;
        }
        public BaseResultMOD ChiTietUserBUS(int userID)
        {
            var result = new BaseResultMOD();
            try
            {
                if (userID > 0) { result = new UserDAL().ChiTietUser(userID); }
                else
                {
                    result.Status = 0;
                    result.Message = "id không hợp lệ";
                }
            }
            catch (Exception)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
                result.Data = null;
                throw;
            }
            return result;
        }
        public BaseResultMOD SuaUserBUS (UserUpdateMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item.UserID < 0 || item.UserID == null)
                {
                    result.Status = 0;
                    result.Message = "UserID không hợp lệ";
                }
                else
                {
                    result = new UserDAL().SuaUser(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}

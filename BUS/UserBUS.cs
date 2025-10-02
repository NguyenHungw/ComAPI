using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.MOD;
using COM.DAL;


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
       /* public BaseResultMOD DanhSachUserBUS(int page ,int size)
        {
            var Result = new BaseResultMOD();
            try
            {

                else { return new UserDAL().RegisterDAL(item); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception: " + ex.Message);
                throw;
            }
        }*/
    }
}

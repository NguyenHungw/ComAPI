using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCrypt.Net;
using COM.MOD;
using COM.ULT;
using System.Data.SqlTypes;

namespace COM.DAL
{

    public class UserDAL
    {
        public BaseResultMOD RegisterDAL(RegisterUSER item)
        {
            var Result = new BaseResultMOD();
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hash = BCrypt.Net.BCrypt.HashPassword(item.Password, salt);

            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "v1_Users_Register";
                    var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                    

                    // var CurrentTime = DateTime.UtcNow;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO Users (FullName,Email,PasswordHash,CreateAt) VALUES (@FullName,@Email,@PasswordHash,@CreateAt)";
                    cmd.Connection = SQLCon;
                    cmd.Parameters.AddWithValue("@FullName", item.FullName);
                    cmd.Parameters.AddWithValue("@Email", item.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", hash);
                    cmd.Parameters.AddWithValue("@CreateAt", vietnamTime);

                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                }
                Result.Status = 1;
                Result.Message = "Đăng ký thành công ";
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = "Lỗi khi đăng ký: " + ex.Message;

            }
            return Result;
        }
    }

}
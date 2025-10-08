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
using COM.MOD.SanPham;
using Microsoft.AspNetCore.Http;

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

        public BaseResultMOD DanhSachUser(int page, int ProductPerPage)
        {
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<UserMOD> dssp = new List<UserMOD>();
                int totalItems = 0;
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    //count
                    SQLCon.Open();
                    var cmdCount = new SqlCommand();
                    cmdCount.CommandType = CommandType.Text;
                    cmdCount.CommandText = @"SELECT COUNT(*) AS TotalItems FROM [Users];";
                    cmdCount.Connection = SQLCon;
                    totalItems = (int)cmdCount.ExecuteScalar();

                    SqlCommand cmd = new SqlCommand();



                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT distinct  
                                        u.UserID,u.FullName, u.Email, u.isActive,NND.TenNND,u.Phone,u.[Address],u.CreateAt
                                        FROM [Users] u  
                                        LEFT JOIN NguoiDungTrongNhom NDTN ON u.UserID = NDTN.UserID  
                                        LEFT JOIN NhomNguoiDung NND ON NDTN.NNDID = NND.NNDID  
                                        LEFT JOIN ChucNangCuaNhomND CNCNND ON NND.NNDID = CNCNND.NNDID  
                                        LEFT JOIN ChucNang CN ON CNCNND.ChucNangid = CN.ChucNangid";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UserMOD item = new UserMOD();
                        item.UserID = reader.GetInt32(0);
                        item.FullName = reader.IsDBNull(1) ? null : reader.GetString(1);
                        item.Email = reader.IsDBNull(2) ? null : reader.GetString(2);
                        item.isActive = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        item.TenNND = reader.IsDBNull(4) ? null : reader.GetString(4);
                        item.Phone = reader.IsDBNull(5) ? null : reader.GetString(5);
                        item.Address = reader.IsDBNull(6) ? null : reader.GetString(6);
                        item.CreateAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7);
                        dssp.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dssp;
                    result.TotalRow = totalItems;
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = "Lỗi hệ thống" + ex;
                throw;
            }
            return result;

        }
        public BaseResultMOD SuaUser(UserUpdateMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = SQLCon;
                    cmd.CommandText = "Update [Users] set FullName=@FullName,Email=@Email,Phone=@Phone,[Address]=@Address,isActive=@isActive where UserID=@UserID";
                    cmd.Parameters.AddWithValue("@UserID", item.UserID);
                  
                    cmd.Parameters.AddWithValue("@Phone", (object?)item.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", (object?)item.Address ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FullName", (object?)item.FullName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object?)item.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@isActive", (object?)item.isActive ?? DBNull.Value);


                    cmd.ExecuteNonQuery();
                   
                }
                if (item.IDNND != null)
                {
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        SQLCon.Open();
                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = SQLCon;
                        cmd2.CommandText = "Update NguoiDungTrongNhom set NNDID=@NNDID where UserID=@UserID";
                        cmd2.Parameters.AddWithValue("@NNDID", (object?)item.IDNND ?? DBNull.Value);
                        cmd2.Parameters.AddWithValue("@UserID", item.UserID);

                        cmd2.ExecuteNonQuery();
                    }
                }

                result.Status = 1;
                result.Message = "Sửa thành công";
                result.Data = 1;
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = "Lỗi hệ thống: " + ex.Message; // in ra để debug dễ hơn
            }
            return result;
        }
    }
 
}



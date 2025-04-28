using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.MOD;
using Microsoft.Extensions.Configuration;

namespace COM.DAL
{
    public class ExternalLoginDAL
    {
        private readonly string _connectionString;

        public ExternalLoginDAL(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public int? GetUserIdByExternalLogin(string provider, string providerKey)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT UserID FROM ExternalLogins WHERE Provider = @Provider AND ProviderKey = @ProviderKey";
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Provider", provider);
            cmd.Parameters.AddWithValue("@ProviderKey", providerKey);

            var result = cmd.ExecuteScalar();
            return result == null ? null : Convert.ToInt32(result);
        }

        public int CreateUserAndExternalLogin(FacebookUserMOD user, string provider, string providerKey)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            using var trans = conn.BeginTransaction();
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

            int userId = 0;

            // 👉 1. Kiểm tra xem Email đã tồn tại chưa
            string sqlCheckUser = "SELECT UserID FROM Users WHERE Email = @Email";
            using (SqlCommand cmdCheck = new SqlCommand(sqlCheckUser, conn, trans))
            {
                cmdCheck.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                var result = cmdCheck.ExecuteScalar();
                if (result != null)
                {
                    // Nếu đã có user thì lấy UserID ra
                    userId = Convert.ToInt32(result);
                }
                else
                {
                    // 👉 2. Nếu chưa có thì thêm user mới
                    string sqlUser = "INSERT INTO Users (Email, CreateAt, isActive) OUTPUT INSERTED.UserID VALUES (@Email, @CreateAt, @isActive)";
                    using SqlCommand cmdUser = new SqlCommand(sqlUser, conn, trans);
                    cmdUser.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    cmdUser.Parameters.AddWithValue("@CreateAt", vietnamTime);
                    cmdUser.Parameters.AddWithValue("@isActive", 1);

                    userId = (int)cmdUser.ExecuteScalar();
                }
            }

            // 👉 3. Thêm ExternalLogin (nếu cần)
            string sqlLogin = @"INSERT INTO ExternalLogins (UserID, Provider, ProviderKey, Email)
                        VALUES (@UserID, @Provider, @ProviderKey, @Email)";
            using (SqlCommand cmdLogin = new SqlCommand(sqlLogin, conn, trans))
            {
                cmdLogin.Parameters.AddWithValue("@UserID", userId);
                cmdLogin.Parameters.AddWithValue("@Provider", provider);
                cmdLogin.Parameters.AddWithValue("@ProviderKey", providerKey);
                cmdLogin.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);

                cmdLogin.ExecuteNonQuery();
            }

            trans.Commit();
            return userId;
        }

        public int CreateUserAndExternalLoginGoogle(GoogleUserMOD user, string provider, string providerKey)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            using var trans = conn.BeginTransaction();
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
            int userId = 0;
            string sqlCheckUser = "SELECT UserID FROM Users WHERE Email = @Email";
            using (SqlCommand cmdCheck = new SqlCommand(sqlCheckUser, conn, trans))
            {
                cmdCheck.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                var result = cmdCheck.ExecuteScalar();
                if (result != null)
                {
                    // Nếu đã có user thì lấy UserID ra
                    userId = Convert.ToInt32(result);
                }
                else
                {
                    // 1. Thêm user
                    // output là sau khi chèn thành công thì trả ngược lại giá trị vừa được tạo ở userid
                    string sqlUser = "INSERT INTO Users (Email,CreateAt,isActive) OUTPUT INSERTED.UserID VALUES (@Email,@CreateAt,@isActive)";
                    using SqlCommand cmdUser = new SqlCommand(sqlUser, conn, trans);
                    //cmdUser.Parameters.AddWithValue("@Name", user.Name);
                    cmdUser.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    cmdUser.Parameters.AddWithValue("@CreateAt", vietnamTime);
                    cmdUser.Parameters.AddWithValue("@isActive", 1);

                    userId = (int)cmdUser.ExecuteScalar();
                }



                // 2. Thêm ExternalLogin
                string sqlLogin = @"INSERT INTO ExternalLogins (UserID, Provider, ProviderKey, Email)
                            VALUES (@UserID, @Provider, @ProviderKey, @Email)";
                using SqlCommand cmdLogin = new SqlCommand(sqlLogin, conn, trans);
                cmdLogin.Parameters.AddWithValue("@UserID", userId);
                cmdLogin.Parameters.AddWithValue("@Provider", provider);
                cmdLogin.Parameters.AddWithValue("@ProviderKey", providerKey);
                cmdLogin.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);

                cmdLogin.ExecuteNonQuery();
                trans.Commit();

                return userId;
            }
            
        }
        
    }
    
}

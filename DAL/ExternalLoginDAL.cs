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

            // 1. Thêm user
            string sqlUser = "INSERT INTO Users (Email) OUTPUT INSERTED.UserID VALUES (@Email)";
            using SqlCommand cmdUser = new SqlCommand(sqlUser, conn, trans);
            //cmdUser.Parameters.AddWithValue("@Name", user.Name);
            cmdUser.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);

            int userId = (int)cmdUser.ExecuteScalar();

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

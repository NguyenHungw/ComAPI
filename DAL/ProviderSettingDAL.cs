using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.MOD;

namespace COM.DAL
{
    public class ProviderSettingDAL
    {
        private readonly string _connectionString;

        public ProviderSettingDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Lấy theo ProviderName
        public ProviderSettingMOD GetByProvider(string providerName)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM ApiLoginSecrets WHERE Provider = @Provider";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Provider", providerName);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ProviderSettingMOD
                {
                    Id = (int)reader["Id"],
                    ProviderName = (string)reader["Provider"],
                    ClientId = (string)reader["AppId"],
                    ClientSecret = (string)reader["AppSecret"],
                    CreateAt = (DateTime)reader["CreatedAt"]
                };
            }
            return null;
        }

        // Thêm mới
        public void Insert(ProviderSettingMOD item)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"INSERT INTO ApiLoginSecrets (Provider, AppId, AppSecret)
                       VALUES (@Provider, @AppId, @AppSecret)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Provider", item.ProviderName);
            cmd.Parameters.AddWithValue("@AppId", item.ClientId);
            cmd.Parameters.AddWithValue("@AppSecret", item.ClientSecret);

            cmd.ExecuteNonQuery();
        }

        // Cập nhật
        public void Update(ProviderSettingMOD item)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"UPDATE ApiLoginSecrets 
                       SET AppId = @AppId, AppSecret = @AppSecret
                       WHERE Provider = @Provider";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Provider", item.ProviderName);
            cmd.Parameters.AddWithValue("@AppId", item.ClientId);
            cmd.Parameters.AddWithValue("@AppSecret", item.ClientSecret);

            cmd.ExecuteNonQuery();
        }

        // Xóa
        public void Delete(string providerName)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"DELETE FROM ApiLoginSecrets WHERE Provider = @Provider";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Provider", providerName);

            cmd.ExecuteNonQuery();
        }
    }

}

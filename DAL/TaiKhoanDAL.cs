using COM.MOD;
using COM.MOD.Jwt;
using COM.ULT;
using System.Data.SqlClient;
using System.Security.Claims;

public class TaiKhoanDAL
{
    public static jwtmod DangNhap(TaiKhoanMOD item, out int userID ,out List<Claim> claims, out string role, out bool isAuthenticated)
    {
        claims = new List<Claim>();
        role = string.Empty;
        isAuthenticated = false;
        userID = 0;

        var jwtitem = new jwtmod();

        using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
        {
            SQLCon.Open();

            string sqlQuery = @"SELECT u.UserID, u.Email, u.PasswordHash,u.isActive, 
                                       NND.TenNND, CN.TenChucNang, CNCNND.Xem, CNCNND.Them, CNCNND.Sua, CNCNND.Xoa
                                FROM [Users] u
                                INNER JOIN NguoiDungTrongNhom NDTN ON u.UserID = NDTN.UserID
                                INNER JOIN NhomNguoiDung NND ON NDTN.NNDID = NND.NNDID
                                INNER JOIN ChucNangCuaNhomND CNCNND ON NND.NNDID = CNCNND.NNDID
                                INNER JOIN ChucNang CN ON CNCNND.ChucNangid = CN.ChucNangid
                                WHERE u.Email = @Email";

            using (SqlCommand cmd = new SqlCommand(sqlQuery, SQLCon))
            {
                cmd.Parameters.AddWithValue("@Email", item.Email);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string hashedPassword = reader.GetString(reader.GetOrdinal("PasswordHash"));

                        if (BCrypt.Net.BCrypt.Verify(item.Password, hashedPassword))
                        {
                            jwtitem.ID = reader.GetInt32(reader.GetOrdinal("UserID"));

                            jwtitem.Email = reader.GetString(reader.GetOrdinal("Email"));

                            int isActive = reader.GetInt32(reader.GetOrdinal("isActive"));
                            string tenChucNang = reader.IsDBNull(reader.GetOrdinal("TenChucNang")) ? null : reader.GetString(reader.GetOrdinal("TenChucNang"));

                            if (isActive == 1)
                            {
                                userID=jwtitem.ID;
                                isAuthenticated = true;
                                role = reader.GetString(reader.GetOrdinal("TenNND"));
                                string quyen = "";

                                if (reader.GetBoolean(reader.GetOrdinal("Xem"))) quyen += "Xem,";
                                if (reader.GetBoolean(reader.GetOrdinal("Them"))) quyen += "Them,";
                                if (reader.GetBoolean(reader.GetOrdinal("Sua"))) quyen += "Sua,";
                                if (reader.GetBoolean(reader.GetOrdinal("Xoa"))) quyen += "Xoa,";

                                quyen = quyen.TrimEnd(',');
                                if (!string.IsNullOrEmpty(tenChucNang))
                                {
                                    string chucNangVaQuyen = $"{tenChucNang}:{quyen}";
                                    claims.Add(new Claim("CN", chucNangVaQuyen));
                                }
                            }
                        }
                    }
                }
            }
        }

        return jwtitem;
    }

    public static jwtmod DangNhapFB(string email, out int userID ,out List<Claim> claims, out string role, out bool isAuthenticated)
    {
        claims = new List<Claim>();
        role = string.Empty;
        isAuthenticated = false;
        userID = 0;
        var jwtitem = new jwtmod();

        using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
        {
            SQLCon.Open();

            string sqlQuery = @"SELECT u.UserID, u.Email, u.PasswordHash,u.isActive, 
                                       NND.TenNND, CN.TenChucNang, CNCNND.Xem, CNCNND.Them, CNCNND.Sua, CNCNND.Xoa
                                FROM [Users] u
                                LEFT JOIN NguoiDungTrongNhom NDTN ON u.UserID = NDTN.UserID
                                LEFT JOIN NhomNguoiDung NND ON NDTN.NNDID = NND.NNDID
                                LEFT JOIN ChucNangCuaNhomND CNCNND ON NND.NNDID = CNCNND.NNDID
                                LEFT JOIN ChucNang CN ON CNCNND.ChucNangid = CN.ChucNangid
                                WHERE u.Email = @Email";

            using (SqlCommand cmd = new SqlCommand(sqlQuery, SQLCon))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //string hashedPassword = reader.GetString(reader.GetOrdinal("PasswordHash"));

                        if (!string.IsNullOrEmpty(email))
                        {
                            jwtitem.ID = reader.GetInt32(reader.GetOrdinal("UserID"));
                            

                            jwtitem.Email = reader.GetString(reader.GetOrdinal("Email"));

                            int isActive = reader.GetInt32(reader.GetOrdinal("isActive"));
                            string tenChucNang = reader.IsDBNull(reader.GetOrdinal("TenChucNang")) ? null : reader.GetString(reader.GetOrdinal("TenChucNang"));

                            if (isActive == 1)
                            {
                                isAuthenticated = true;
                                int ordinalTenNND = reader.GetOrdinal("TenNND");
                                role = reader.IsDBNull(ordinalTenNND) ? "" : reader.GetString(ordinalTenNND);
                                //role = reader.GetString(reader.GetOrdinal("TenNND"));
                                string quyen = "";

                                //if (reader.GetBoolean(reader.GetOrdinal("Xem"))) quyen += "Xem,";
                                //if (reader.GetBoolean(reader.GetOrdinal("Them"))) quyen += "Them,";
                                //if (reader.GetBoolean(reader.GetOrdinal("Sua"))) quyen += "Sua,";
                                //if (reader.GetBoolean(reader.GetOrdinal("Xoa"))) quyen += "Xoa,";
                                int xemOrdinal = reader.GetOrdinal("Xem");
                                int themOrdinal = reader.GetOrdinal("Them");
                                int suaOrdinal = reader.GetOrdinal("Sua");
                                int xoaOrdinal = reader.GetOrdinal("Xoa");

                                if (!reader.IsDBNull(xemOrdinal) && reader.GetBoolean(xemOrdinal)) quyen += "Xem,";
                                if (!reader.IsDBNull(themOrdinal) && reader.GetBoolean(themOrdinal)) quyen += "Them,";
                                if (!reader.IsDBNull(suaOrdinal) && reader.GetBoolean(suaOrdinal)) quyen += "Sua,";
                                if (!reader.IsDBNull(xoaOrdinal) && reader.GetBoolean(xoaOrdinal)) quyen += "Xoa,";

                                quyen = quyen.TrimEnd(',');
                                if (!string.IsNullOrEmpty(tenChucNang))
                                {
                                    string chucNangVaQuyen = $"{tenChucNang}:{quyen}";
                                    claims.Add(new Claim("CN", chucNangVaQuyen));
                                }
                            }
                        }
                    }
                }
            }
        }

        return jwtitem;
    }

    public static void LuuRefreshToken(int userId, string refreshToken)
    {
        using var connection = new SqlConnection(SQLHelper.appConnectionStrings);
        connection.Open();

        string sqlDelete = "DELETE FROM RefreshTokens WHERE UserId = @UserId";
        using (var deleteCmd = new SqlCommand(sqlDelete, connection))
        {
            deleteCmd.Parameters.AddWithValue("@UserId", userId);
            deleteCmd.ExecuteNonQuery();
        }

        string sqlInsert = "INSERT INTO RefreshTokens (UserId, TokenValue, ExpirationDate) VALUES (@UserId, @TokenValue, @ExpirationDate)";
        using (var insertCmd = new SqlCommand(sqlInsert, connection))
        {
            insertCmd.Parameters.AddWithValue("@UserId", userId);
            insertCmd.Parameters.AddWithValue("@TokenValue", refreshToken);
            insertCmd.Parameters.AddWithValue("@ExpirationDate", DateTime.UtcNow.AddDays(7));
            insertCmd.ExecuteNonQuery();
        }
    }
}

using COM.MOD;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using COM.Services;
using Microsoft.Extensions.Configuration;
using COM.MOD.Jwt;
using COM.ULT;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;
namespace COM.Services
{
    public class AuthService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public AuthService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
        }

        public (string jwtToken, string refreshToken) GenerateJwtAndRefreshToken(TaiKhoanMOD item,int userID, string userRole, List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            //var ThoiGianHetHan = DateTime.Now.AddMinutes(10);
            var ThoiGianHetHan = DateTime.Now.AddSeconds(10);


            var additionalClaims = new List<Claim>
            {
                
              /*  new Claim("PhoneNumber", item.PhoneNumber),
                new Claim("NhomNguoiDung", userRole),
                new Claim("ThoiHanDangNhap", ThoiGianHetHan.ToString(), ClaimValueTypes.Integer),*/
            
            };
            bool idUser = additionalClaims.Any(claim => claim.Type == "UserID");
            bool phoneEmailClaimExists = additionalClaims.Any(claim => claim.Type == "Email");
            bool NhomNguoiDungClaimExists = additionalClaims.Any(claim => claim.Type == "NhomNguoiDung");
            bool ThoiHanDangNhapClaimExists = additionalClaims.Any(claim => claim.Type == "ThoiHanDangNhap");
            string userIDString = userID.ToString();
            if (!idUser)
            {
                // Nếu claim "UserID" chưa tồn tại, thêm nó vào danh sách
                //additionalClaims.Add(new Claim("UserID", item..ToString()));
                additionalClaims.Add(new Claim("ID", userIDString));
            }


            if (!phoneEmailClaimExists) 
            {
                // Nếu claim "PhoneNumber" chưa tồn tại, thêm nó vào danh sách
                additionalClaims.Add(new Claim("Email", item.Email));
            }
            if (!NhomNguoiDungClaimExists)
            {
                additionalClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            if (!ThoiHanDangNhapClaimExists)
            {
                additionalClaims.Add(new Claim("ThoiHanDangNhap", ThoiGianHetHan.ToString(), ClaimValueTypes.Integer));
            }

            var refreshToken = Guid.NewGuid().ToString();
            additionalClaims.Add(new Claim("RefreshToken", refreshToken));
            // Chèn claim vào phía trước danh sách claim hiện tại
            claims.InsertRange(0, additionalClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = ThoiGianHetHan,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _issuer,
                Audience = _audience
            };

            // Tạo JWT Token
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtTokenString = tokenHandler.WriteToken(jwtToken);

            return (jwtTokenString, refreshToken);
        }

        public ( string jwtToken, string refreshToken) GenerateJwtAndRefreshToken2( string userRole, List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var ThoiGianHetHan = DateTime.Now.AddMinutes(10);

            var additionalClaims = new List<Claim>(); // Khởi tạo danh sách Claims

            additionalClaims.Add(new Claim("ThoiHanDangNhap", ThoiGianHetHan.ToString(), ClaimValueTypes.Integer));

            // Xóa tất cả các claim có loại "ThoiHanDangNhap" từ danh sách claims
            var claimsToRemove = claims.Where(claim => claim.Type == "ThoiHanDangNhap").ToList();
            foreach (var claim in claimsToRemove)
            {
                claims.Remove(claim);
            }

            // Thêm các Claim mới vào danh sách claims
            claims.AddRange(additionalClaims);

            var refreshToken = Guid.NewGuid().ToString();
            

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = ThoiGianHetHan,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _issuer,
                Audience = _audience
            };

            // Tạo JWT Token
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtTokenString = tokenHandler.WriteToken(jwtToken);

            return (jwtTokenString, refreshToken);
        }
        public ( string jwtToken, string refreshToken) GenerateJwtAndRefreshTokenNoID(List<Claim> claims,int userID,string email,string role)
        {
          
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            //var ThoiGianHetHan = DateTime.Now.AddMinutes(10);
            var ThoiGianHetHan = DateTime.Now.AddSeconds(10);

            var additionalClaims = new List<Claim>(); // Khởi tạo danh sách Claims
            bool idUser = additionalClaims.Any(claim => claim.Type == "UserID");
            bool phoneEmailClaimExists = additionalClaims.Any(claim => claim.Type == "Email");
            string userIDString = userID.ToString();
            bool NhomNguoiDungClaimExists = additionalClaims.Any(claim => claim.Type == "NhomNguoiDung");

            if (!idUser)
            {
                additionalClaims.Add(new Claim("ID", userIDString));
            }
            if (!phoneEmailClaimExists)
            {
                // Nếu claim "PhoneNumber" chưa tồn tại, thêm nó vào danh sách
                additionalClaims.Add(new Claim("Email", email));
            }
            if (!NhomNguoiDungClaimExists)
            {
                additionalClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            additionalClaims.Add(new Claim("ThoiHanDangNhap", ThoiGianHetHan.ToString(), ClaimValueTypes.Integer));

            // Xóa tất cả các claim có loại "ThoiHanDangNhap" từ danh sách claims
            //var claimsToRemove = claims.Where(claim => claim.Type == "ThoiHanDangNhap").ToList();
            //foreach (var claim in claimsToRemove)
            //{
            //    claims.Remove(claim);
            //}

            // Thêm các Claim mới vào danh sách claims
            //claims.AddRange(additionalClaims);

            var refreshToken = Guid.NewGuid().ToString();
            additionalClaims.Add(new Claim("RefreshToken", refreshToken));
            claims.InsertRange(0, additionalClaims);



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = ThoiGianHetHan,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _issuer,
                Audience = _audience
            };

            // Tạo JWT Token
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtTokenString = tokenHandler.WriteToken(jwtToken);

            return (jwtTokenString, refreshToken);
        }

        public (string jwtToken, string refreshToken) GenerateJwtAndRefreshTokenFB(string email,int userID, string userRole, List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var ThoiGianHetHan = DateTime.Now.AddMinutes(10);

            var additionalClaims = new List<Claim>
            {

                /*  new Claim("PhoneNumber", item.PhoneNumber),
                  new Claim("NhomNguoiDung", userRole),
                  new Claim("ThoiHanDangNhap", ThoiGianHetHan.ToString(), ClaimValueTypes.Integer),*/

            };
            bool idUser = additionalClaims.Any(claim => claim.Type == "UserID");
            bool phoneEmailClaimExists = additionalClaims.Any(claim => claim.Type == "Email");
            bool NhomNguoiDungClaimExists = additionalClaims.Any(claim => claim.Type == "NhomNguoiDung");
            bool ThoiHanDangNhapClaimExists = additionalClaims.Any(claim => claim.Type == "ThoiHanDangNhap");
            string userIDString = userID.ToString();

            if (!idUser)
            {
                // Nếu claim "UserID" chưa tồn tại, thêm nó vào danh sách
                //additionalClaims.Add(new Claim("UserID", item..ToString()));
                additionalClaims.Add(new Claim("ID", userIDString));
            }
            if (!phoneEmailClaimExists)
            {
                // Nếu claim "PhoneNumber" chưa tồn tại, thêm nó vào danh sách
                additionalClaims.Add(new Claim("Email", email));
            }
            if (!NhomNguoiDungClaimExists)
            {
                additionalClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            if (!NhomNguoiDungClaimExists)
            {
                additionalClaims.Add(new Claim("ThoiHanDangNhap", ThoiGianHetHan.ToString(), ClaimValueTypes.Integer));
            }

            var refreshToken = Guid.NewGuid().ToString();
            additionalClaims.Add(new Claim("RefreshToken", refreshToken));
            // Chèn claim vào phía trước danh sách claim hiện tại
            claims.InsertRange(0, additionalClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = ThoiGianHetHan,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _issuer,
                Audience = _audience
            };

            // Tạo JWT Token
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtTokenString = tokenHandler.WriteToken(jwtToken);

            return (jwtTokenString, refreshToken);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // Thay đổi tùy theo cấu hình
                ValidateIssuer = false, // Thay đổi tùy theo cấu hình
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey)),
                ValidateLifetime = false // Chú ý rằng AccessToken đã hết hạn
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                var claims = principal.Claims.ToList();
                return principal;
            }
            catch
            {
                return null;
            }
        }
        public RefreshToken GetRefreshTokenFromDatabase(string userId, string refreshToken)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();

                string sqlQuery = "SELECT * FROM RefreshTokens WHERE UserID = @UserId AND TokenValue = @TokenValue";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, SQLCon))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@TokenValue", refreshToken);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var dbRefreshToken = new RefreshToken

                            {
                                UserId = userId,
                                TokenValue = reader.GetString(reader.GetOrdinal("TokenValue")),
                                ExpirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"))
                            };
                            return dbRefreshToken;
                        }
                    }
                }
            }
            return null;
        }
        public RefreshToken GetRefreshTokenFromDatabaseNoID( string refreshToken)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();

                string sqlQuery = "SELECT * FROM RefreshTokens WHERE TokenValue = @TokenValue";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, SQLCon))
                {
            
                    cmd.Parameters.AddWithValue("@TokenValue", refreshToken);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var dbRefreshToken = new RefreshToken

                            {
                                //UserId = userId,
                                TokenValue = reader.GetString(reader.GetOrdinal("TokenValue")),
                                ExpirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"))
                            };
                            return dbRefreshToken;
                        }
                    }
                }
            }
            return null;
        }

        public void UpdateRefreshTokenInDatabase(string userId, string oldRefreshToken, string newRefreshToken)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();

                string sqlQuery = "UPDATE RefreshTokens SET TokenValue = @NewRefreshToken WHERE UserId = @UserId AND TokenValue = @OldRefreshToken";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, SQLCon))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@OldRefreshToken", oldRefreshToken);
                    cmd.Parameters.AddWithValue("@NewRefreshToken", newRefreshToken);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateRefreshTokenInDatabaseNoID( string oldRefreshToken, string newRefreshToken)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();

                string sqlQuery = "UPDATE RefreshTokens SET TokenValue = @NewRefreshToken WHERE TokenValue = @OldRefreshToken";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, SQLCon))
                {
                    //cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@OldRefreshToken", oldRefreshToken);
                    cmd.Parameters.AddWithValue("@NewRefreshToken", newRefreshToken);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public TokenInfo GetUserInfoFromRefreshToken(string refreshToken, out List<Claim> claims)
        {
            claims = new List<Claim>();

            //userID = 0;
            //role = string.Empty;
            //email = string.Empty;
            //isAuthenticated = false;

            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();

                // 1. Lấy UserID từ bảng RefreshTokens  
                string getUserIdQuery = "SELECT UserID FROM RefreshTokens WHERE TokenValue = @ref";

                int? userId = null;

                using (SqlCommand cmd = new SqlCommand(getUserIdQuery, SQLCon))
                {
                    cmd.Parameters.AddWithValue("@ref", refreshToken);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                    //else
                    //{
                    //    Console.WriteLine("Refresh token không hợp lệ.");
                    //    return new me;//Return null if the refresh token is invalid.  
                    //}
                }

                // 2. Dùng UserID để truy vấn thông tin chi tiết  
                string userDetailQuery = @"  
               SELECT   
                   u.UserID, u.Email, u.isActive,   
                   NND.TenNND, CN.TenChucNang, CNCNND.Xem, CNCNND.Them, CNCNND.Sua, CNCNND.Xoa  
               FROM [Users] u  
               INNER JOIN NguoiDungTrongNhom NDTN ON u.UserID = NDTN.UserID  
               INNER JOIN NhomNguoiDung NND ON NDTN.NNDID = NND.NNDID  
               INNER JOIN ChucNangCuaNhomND CNCNND ON NND.NNDID = CNCNND.NNDID  
               INNER JOIN ChucNang CN ON CNCNND.ChucNangid = CN.ChucNangid  
               WHERE u.UserID = @UserId";

                var userInfo = new TokenInfo();
                using (SqlCommand detailCmd = new SqlCommand(userDetailQuery, SQLCon))
                {
                    detailCmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = detailCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {   
                            do
                            {

                                userInfo.UserID = reader.GetInt32(reader.GetOrdinal("UserID"));

                                userInfo.Email = reader.GetString(reader.GetOrdinal("Email"));

                                int isActive = reader.GetInt32(reader.GetOrdinal("isActive"));
                                string tenChucNang = reader.IsDBNull(reader.GetOrdinal("TenChucNang")) ? null : reader.GetString(reader.GetOrdinal("TenChucNang"));

                                if (isActive == 1)
                                {
                                    
                                    userInfo.Role = reader.GetString(reader.GetOrdinal("TenNND"));
                                  
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
                            
                               
                            } while (reader.Read());
                        }
                    }
                }

                return userInfo;


            }
        }



    }
}

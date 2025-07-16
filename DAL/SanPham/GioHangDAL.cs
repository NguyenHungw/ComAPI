using COM.MOD;
using COM.MOD.SanPham;
using COM.MOD.SanPham;
using COM.ULT;
//using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.DAL.SanPham
{
    public class GioHangDAL
    {
        // private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        public BaseResultMOD getdsGioHang(int page)
        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<GioHangMOD> dscn = new List<GioHangMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from GioHang ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GioHangMOD item = new GioHangMOD();
                        item.ID = reader.GetInt32(0);
                        item.SanPhamID = reader.GetInt32(1);
                        item.UserID = reader.GetInt32(2);
                        item.GioSoLuong = reader.GetInt32(3);

                        dscn.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dscn;
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
        public BaseResultMOD getGioHangUserThanhToan(int UserID)
        {
            //const int ProductPerPage = 10;
            //int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<GioHangUserThanhToanMOD> ghUser = new List<GioHangUserThanhToanMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @";
                   SELECT sp.ID,sp.MSanPham,sp.TenSanPham,SPI.FilePath,gh.GioSoLuong,GBSP.GiaBan,GBSP.SalePercent,GBSP.GiaSauGiam
                    FROM GioHang gh
                    LEFT JOIN SanPham sp ON gh.SanPhamID = sp.ID
                    LEFT JOIN GiaBanSanPham GBSP ON sp.ID = GBSP.SanPhamID
                    LEFT JOIN SanPhamImage SPI ON sp.ID = SPI.SanPhamID AND IndexOrder=0
                    WHERE gh.UserID = @UserID;";
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GioHangUserThanhToanMOD item = new GioHangUserThanhToanMOD();
                        item.SanPhamID = reader.GetInt32(0);
                        item.MSanPham = reader.GetString(1);
                        item.TenSanPham = reader.GetString(2);
                        item.HinhAnh = reader.IsDBNull(3) ? null : reader.GetString(3);
                        item.GioSoLuong = reader.GetInt32(4);
                        item.DonGia = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5);
                        item.TrietKhau = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6); 
                        item.GiaSauGiam = reader.GetDecimal(7);
                        item.TongTien = item.GioSoLuong * item.GiaSauGiam;
                        ghUser.Add(item);
                    }
                    reader.Close();
                    //decimal tongTatca = ghUser.Sum(x => x.TongTien ?? 0);

                    result.Status = 1;
                    result.Data = ghUser;
                    //result.Data = new
                    //{
                    //    DanhSachGioHang = ghUser,
                    //    TongTatCa = tongTatca
                    //};
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
        public BaseResultMOD getGioHangUserChiTiet(int UserID)
        {
            //const int ProductPerPage = 10;
            //int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<GioHangMOD> dscn = new List<GioHangMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from GioHang where UserID = @UserID ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        GioHangMOD item = new GioHangMOD();
                        item.ID = reader.GetInt32(0);
                        item.SanPhamID = reader.GetInt32(1);
                        item.UserID = reader.GetInt32(2);
                        item.GioSoLuong = reader.GetInt32(3);

                        dscn.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dscn;
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

        public BaseResultMOD ThemGioHang(GioHangMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                // Kiểm tra trước khi thêm chức năng
                bool isDuplicate = KiemTraTrungSanPham((int)item.SanPhamID);
                if (isDuplicate)
                {
                    //sp ton tai
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        int soLuongCanCong = item.GioSoLuong.HasValue && item.GioSoLuong.Value > 0 ? item.GioSoLuong.Value : 1;
                                                               // điều kiện                       //     đúng              // sai                     

                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Update GioHang set GioSoLuong = GioSoLuong + @SoLuong where SanPhamID=@SanPhamID AND UserID = @UserID ";
                        cmd.Parameters.AddWithValue("@SoLuong", soLuongCanCong);
                        cmd.Parameters.AddWithValue("@SanPhamID", item.SanPhamID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserID", item.UserID ?? (object)DBNull.Value);



                        cmd.Connection = SQLCon;
                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm thành công";
                        result.Data = 1;
                    }
                }
                else
                {
                    // Thêm chức năng vào cơ sở dữ liệu
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into GioHang (SanPhamID,UserID,GioSoLuong) VALUES(@SanPhamID,@UserID,@GioSoLuong)";
                        cmd.Parameters.AddWithValue("@SanPhamID", item.SanPhamID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserID", item.UserID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@GioSoLuong", item.GioSoLuong ?? (object)DBNull.Value);



                        cmd.Connection = SQLCon;
                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm thành công";
                        result.Data = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }

        // Hàm để kiểm tra trùng chức năng
        private bool KiemTraTrungSanPham(int id)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();
                string checkQuery = "SELECT COUNT(*) FROM GioHang WHERE SanPhamID = @SanPhamID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@SanPhamID", id);
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }

        public BaseResultMOD SuaGioHang(GioHangMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [GioHang] set SanPhamID =@SanPhamID,UserID =@UserID,GioSoLuong =@GioSoLuong where ID =@ID ";
                    cmd.Parameters.AddWithValue("@ID", item.ID);
                    cmd.Parameters.AddWithValue("@SanPhamID", item.SanPhamID);
                    cmd.Parameters.AddWithValue("@UserID", item.UserID);
                    cmd.Parameters.AddWithValue("@GioSoLuong", item.GioSoLuong);

                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Sửa giỏ hàng thành công";
                    result.Data = 1;
                }

            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD XoaGioHang(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from GioHang where UserID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", id);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if (rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa thành công";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = "{id} không hợp lệ";

                    }
                }

            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }

    }
}

using COM.MOD;
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
    public class DonHangDAL
    {
        // private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        public BaseResultMOD getdsDonHang(int page)
        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<DonHangMOD> dscn = new List<DonHangMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from DonHang ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DonHangMOD item = new DonHangMOD();
                        item.ID = reader.GetInt32(0);
                        item.OrderID = reader.GetString(1);
                        item.UserID = reader.GetInt32(2);
                        item.PhuongThucThanhToan = reader.GetString(3);
                        item.NgayMua = reader.GetDateTime(4);
                        item.Status = reader.GetInt32(5);

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
        public BaseResultMOD ThemDonHang(DonHangMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                // Kiểm tra trước khi thêm chức năng
                bool isDuplicate = KiemTraTrungChucNang(item.OrderID);
                if (isDuplicate)
                {
                    result.Status = -1;
                    result.Message = "Tên đơn hàng đã tồn tại.";
                }
                else
                {
                    // Thêm chức năng vào cơ sở dữ liệu
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into DonHang (OrderID,UserID,PhuongThucThanhToan,NgayMua,Status) VALUES(@OrderID,@UserID,@PhuongThucThanhToan,@NgayMua,@Status)";
                        cmd.Parameters.AddWithValue("@OrderID", item.OrderID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserID", item.UserID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhuongThucThanhToan", item.PhuongThucThanhToan ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NgayMua", item.NgayMua ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", item.Status ?? (object)DBNull.Value);
                        cmd.Connection = SQLCon;
                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm đơn hàng thành công";
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
        private bool KiemTraTrungChucNang(string OrderID)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();
                string checkQuery = "SELECT COUNT(*) FROM DonHang WHERE OrderID = @OrderID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@OrderID", OrderID);
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }

        public BaseResultMOD SuaDonHang(string OrderID)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [DonHang] set Status =@Status where OrderID =@OrderID ";
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    //cmd.Parameters.AddWithValue("@Mota", item.Mota);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Cập nhật thành công";
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
        public BaseResultMOD CapNhatTrangThaiDonHang(string OrderID)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [DonHang] set Status =@Status where OrderID =@OrderID ";
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.Parameters.AddWithValue("@Status", 1);
                    //cmd.Parameters.AddWithValue("@Mota", item.Mota);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Cập nhật thành công";
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
        public BaseResultMOD XoaDonHang(int OrderID)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from DonHang where OrderID = @OrderID";
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if (rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa đơn hàng thành công";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = "OrderID không hợp lệ";
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

        //chi tiết đơn hàng

        public BaseResultMOD getdsChiTietDonHang(int page)
        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<DonHangMOD> dscn = new List<DonHangMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from ChiTietDonHang ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DonHangMOD item = new DonHangMOD();
                        item.ID = reader.GetInt32(0);
                        item.OrderID = reader.GetString(1);
                        item.UserID = reader.GetInt32(2);
                        item.PhuongThucThanhToan = reader.GetString(3);
                        item.NgayMua = reader.GetDateTime(4);
                        item.Status = reader.GetInt32(5);

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
       
        public BaseResultMOD LayDonHangTheoID(string orderID)
        {
            var result = new BaseResultMOD();

            try
            {
                List<DonHangMOD> dscn = new List<DonHangMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from DonHang where OrderID =@OrderID";
                    cmd.Parameters.AddWithValue("@OrderID", orderID);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DonHangMOD item = new DonHangMOD();
                        item.ID = reader.GetInt32(0);
                        item.OrderID = reader.GetString(1);
                        item.UserID = reader.GetInt32(2);
                        item.PhuongThucThanhToan = reader.GetString(3);
                        item.NgayMua = reader.GetDateTime(4);
                        item.Status = reader.GetInt32(5);

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
        public BaseResultMOD LayChiTietDonHangTheoID(string orderID)
        {
            //const int ProductPerPage = 10;
            //int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<ChiTietDonHangMOD> dscn = new List<ChiTietDonHangMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from ChiTietDonHang where OrderID =@OrderID";
                    cmd.Parameters.AddWithValue("@OrderID", orderID);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ChiTietDonHangMOD item = new ChiTietDonHangMOD();
                        //item.ID = reader.GetInt32(0);
                        item.OrderID = reader.GetString(0);
                        item.SanPhamID = reader.GetInt32(1);
                        item.SoLuong = reader.GetInt32(2);
                        item.DonGia = reader.GetDecimal(3);
                        item.TrietKhau = reader.GetDecimal(4);
                        item.ThanhTien = reader.GetDecimal(5);


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

        public BaseResultMOD ThemChiTietDonHang(ChiTietDonHangMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                // Kiểm tra trước khi thêm chức năng
                //bool isDuplicate = KiemTraTrungChucNang(item.OrderID);
                //if (isDuplicate)
                //{
                //    result.Status = -1;
                //    result.Message = "Tên đơn hàng đã tồn tại.";
                //}
                //else
                //{
                    // Thêm chức năng vào cơ sở dữ liệu
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into ChiTietDonHang (OrderID,SanPhamID,SoLuong,DonGia,TrietKhau,ThanhTien) VALUES(@OrderID,@SanPhamID,@SoLuong,@DonGia,@TrietKhau,@ThanhTien)";
                        cmd.Parameters.AddWithValue("@OrderID", item.OrderID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@SanPhamID", item.SanPhamID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@SoLuong", item.SoLuong ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@DonGia", item.DonGia ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TrietKhau", item.TrietKhau ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ThanhTien", item.ThanhTien ?? (object)DBNull.Value);

                    cmd.Connection = SQLCon;
                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm chi tiết đơn hàng thành công";
                        result.Data = 1;
                    }
                //}
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }

        public BaseResultMOD SuaChiTietDonHang(string OrderID)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [DonHang] set Status =@Status where OrderID =@OrderID ";
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    //cmd.Parameters.AddWithValue("@Mota", item.Mota);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Cập nhật thành công";
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

        public BaseResultMOD XoaChiTietDonHang(int OrderID)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from DonHang where OrderID = @OrderID";
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if (rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa đơn hàng thành công";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = "OrderID không hợp lệ";
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

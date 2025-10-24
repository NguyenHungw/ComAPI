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
                        item.TrangThaiThanhToan = reader.GetInt32(5);
                        item.TrangThaiDonHang = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6);

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
        public BaseResultMOD getDSdonhang(int page, int ProductPerPage)
        {
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<DanhSachDonHangMOD> dssp = new List<DanhSachDonHangMOD>();
                int totalItems = 0;
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    //count
                    SQLCon.Open();
                    var cmdCount = new SqlCommand();
                    cmdCount.CommandType = CommandType.Text;
                    cmdCount.CommandText = @"SELECT COUNT(*) AS TotalItems from DonHang;";
                    cmdCount.Connection = SQLCon;
                    totalItems = (int)cmdCount.ExecuteScalar();

                    SqlCommand cmd = new SqlCommand();



                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT dh.ID,dh.OrderID,u.UserID,u.FullName,dh.PhuongThucThanhToan,dh.NgayMua,dh.TrangThaiThanhToan,dh.TrangThaiDonHang
                                        FROM DonHang dh
                                        LEFT JOIN Users u on dh.UserID = u.UserID
                                       ORDER BY dh.ID
                                       OFFSET @StartPage ROWS  
                                       FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhSachDonHangMOD item = new DanhSachDonHangMOD();
                        item.ID = reader.GetInt32(0);
                        item.OrderID = reader.IsDBNull(1) ? null : reader.GetString(1);
                        item.UserID = reader.IsDBNull (2) ? null : reader.GetInt32(2);
                        item.FullName = reader.IsDBNull(3) ? null : reader.GetString(3);
                        item.PhuongThucThanhToan = reader.IsDBNull(4) ? null : reader.GetString(4);
                        item.NgayMua = reader.IsDBNull(5) ? null : reader.GetDateTime(5);
                        item.TrangThaiThanhToan = reader.IsDBNull(6) ? null : reader.GetInt32(6);
                        item.TrangThaiDonHang = reader.IsDBNull(7) ? null : reader.GetInt32(7);

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
        public BaseResultMOD ChiTietDonHangIMG(string OrderID)
        {
            var result = new BaseResultMOD();
            try
            {
                List<ChiTietDonHangIMGMOD> dscn = new List<ChiTietDonHangIMGMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select dh.ID,ctdh.OrderID,ctdh.SanPhamID,spi.FilePath,sp.TenSanPham,ctdh.SoLuong,ctdh.DonGia,ctdh.TrietKhau,ctdh.ThanhTien
                                        from DonHang dh
                                        left join ChiTietDonHang ctdh on dh.OrderID = ctdh.OrderID
                                        left join SanPham sp on ctdh.SanPhamID = sp.ID
                                        left join SanPhamImage spi on sp.ID = spi.SanPhamID
                                        where ctdh.OrderID = @OrderID and spi.IndexOrder=0";
                    cmd.Connection = SQLCon;
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.ExecuteNonQuery();
            
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ChiTietDonHangIMGMOD item = new ChiTietDonHangIMGMOD();
                        item.ID = reader.GetInt32(0);
                        item.OrderID = reader.GetString(1);
                        item.SanPhamID = reader.GetInt32(2);
                        item.FilePath = reader.GetString(3);
                        item.TenSanPham = reader.GetString(4);
                        item.SoLuong = reader.GetInt32(5);
                        item.DonGia = reader.GetDecimal(6);
                        item.TrietKhau = reader.GetDecimal(7);
                        item.ThanhTien = reader.GetDecimal(8);


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
                        cmd.CommandText = "Insert into DonHang (OrderID,UserID,PhuongThucThanhToan,NgayMua,TrangThaiThanhToan) VALUES(@OrderID,@UserID,@PhuongThucThanhToan,@NgayMua,@TrangThaiThanhToan)";
                        cmd.Parameters.AddWithValue("@OrderID", item.OrderID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UserID", item.UserID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhuongThucThanhToan", item.PhuongThucThanhToan ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NgayMua", SqlDbType.DateTime2).Value = DateTime.Now;
                        cmd.Parameters.AddWithValue("@TrangThaiThanhToan", item.TrangThaiThanhToan ?? (object)DBNull.Value);

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
                    cmd.CommandText = "Update [DonHang] set TrangThaiThanhToan =@TrangThaiThanhToan where OrderID =@OrderID ";
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
      
        public BaseResultMOD CapNhatTrangThaiDonHang1(string OrderID,int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [DonHang] set TrangThaiDonHang =@TrangThaiDonHang where OrderID =@OrderID ";
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.Parameters.AddWithValue("@TrangThaiDonHang", id);
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
        //auto checkout
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
                    cmd.CommandText = "Update [DonHang] set TrangThaiThanhToan =@TrangThaiThanhToan, TrangThaiDonHang =@TrangThaiDonHang where OrderID =@OrderID ";
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.Parameters.AddWithValue("@TrangThaiThanhToan", TrangThaiThanhToan.DaThanhToan);
                    cmd.Parameters.AddWithValue("@TrangThaiDonHang", TrangThaiDonHang.ChoXuLy);
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
                        item.TrangThaiThanhToan = reader.GetInt32(5);
                        item.TrangThaiDonHang = reader.GetInt32(6);


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
                        item.TrangThaiThanhToan = reader.GetInt32(5);
                        item.TrangThaiDonHang = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6);
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
                        item.ID = reader.GetInt32(0);
                        item.OrderID = reader.GetString(1);
                        item.SanPhamID = reader.GetInt32(2);
                        item.SoLuong = reader.GetInt32(3);
                        item.DonGia = reader.GetDecimal(4);
                        item.TrietKhau = reader.GetDecimal(5);
                        item.ThanhTien = reader.GetDecimal(6);


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

using COM.MOD;
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
    public class LoaiSanPhamDAL
    {
        // private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        public BaseResultMOD getdsLoaiSanPham()
        {
         
            var result = new BaseResultMOD();
            try
            {
                List<LoaiSanPhamMOD> dscn = new List<LoaiSanPhamMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from LoaiSanPham Where TrangThai=1 ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        LoaiSanPhamMOD item = new LoaiSanPhamMOD();
                        item.LoaiSanPhamID = reader.GetInt32(0);
                        item.TenLoaiSanPham = reader.GetString(1);
                        item.MoTaLoaiSP = reader.GetString(2);
                        item.TrangThai = reader.GetInt32(3);
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
        public BaseResultMOD getdsLoaiSanPhamPage(int page)
        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<LoaiSanPhamMOD> dscn = new List<LoaiSanPhamMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from LoaiSanPham ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        LoaiSanPhamMOD item = new LoaiSanPhamMOD();
                        item.LoaiSanPhamID = reader.GetInt32(0);
                        item.TenLoaiSanPham = reader.GetString(1);
                        item.MoTaLoaiSP = reader.GetString(2);
                        item.TrangThai = reader.GetInt32(3);
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
        public BaseResultMOD ThemLoaiSanPham(LoaiSanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                // Kiểm tra trước khi thêm chức năng
                bool isDuplicate = KiemTraTrungChucNang(item.TenLoaiSanPham);
                if (isDuplicate)
                {
                    result.Status = -1;
                    result.Message = "Tên loại sản phẩm đã tồn tại.";
                }
                else
                {
                    // Thêm chức năng vào cơ sở dữ liệu
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into LoaiSanPham (TenLoaiSP,MoTaLoaiSP,TrangThai) VALUES(@TenLoaiSP,@MoTaLoaiSP,@TrangThai)";
                        cmd.Parameters.AddWithValue("@TenLoaiSP", item.TenLoaiSanPham ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@MoTaLoaiSP", item.MoTaLoaiSP ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TrangThai", item.TrangThai ?? (object)DBNull.Value);
                        cmd.Connection = SQLCon;
                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm loại sản phẩm thành công";
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
        private bool KiemTraTrungChucNang(string namecn)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();
                string checkQuery = "SELECT COUNT(*) FROM LoaiSanPham WHERE TenLoaiSP = @TenLoaiSP";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@TenLoaiSP", namecn);
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }

        public BaseResultMOD SuaLoaiSanPham(LoaiSanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE [LoaiSanPham] SET TenLoaiSP = @TenLoaiSP, MoTaLoaiSP = @MoTaLoaiSP, TrangThai = @TrangThai WHERE LoaiSanPhamID = @LoaiSanPhamID";
                    cmd.Parameters.AddWithValue("@LoaiSanPhamID", item.LoaiSanPhamID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TenLoaiSP", item.TenLoaiSanPham ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MoTaLoaiSP", item.MoTaLoaiSP ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TrangThai", item.TrangThai ?? (object)DBNull.Value);

                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Sửa loại sản phẩm thành công";
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
        public BaseResultMOD XoaLoaiSanPham(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from LoaiSanPham where LoaiSanPhamID = @LoaiSanPhamID";
                    cmd.Parameters.AddWithValue("@LoaiSanPhamID", id);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if (rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa loại sản phẩm thành công";
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

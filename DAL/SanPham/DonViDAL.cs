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
    public class DonViDAL
    {
        // private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        public BaseResultMOD getdsDonVi()
        {
           
            var result = new BaseResultMOD();
            try
            {
                List<DonViMOD> dscn = new List<DonViMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from DonViTinh ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DonViMOD item = new DonViMOD();
                        item.DonViTinhID = reader.GetInt32(0);
                        item.TenDonVi = reader.GetString(1);
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
        public BaseResultMOD getdsDonViPage(int page, int ProductPerPage)
        {
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<DonViMOD> dssp = new List<DonViMOD>();
                int totalItems = 0;
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    //count
                    SQLCon.Open();
                    var cmdCount = new SqlCommand();
                    cmdCount.CommandType = CommandType.Text;
                    cmdCount.CommandText = @"SELECT COUNT(*) AS TotalItems FROM DonViTinh;";
                    cmdCount.Connection = SQLCon;
                    totalItems = (int)cmdCount.ExecuteScalar();

                    SqlCommand cmd = new SqlCommand();



                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT * FROM DonViTinh
                                       ORDER BY DonViTinhID
                                       OFFSET @StartPage ROWS  
                                       FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DonViMOD item = new DonViMOD();
                        item.DonViTinhID = reader.GetInt32(0);
                        item.TenDonVi = reader.IsDBNull(1) ? null : reader.GetString(1);
                        item.Mota = reader.IsDBNull(2) ? null : reader.GetString(2);
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
        public BaseResultMOD ThemDonVi(DonViMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                // Kiểm tra trước khi thêm chức năng
                bool isDuplicate = KiemTraTrungChucNang(item.TenDonVi);
                if (isDuplicate)
                {
                    result.Status = -1;
                    result.Message = "Tên chức năng đã tồn tại.";
                }
                else
                {
                    // Thêm chức năng vào cơ sở dữ liệu
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into DonViTinh (TenDonVi,Mota) VALUES(@TenDonVi,@Mota)";
                        cmd.Parameters.AddWithValue("@TenDonVi", item.TenDonVi ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Mota", item.Mota ?? (object)DBNull.Value);


                        cmd.Connection = SQLCon;
                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm đơn vị thành công";
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
        private bool KiemTraTrungChucNang(string tendonvi)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();
                string checkQuery = "SELECT COUNT(*) FROM DonViTinh WHERE TenDonVi = @TenDonVi";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@TenDonVi", tendonvi);
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }

        public BaseResultMOD SuaDonVi(DonViMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [DonViTinh] set TenDonVi =@TenDonVi ,Mota = @Mota where DonViTinhID =@DonViTinhID ";
                    cmd.Parameters.AddWithValue("@TenDonVi", item.TenDonVi);
                    cmd.Parameters.AddWithValue("@Mota", item.Mota);
                    cmd.Parameters.AddWithValue("@DonViTinhID", item.DonViTinhID);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Sửa đơn vị thành công";
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
        public BaseResultMOD XoaDonVi(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from DonViTinh where DonViTinhID = @DonViTinhID";
                    cmd.Parameters.AddWithValue("@DonViTinhID", id);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if (rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa đơn vị thành công";
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

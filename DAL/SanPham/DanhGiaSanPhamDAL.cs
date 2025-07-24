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
    public class DanhGiaSanPhamDAL
    {
        // private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        public BaseResultMOD getdsDanhGiaSanPham(int page)
        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<DanhGiaSanPhamMOD> dsdgsp = new List<DanhGiaSanPhamMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from DanhGiaSanPham ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhGiaSanPhamMOD item = new DanhGiaSanPhamMOD();
                        item.ID = reader.GetInt32(0);
                        item.UserID = reader.GetInt32(1);
                        item.SanPhamID = reader.GetInt32(2);
                        item.DiemDanhGia = reader.GetInt32(3);
                        item.NhanXet = reader.GetString(4);
                        item.NgayDanhGia = reader.GetDateTime(5);

                        dsdgsp.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsdgsp;
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System + ex;
                throw;

            }
            return result;

        }
        public BaseResultMOD ThemDanhGiaSanPham(DanhGiaSanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
              
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into DanhGiaSanPham (UserID,SanPhamID,DiemDanhGia,NhanXet,NgayDanhGia) VALUES(@UserID,@SanPhamID,@DiemDanhGia,@NhanXet,@NgayDanhGia)";
                        cmd.Parameters.AddWithValue("@UserID", item.UserID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SanPhamID", item.SanPhamID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DiemDanhGia", item.DiemDanhGia ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NhanXet", item.NhanXet ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NgayDanhGia", item.NgayDanhGia ?? (object)DBNull.Value);





                    cmd.Connection = SQLCon;
                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm đơn vị thành công";
                        result.Data = 1;
                    }
               // }
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

        public BaseResultMOD SuaDanhGiaSanPham(DanhGiaSanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [DanhGiaSanPham] set NhanXet =@NhanXet,DiemDanhGia=@DiemDanhGia where UserID =@UserID AND SanPhamID= @SanPhamID  ";
                    cmd.Parameters.AddWithValue("@UserID", item.UserID );
                    cmd.Parameters.AddWithValue("@SanPhamID", item.SanPhamID );
                    cmd.Parameters.AddWithValue("@DiemDanhGia", item.DiemDanhGia);
                    cmd.Parameters.AddWithValue("@NhanXet", item.NhanXet );
                    cmd.Parameters.AddWithValue("@NgayDanhGia", item.NgayDanhGia );
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Sửa đánh giá thành công";
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
        public BaseResultMOD XoaDanhGiaSanPham(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from DanhGiaSanPham where ID = @ID";
                    cmd.Parameters.AddWithValue("@DonViTinhID", id);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if (rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa đánh giá thành công";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = id+" không hợp lệ";
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

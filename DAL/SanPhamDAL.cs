using COM.MOD;
using COM.ULT;
//using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.DAL
{
    public class SanPhamDAL
    {
       // private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        public BaseResultMOD getdsSanPham(int page)
        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<SanPhamMOD> dssp = new List<SanPhamMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from SanPham ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SanPhamMOD item = new SanPhamMOD();
                        item.MSanPham = reader.GetValue(1).ToString();
                        //item.MSanPham = reader.GetValue(1).ToString();

                        item.TenSanPham = reader.GetString(2);
                        item.LoaiSanPhamID = reader.GetInt32(3);
                        item.DonViTinhID = reader.GetInt32(4);

                        dssp.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dssp;
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
        public BaseResultMOD ThemSanPham(SanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                // Kiểm tra trước khi thêm chức năng
                bool isDuplicate = KiemTraTrungChucNang(item.MSanPham);
                if (isDuplicate)
                {
                    result.Status = -1;
                    result.Message = "Tên sản phẩm đã tồn tại.";
                }
                else
                {
                    // Thêm chức năng vào cơ sở dữ liệu
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        string shortGuid2 = ULT.Utils.Utilities.GenerateRandomCode(5);
                        string name = ULT.Utils.Utilities.RemoveDiacritics(item.TenSanPham.Replace(" ","-")); //để dấu gạch cho chuẩn seo
                        //string shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8); // 8 ký tự ngẫu nhiên

                        //string msp = $"{shortGuid2}_{item.TenSanPham}";
                        string msp = $"{shortGuid2}-{name}";


                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into SanPham (MSanPham,TenSanPham,LoaiSanPhamID,DonViTinhID) VALUES(@MSanPham,@TenSanPham,@LoaiSanPhamID,@DonViTinhID)";
                        cmd.Parameters.AddWithValue("@MSanPham", msp ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TenSanPham", item.TenSanPham ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@LoaiSanPhamID", item.LoaiSanPhamID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@DonViTinhID", item.DonViTinhID ?? (object)DBNull.Value);
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
                string checkQuery = "SELECT COUNT(*) FROM SanPham WHERE MSanPham = @MSanPham";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@MSanPham", tendonvi);
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }

        public BaseResultMOD SuaSanPham(SanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using(SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [SanPham] set TenSanPham = @TenSanPham, LoaiSanPhamID = @LoaiSanPhamID, DonViTinhID = @DonViTinhID where MSanPham=@MSanPham ";
                    cmd.Parameters.AddWithValue("@TenSanPham", item.TenSanPham);
                    cmd.Parameters.AddWithValue("@LoaiSanPhamID", item.LoaiSanPhamID);
                    cmd.Parameters.AddWithValue("@DonViTinhID", item.DonViTinhID);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    result.Status = 1;
                    result.Message = "Sửa sản phẩm thành công";
                    result.Data=1;
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD XoaSanPham(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using(SqlConnection SQLCon =new SqlConnection(SQLHelper.appConnectionStrings)){
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from SanPham where MSanPham = @MSanPham";
                    cmd.Parameters.AddWithValue("@MSanPham", id);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if(rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa đơn sản phẩm thành công";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = "mã sản phẩm "+id+" không hợp lệ";

                    }
                }

            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }
        
    }
}

using COM.MOD;
using COM.MOD.SanPham;
using COM.ULT;
using Microsoft.AspNetCore.Http;

//using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace COM.DAL.SanPham
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
                        item.MoTa = reader.GetString(5);
                        item.SoLuong = reader.GetInt32(6);

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
        public BaseResultMOD getdsSanPhamView(int page)
        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<SanPhamTrangChuMOD> dssp = new List<SanPhamTrangChuMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT spi.FilePath,sp.TenSanPham,dgsp.DiemDanhGia,gb.GiaBan,gb.SalePercent,GiaSauGiam,gb.NgayBatDau  
                                       From [SanPham] sp  
                                       LEFT JOIN SanPhamImage spi on sp.ID= spi.SanPhamID  
                                       LEFT JOIN DanhGiaSanPham dgsp on sp.ID = dgsp.SanPhamID  
                                       LEFT JOIN GiaBanSanPham gb on sp.ID = gb.SanPhamID   
                                       WHERE spi.IndexOrder=0  
                                       ORDER BY sp.id  
                                       OFFSET @StartPage ROWS  
                                       FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SanPhamTrangChuMOD item = new SanPhamTrangChuMOD();
                        item.FilePath = reader.IsDBNull(0) ? null : reader.GetString(0);
                        item.TenSanPham = reader.GetString(1);
                        item.DiemDanhGia = reader.IsDBNull(2) ? null : reader.GetInt32(2);
                        item.GiaBan = reader.GetDecimal(3);
                        item.SalePercent = reader.GetDecimal(4);
                        item.GiaSauGiam = reader.GetDecimal(5);
                        item.NgayBatDau = reader.IsDBNull(6) ? null : DateOnly.FromDateTime(reader.GetDateTime(6)); // Fixed conversion issue  
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
        public BaseResultMOD getdsSanPhamViewTotal(int page ,int ProductPerPage)
        {
            //const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<SanPhamTrangChuMOD> dssp = new List<SanPhamTrangChuMOD>();
                int totalItems = 0;
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    //count
                    SQLCon.Open();
                    var cmdCount = new SqlCommand();
                    cmdCount.CommandType = CommandType.Text;
                    cmdCount.CommandText = @"SELECT COUNT(*) AS TotalItems
                                            FROM SanPham sp
                                            LEFT JOIN SanPhamImage spi ON sp.ID = spi.SanPhamID
                                            WHERE spi.IndexOrder = 0;";
                    cmdCount.Connection = SQLCon;
                    totalItems = (int)cmdCount.ExecuteScalar();

                    SqlCommand cmd = new SqlCommand();



                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT spi.FilePath,sp.TenSanPham,dgsp.DiemDanhGia,gb.GiaBan,gb.SalePercent,GiaSauGiam,gb.NgayBatDau  
                                       From [SanPham] sp  
                                       LEFT JOIN SanPhamImage spi on sp.ID= spi.SanPhamID  
                                       LEFT JOIN DanhGiaSanPham dgsp on sp.ID = dgsp.SanPhamID  
                                       LEFT JOIN GiaBanSanPham gb on sp.ID = gb.SanPhamID   
                                       WHERE spi.IndexOrder=0  
                                       ORDER BY sp.id  
                                       OFFSET @StartPage ROWS  
                                       FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SanPhamTrangChuMOD item = new SanPhamTrangChuMOD();
                        item.FilePath = reader.IsDBNull(0) ? null : reader.GetString(0);
                        item.TenSanPham = reader.GetString(1);
                        item.DiemDanhGia = reader.IsDBNull(2) ? null : reader.GetInt32(2);
                        item.GiaBan = reader.GetDecimal(3);
                        item.SalePercent = reader.GetDecimal(4);
                        item.GiaSauGiam = reader.GetDecimal(5);
                        item.NgayBatDau = reader.IsDBNull(6) ? null : DateOnly.FromDateTime(reader.GetDateTime(6)); // Fixed conversion issue  
                        dssp.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dssp;
                    result.TotalRow =totalItems;
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
        public BaseResultMOD ChiTietSanPhamView(int id)
        {
            var result = new BaseResultMOD();   
            try
            {
                List<ChiTietSanPhamTrangChuMOD> dssp = new List<ChiTietSanPhamTrangChuMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT spi.FilePath,sp.TenSanPham,gb.GiaBan,gb.SalePercent,GiaSauGiam,gb.NgayBatDau,sp.MoTa  
                                       From [SanPham] sp  
                                       LEFT JOIN SanPhamImage spi on sp.ID= spi.SanPhamID  
                                       LEFT JOIN DanhGiaSanPham dgsp on sp.ID = dgsp.SanPhamID  
                                       LEFT JOIN GiaBanSanPham gb on sp.ID = gb.SanPhamID   
                                       WHERE sp.ID = @id;";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ChiTietSanPhamTrangChuMOD item = new ChiTietSanPhamTrangChuMOD();
                        item.FilePath = reader.IsDBNull(0) ? null : reader.GetString(0);
                        item.TenSanPham = reader.GetString(1);
                        item.GiaBan = reader.GetDecimal(2);
                        item.SalePercent = reader.GetDecimal(3);
                        item.GiaSauGiam = reader.GetDecimal(4);
                        item.NgayBatDau = reader.IsDBNull(5) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(5));
                        item.MoTa = reader.IsDBNull(6) ? null : reader.GetString(6);
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
                        string shortGuid2 = Utils.Utilities.GenerateRandomCode(8);
                        string name = Utils.Utilities.RemoveDiacritics(item.TenSanPham.Replace(" ", "-")); //để dấu gạch cho chuẩn seo
                        //string shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8); // 8 ký tự ngẫu nhiên

                        //string msp = $"{shortGuid2}_{item.TenSanPham}";
                        //string msp = $"{shortGuid2}-{name}";
                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into SanPham (MSanPham,TenSanPham,LoaiSanPhamID,DonViTinhID) VALUES(@MSanPham,@TenSanPham,@LoaiSanPhamID,@DonViTinhID)";
                        cmd.Parameters.AddWithValue("@MSanPham", shortGuid2 ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TenSanPham", item.TenSanPham ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@LoaiSanPhamID", item.LoaiSanPhamID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@DonViTinhID", item.DonViTinhID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@MoTa", item.MoTa ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@SoLuong", item.SoLuong ?? (object)DBNull.Value); 
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


        public void ThemSanPhamAnhVaGia(List<IFormFile> files, SanPhamAnhVaGiaMOD item)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                string Picture;

                var Result = new BaseResultMOD();
                SQLCon.Open();
                var trans = SQLCon.BeginTransaction();
                string shortGuid2 = Utils.Utilities.GenerateRandomCode(8);

                try
                {
                    var cmd = new SqlCommand(@"
                INSERT INTO SanPham (MSanPham, TenSanPham, LoaiSanPhamID, DonViTinhID, MoTa,SoLuong)
                VALUES (@MSanPham, @TenSanPham, @LoaiSanPhamID, @DonViTinhID,@MoTa,@SoLuong);
                SELECT SCOPE_IDENTITY();", SQLCon, trans); //trả về ID vừa chèn


                    cmd.Parameters.AddWithValue("@MSanPham", shortGuid2);
                    cmd.Parameters.AddWithValue("@TenSanPham", item.TenSanPham);
                    cmd.Parameters.AddWithValue("@LoaiSanPhamID", item.LoaiSanPhamID);
                    cmd.Parameters.AddWithValue("@DonViTinhID", item.DonViTinhID);
                    
                    cmd.Parameters.AddWithValue("@MoTa", item.MoTa);
                    cmd.Parameters.AddWithValue("@SoLuong", item.SoLuong ?? (object)DBNull.Value); // nếu SoLuong là null thì truyền DBNull
                    int newID = Convert.ToInt32(cmd.ExecuteScalar()); // lấy ra id vừa chèn và chuyển kq sang int

                    // 2. Thêm giá
                    var cmdGia = new SqlCommand(@"
                INSERT INTO GiaBanSanPham (SanPhamID, NgayBatDau, GiaBan, SalePercent, GiaSauGiam)
                VALUES (@SanPhamID, @NgayBatDau, @GiaBan, @SalePercent,@GiaSauGiam);", SQLCon, trans);
                    cmdGia.Parameters.AddWithValue("@SanPhamID", newID);
                    cmdGia.Parameters.AddWithValue("@NgayBatDau", DateTime.UtcNow);

                    decimal giaSauGiam = (decimal)(item.GiaBan * (1 - item.SalePercent / 100m));

                    cmdGia.Parameters.AddWithValue("@GiaBan", item.GiaBan);
                    cmdGia.Parameters.AddWithValue("@SalePercent", item.SalePercent);
                    cmdGia.Parameters.AddWithValue("@GiaSauGiam", giaSauGiam);
                    cmdGia.ExecuteNonQuery();

                    //3. Thêm hình ảnh
                    int index = 0;

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            string extension = Path.GetExtension(file.FileName);

                            string name = Utils.Utilities.RemoveDiacritics(item.TenSanPham.Replace(" ", "")); //để dấu gạch cho chuẩn seo
                            string newName = $"{shortGuid2}_{item.TenSanPham}{extension}";
                            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
                            string filePath = Path.Combine(uploadsFolder, newName);
                            using (Stream stream = File.Create(filePath))
                            {
                                file.CopyTo(stream);

                            }
                            Picture = "/upload/" + newName;
                        }
                        else
                        {
                            Picture = "";
                        }

                        var cmdImg = new SqlCommand(@"
                    INSERT INTO SanPhamImage (SanPhamID, FilePath, IndexOrder)
                    VALUES (@SanPhamID, @FilePath, @IndexOrder);", SQLCon, trans);
                        cmdImg.Parameters.AddWithValue("@SanPhamID", newID);
                        cmdImg.Parameters.AddWithValue("@FilePath", Picture);
                        cmdImg.Parameters.AddWithValue("@IndexOrder", index++);
                        cmdImg.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void SuaSanPhamAnhVaGia(List<IFormFile> files, SanPhamAnhVaGiaMOD item)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                string Picture;
                var Result = new BaseResultMOD();
                SQLCon.Open();
                var trans = SQLCon.BeginTransaction();
                string shortGuid2 = Utils.Utilities.GenerateRandomCode(8);
                try
                {
                    var cmd = new SqlCommand(@"Update [SanPham] set TenSanPham =@TenSanPham, LoaiSanPhamID = @LoaiSanPhamID, DonViTinhID = @DonViTinhID OUTPUT INSERTED.ID where MSanPham=@MSanPham;
                                             ", SQLCon, trans); //lấy ra ID vừa update
                    //cmd.Parameters.AddWithValue("@MSanPham", shortGuid2);
                    cmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
                    cmd.Parameters.AddWithValue("@TenSanPham", item.TenSanPham);
                    cmd.Parameters.AddWithValue("@LoaiSanPhamID", item.LoaiSanPhamID);
                    cmd.Parameters.AddWithValue("@DonViTinhID", item.DonViTinhID);
                    cmd.Parameters.AddWithValue("@MoTa", item.MoTa);
                    cmd.Parameters.AddWithValue("@SoLuong", item.SoLuong ?? (object)DBNull.Value); // nếu SoLuong là null thì truyền DBNull
                    int newID = Convert.ToInt32(cmd.ExecuteScalar()); // lấy ra id vừa chèn và chuyển kq sang int


                    var cmdGia = new SqlCommand(@"Update [GiaBanSanPham] set SanPhamID =@SanPhamID, NgayBatDau = @NgayBatDau, GiaBan = @GiaBan,SalePercent = @SalePercent,GiaSauGiam 
                                                = @GiaSauGiam where SanPhamID=@SanPhamID", SQLCon, trans);
                    // 2. Thêm giá

                    cmdGia.Parameters.AddWithValue("@SanPhamID", newID);
                    cmdGia.Parameters.AddWithValue("@NgayBatDau", DateTime.UtcNow);

                    decimal giaSauGiam = (decimal)(item.GiaBan * (1 - item.SalePercent / 100));

                    cmdGia.Parameters.AddWithValue("@GiaBan", item.GiaBan);
                    cmdGia.Parameters.AddWithValue("@SalePercent", item.SalePercent);
                    cmdGia.Parameters.AddWithValue("@GiaSauGiam", giaSauGiam);
                    cmdGia.ExecuteNonQuery();
                    //3. Thêm hình ảnh
                    int index = 0;

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            string name = Utils.Utilities.RemoveDiacritics(item.TenSanPham.Replace(" ", "")); //để dấu gạch cho chuẩn seo
                            string newName = $"{shortGuid2}_{item.TenSanPham}";
                            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
                            string filePath = Path.Combine(uploadsFolder, newName);
                            using (Stream stream = File.Create(filePath))
                            {
                                file.CopyTo(stream);
                            }
                            Picture = "/upload/" + newName;
                        }
                        else
                        {
                            Picture = "";
                        }
                        var cmdImg = new SqlCommand(@"UPDATE [SanPhamImage] set SanPhamID =@SanPhamID, FilePath=@FilePath,IndexOrder=@IndexOrder", SQLCon, trans);
                        cmdImg.Parameters.AddWithValue("@SanPhamID", newID);
                        cmdImg.Parameters.AddWithValue("@FilePath", Picture);
                        cmdImg.Parameters.AddWithValue("@IndexOrder", index++);
                        cmdImg.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
        public BaseResultMOD XoaSanPhamAnhVaGia(int id)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                var result = new BaseResultMOD();
                SQLCon.Open();
                var trans = SQLCon.BeginTransaction();
                try
                {
                    var cmdGia = new SqlCommand(@"DELETE FROM [GiaBanSanPham] where SanPhamID=@SanPhamID", SQLCon, trans);
                    cmdGia.Parameters.AddWithValue("@SanPhamID", id);
                    cmdGia.ExecuteNonQuery();
                    //3. xóa hình ảnh
                    var cmdImg = new SqlCommand(@"DELETE FROM [SanPhamImage] where SanPhamID=@SanPhamID", SQLCon, trans);
                    cmdImg.Parameters.AddWithValue("@SanPhamID", id);

                    cmdImg.ExecuteNonQuery();

                    var cmd = new SqlCommand(@"Delete FROM [SanPham] where ID=@ID;", SQLCon, trans); //lấy ra ID vừa update
                    cmd.Parameters.AddWithValue("@ID", id);

                    //int newID = Convert.ToInt32(cmd.ExecuteScalar()); // lấy ra id vừa chèn và chuyển kq sang int
                    cmd.ExecuteNonQuery();

                    trans.Commit();
                    result.Status = 1;
                    result.Message = "Xóa sản phẩm thành công";
                    result.Data = id;

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
                return result;
            }
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
        public BaseResultMOD TruSoLuongSanPham(int sanphamID,int soluong)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [SanPham] set SoLuong = SoLuong - @SoLuong where ID=@sanphamID ";
                    cmd.Parameters.AddWithValue("@sanphamID", sanphamID);
                    cmd.Parameters.AddWithValue("@SoLuong", soluong);   
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    result.Status = 1;
                    result.Message = "Trừ Số Lượng Thành công";
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

        public BaseResultMOD SuaSanPham(SanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
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
        public BaseResultMOD XoaSanPham(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from SanPham where MSanPham = @MSanPham";
                    cmd.Parameters.AddWithValue("@MSanPham", id);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if (rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa đơn sản phẩm thành công";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = "mã sản phẩm " + id + " không hợp lệ";

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

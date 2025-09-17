using COM.MOD;
using COM.MOD.SanPham;
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
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace COM.DAL.SanPham
{
    public class SanPhamImageDAL
    {
        // private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        public BaseResultMOD getdsSanPhamImage(int page)
        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<SanPhamImageMOD> dsimgsp = new List<SanPhamImageMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from SanPhamImage ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SanPhamImageMOD item = new SanPhamImageMOD();
                        item.ID = reader.GetInt32(0);
                        item.SanPhamImageID = reader.GetInt32(1);
                        item.FilePath = reader.GetString(2);
                        item.IndexOrder = reader.GetInt32(3);
                        dsimgsp.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsimgsp;
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
        public BaseResultMOD ThemImage(List<IFormFile> files, int id)
        {
            var result = new BaseResultMOD();
            try
            {
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                    string Picture;
                    string tenSanPham = "";

                    string shortGuid2 = Utils.Utilities.GenerateRandomCode(8);

                    SQLCon.Open();
                    

                    SqlCommand cmdGet = new SqlCommand();
                    cmdGet.Connection = SQLCon;
                    cmdGet.CommandType = CommandType.Text;
                  
                    cmdGet.CommandText = @"SELECT sp.TenSanPham
                    FROM SanPham sp 
                    LEFT JOIN SanPhamImage SPI ON sp.ID = SPI.SanPhamID
                    Where sp.ID= @SanPhamID";
                    cmdGet.Parameters.AddWithValue("@SanPhamID", id);
                    var data = cmdGet.ExecuteScalar();
                    if (data != null)
                    {
                        tenSanPham = data.ToString();
                    }
                    else
                    {
                        tenSanPham = "HinhAnh";
                    }
                    SqlCommand cmd = new SqlCommand();

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            string extension = Path.GetExtension(file.FileName); 
                            string name = Utils.Utilities.RemoveDiacritics(tenSanPham.Replace(" ", "")); //để dấu gạch cho chuẩn seo
                            string newName = $"{shortGuid2}_{tenSanPham}{extension}";
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
                    
                    cmd.CommandType = CommandType.Text;
                        cmd = new SqlCommand(@"
                        INSERT INTO SanPhamImage (SanPhamID, FilePath, IndexOrder)
                        VALUES (
                            @SanPhamID,
                            @FilePath,
                            ISNULL((SELECT MAX(IndexOrder) + 1 FROM SanPhamImage WHERE SanPhamID = @SanPhamID), 1)
                        )");

                        cmd.Parameters.AddWithValue("@SanPhamID", id );
                        cmd.Parameters.AddWithValue("@FilePath", Picture ?? (object)DBNull.Value);
                    //cmd.Parameters.AddWithValue("@IndexOrder", item.IndexOrder ?? (object)DBNull.Value);



                    cmd.Connection = SQLCon;
                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm hình ảnh thành công";
                        result.Data = 1;
                    }
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

        public BaseResultMOD SuaSanPhamIMG(int id, IFormFile file)
        {
            var result = new BaseResultMOD();
            string Picture;
            string tenSanPham = "";

            string shortGuid2 = Utils.Utilities.GenerateRandomCode(8);

            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                   


                    SqlCommand cmdGet = new SqlCommand();
                    cmdGet.Connection = SQLCon;
                    cmdGet.CommandType = CommandType.Text;

                    cmdGet.CommandText = @"SELECT sp.TenSanPham
                    FROM SanPham sp 
                    LEFT JOIN SanPhamImage SPI ON sp.ID = SPI.SanPhamID
                    Where sp.ID= @SanPhamID";
                    cmdGet.Parameters.AddWithValue("@SanPhamID", id);
                    var data = cmdGet.ExecuteScalar();
                    if (data != null)
                    {
                        tenSanPham = data.ToString();
                    }
                    else
                    {
                        tenSanPham = "HinhAnh";
                    }
                    if (file.Length > 0)
                    {
                        string extension = Path.GetExtension(file.FileName);
                        string name = Utils.Utilities.NormalizeFileName(tenSanPham); //để dấu gạch cho chuẩn seo
                        string newName = $"{shortGuid2}_{tenSanPham}{extension}";
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
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [SanPhamImage] set FilePath =@FilePath where SanPhamID =@SanPhamID ";
                    cmd.Parameters.AddWithValue("@SanPhamID", id);
                    cmd.Parameters.AddWithValue("@FilePath", Picture);
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
        public BaseResultMOD XoaSanPhamIMG(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from SanPhamImage where ID = @ID";
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if (rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa ảnh sản phẩm thành công";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = id + "không hợp lệ";

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
        public BaseResultMOD DoiViTriIMG(int id,int currentIndex,int nextIndex )
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    var trans= SQLCon.BeginTransaction();
                    try
                    {
                        // đổi tạm indexorder của ảnh hiện tại (current) thành -1
                        var cmdTemp = new SqlCommand();
                        cmdTemp.CommandType = CommandType.Text;
                        cmdTemp.CommandText = "Update [SanPhamImage] set IndexOrder= -1 where SanPhamID = @SanPhamID AND IndexOrder=@CurrentIndex";
                        cmdTemp.Parameters.AddWithValue("@SanPhamID", id);
                        cmdTemp.Parameters.AddWithValue("@CurrentIndex", currentIndex);
                        cmdTemp.Connection = SQLCon;
                        cmdTemp.Transaction = trans;

                        cmdTemp.ExecuteNonQuery();
                        // gán indexorder của ảnh dích (next) thành current
                        SqlCommand cmdCurrentIndex = new SqlCommand();
                        cmdCurrentIndex.CommandType = CommandType.Text;
                        cmdCurrentIndex.CommandText = "Update [SanPhamImage] set IndexOrder=@CurrentIndex where SanPhamID =@SanPhamID AND IndexOrder = @NextIndex ";
                        cmdCurrentIndex.Parameters.AddWithValue("@SanPhamID", id);
                        cmdCurrentIndex.Parameters.AddWithValue("@CurrentIndex", currentIndex);

                        cmdCurrentIndex.Parameters.AddWithValue("@NextIndex", nextIndex);
                        cmdCurrentIndex.Connection = SQLCon;
                        cmdCurrentIndex.Transaction = trans;

                        cmdCurrentIndex.ExecuteNonQuery();
                        // gán ảnh có indexorder = -1 thành next index
                        SqlCommand cmdNextIndex = new SqlCommand();
                        cmdNextIndex.CommandType = CommandType.Text;
                        cmdNextIndex.CommandText = "Update [SanPhamImage] set IndexOrder=@IndexOrder where SanPhamID =@SanPhamID and IndexOrder = -1 ";
                        cmdNextIndex.Parameters.AddWithValue("@SanPhamID", id);
                        cmdNextIndex.Parameters.AddWithValue("@IndexOrder", nextIndex);
                        cmdNextIndex.Connection = SQLCon;
                        cmdNextIndex.Transaction = trans;

                        cmdNextIndex.ExecuteNonQuery();
                        trans.Commit();
                        result.Status = 1;
                        result.Message = "Đổi vị trí thành công";

                        result.Data = 1;
                       
                    }
                    catch
                    {
                        trans.Rollback();
                        result.Status = -1;
                        result.Message = "Đổi vị trí thất bại";
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
        //update nhieu hinh anh
        public BaseResultMOD UpdateImageOrder(List<ImageOrderUpdateModel> list)
        {
            BaseResultMOD result = new BaseResultMOD();
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();
                SqlTransaction trans = SQLCon.BeginTransaction();

                try
                {
                    foreach (var item in list)
                    {
                        SqlCommand cmd = new SqlCommand(@"
                    UPDATE SanPhamImage 
                    SET IndexOrder = @IndexOrder 
                    WHERE ID = @ID", SQLCon, trans);

                        cmd.Parameters.AddWithValue("@IndexOrder", item.NewIndex);
                        cmd.Parameters.AddWithValue("@ID", item.ImageID);
                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();
                    result.Status = 1;
                    result.Message = "Cập nhật thứ tự thành công";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    result.Status = -1;
                    result.Message = "Lỗi: " + ex.Message;
                }
            }
            return result;
        }

    }
}

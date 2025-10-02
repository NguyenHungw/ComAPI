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

namespace CT.DAL
{
    public class NhomNgoiDungDAL
    {
       // private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        //SqlConnection SQLCon = null;

        
        
        
        public BaseResultMOD getDanhSachNhomND(int page)

        {
           
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {

                List<DanhSachNhomNDMOD> dsnnd = new List<DanhSachNhomNDMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {

                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM NhomNguoiDung";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhSachNhomNDMOD item = new DanhSachNhomNDMOD();
                        item.NNDID = reader.GetInt32(0);
                        item.TenNND = reader.IsDBNull(1) ? null : reader.GetString(1);
                        item.GhiChu = reader.IsDBNull(2) ? null : reader.GetString(2);
                        dsnnd.Add(item);

                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsnnd;
                }
            }

            catch (Exception ex)
            {
                throw;

            }
            return result;
        }
        public BaseResultMOD getdsNNDPage(int page, int ProductPerPage)
        {
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<DanhSachNhomNDMOD> dssp = new List<DanhSachNhomNDMOD>();
                int totalItems = 0;
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    //count
                    SQLCon.Open();
                    var cmdCount = new SqlCommand();
                    cmdCount.CommandType = CommandType.Text;
                    cmdCount.CommandText = @"SELECT COUNT(*) AS TotalItems FROM NhomNguoiDung;";
                    cmdCount.Connection = SQLCon;
                    totalItems = (int)cmdCount.ExecuteScalar();

                    SqlCommand cmd = new SqlCommand();



                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT * FROM NhomNguoiDung
                                       ORDER BY NNDID
                                       OFFSET @StartPage ROWS  
                                       FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhSachNhomNDMOD item = new DanhSachNhomNDMOD();
                        item.NNDID = reader.GetInt32(0);
                        item.TenNND = reader.IsDBNull(1) ? null : reader.GetString(1);
                        item.GhiChu = reader.IsDBNull(2) ? null : reader.GetString(2);
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
        public BaseResultMOD ChiTietNND(int page, int ProductPerPage,int id)
        {
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<ChiTietNND> dssp = new List<ChiTietNND>();
                int totalItems = 0;
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    //count
                    SQLCon.Open();
                    var cmdCount = new SqlCommand();
                    cmdCount.CommandType = CommandType.Text;
                    cmdCount.CommandText = @"SELECT COUNT(DISTINCT CNCNND.ChucNangID) AS TotalItems
                                            FROM ChucNangCuaNhomND CNCNND
                                            LEFT JOIN NhomNguoiDung AS NND ON CNCNND.NNDID = NND.NNDID
                                            LEFT JOIN ChucNang AS CN ON CNCNND.ChucNangid = CN.ChucNangid
                                            WHERE NND.NNDID = @id;";
                    cmdCount.Parameters.AddWithValue("@id", id);

                    cmdCount.Connection = SQLCon;
                    totalItems = (int)cmdCount.ExecuteScalar();

                    SqlCommand cmd = new SqlCommand();



                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @" select distinct CNCNND.ID,CNCNND.NNDID,NND.TenNND,CNCNND.ChucNangid,CN.TenChucNang, Xem , Them,Sua , Xoa
                                        From ChucNangCuaNhomND CNCNND
                                        left join NhomNguoiDung as NND on  CNCNND.NNDID = NND.NNDID
                                        left join ChucNang as CN on CNCNND.ChucNangid = CN.ChucNangid 
										where NND.NNDID = @id
                                        ORDER BY CNCNND.ChucNangID
                                       OFFSET @StartPage ROWS  
                                       FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ChiTietNND item = new ChiTietNND();
                        item.ID = reader.GetInt32(0);
                        item.NNDID = reader.IsDBNull(1) ? null : reader.GetInt32(1);
                        item.TenNND = reader.IsDBNull(2) ? null : reader.GetString(2);

                        item.ChucNangID = reader.IsDBNull(3) ? null : reader.GetInt32(3);

                        item.TenChucNang = reader.IsDBNull(4) ? null : reader.GetString(4);

                        item.Xem = reader.IsDBNull(5) ? null : reader.GetBoolean(5);
                        item.Them = reader.IsDBNull(6) ? null : reader.GetBoolean(6);

                        item.Sua = reader.IsDBNull(7) ? null : reader.GetBoolean(7);

                        item.Xoa = reader.IsDBNull(8) ? null : reader.GetBoolean(8);


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

        public BaseResultMOD ThemNND(ThemMoiNND item)
        {
            var result = new BaseResultMOD();
            try
            {
                bool isDuplicate = KiemTraTrungNND(item);
                if (isDuplicate)
                {
                    result.Status = -1;
                    result.Message = "Tên nhóm người dùng đã tồn tại.";
                }
                else
                {
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into NhomNguoiDung (TenNND,GhiChu)VALUES(@TenNND,@GhiChu)";
                        cmd.Connection = SQLCon;
                        cmd.Parameters.AddWithValue("@TenNND", item.TenNND);
                        cmd.Parameters.AddWithValue("@GhiChu", item.GhiChu);
                        SQLCon.Open();

                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm thành công";
                        result.Data = 1;

                    }
                }
                

            }catch(Exception ex){
                result.Status = -1;
                result.Message = COM.ULT.Constant.API_Error_System;
                throw;
            }
            return result;
        }

        private bool KiemTraTrungNND(ThemMoiNND item)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();
                string checkQuery = "SELECT COUNT(*) FROM NhomNguoiDung WHERE TenNND = @TenNND";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@TenNND", item.TenNND);
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }
        public BaseResultMOD SuaNhomND(DanhSachNhomNDMOD item)
        {
            var Result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update NhomNguoiDung set TenNND = @TenNND , GhiChu = @GhiChu where NNDID=@NNDID";
                    cmd.Connection = SQLCon;
                    

                    cmd.Parameters.AddWithValue("@NNDID", item.NNDID);
                    cmd.Parameters.AddWithValue("@TenNND", item.TenNND);
                    cmd.Parameters.AddWithValue("@GhiChu", item.GhiChu);
                    SQLCon.Open();
                    cmd.ExecuteNonQuery();
                    Result.Status = 1;
                    Result.Message = "Sua thanh cong";
                    Result.Data = 1;
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = COM.ULT.Constant.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultMOD XoaNND(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using(SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from NhomNguoiDung where NNDID = @NNDID";
                    cmd.Parameters.AddWithValue("@NNDID", id);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if(rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa nhóm người dùng thành công";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = "Không tìm thấy nhóm người dùng để xóa";
                    }
                }
            }catch(Exception ex)
            {
                result.Status= -1;
                result.Message = COM.ULT.Constant.API_Error_System + ex;
                throw;

            }
            return result;
        }
      
    }
    

}

﻿using COM.MOD;
using COM.ULT;
using COM.MOD;

//using Microsoft.AspNetCore.Mvc.Infrastructure;
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
    public class ChucNangCuaNNDDAL
    {
        //private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;";
        public BaseResultMOD getDSChucNangCuaNND(int page)
        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage  * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<ChucNangCuaNNDMOD> listcncnnd = new List<ChucNangCuaNNDMOD>();
                using(SqlConnection SQLCon =  new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " Select * from ChucNangCuaNhomND ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ChucNangCuaNNDMOD item = new ChucNangCuaNNDMOD();
                        item.idChucNangCuaNND = reader.GetInt32(0);

                        item.ChucNang = reader.GetInt32(1);
                        item.NNDID = reader.GetInt32(2);
                        item.Xem = reader.GetBoolean(3);
                        item.Them = reader.GetBoolean(4);
                        item.Sua = reader.GetBoolean(5);
                        item.Xoa = reader.GetBoolean(6);
                        listcncnnd.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = listcncnnd;
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = "Lỗi hệ thống" + ex;
                throw;

            }
            return result;

        }
        public BaseResultMOD getDSChucNangCuaNND2(int page)
        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<ChucNangCuaNNDMOD2> listcncnnd = new List<ChucNangCuaNNDMOD2>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    //cmd.CommandText = " Select * from ChucNangCuaNhomND ";
                    cmd.CommandText = @"select distinct CNCNND.idChucNangCuaNND,CNCNND.NNDID,NND.TenNND,CNCNND.ChucNangid,CN.TenChucNang, Xem , Them,Sua , Xoa
                                        From ChucNangCuaNhomND CNCNND
                                        inner join NhomNguoiDung as NND on  CNCNND.NNDID = NND.NNDID
                                        inner join ChucNang as CN on CNCNND.ChucNangid = CN.ChucNangid
                                        ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ChucNangCuaNNDMOD2 item = new ChucNangCuaNNDMOD2();
                        item.idChucNangCuaNND = reader.GetInt32(0);
                        item.NNDID = reader.GetInt32(1);
                        item.TenNND = reader.GetString(2);
                        item.ChungNangid = reader.GetInt32(3);
                        item.TenChucNang = reader.GetString(4);
                        item.Xem = reader.GetBoolean(5);
                        item.Them = reader.GetBoolean(6);
                        item.Sua = reader.GetBoolean(7);
                        item.Xoa = reader.GetBoolean(8);
                        listcncnnd.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = listcncnnd;
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


        public BaseResultMOD ThemQuyenCNCNND(ThemChucNangCuaNNDMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                bool isDuplicate = KiemTraTrungChucNang(item);
                if (isDuplicate)
                {
                    result.Status = -1;
                    result.Message = "Quyền chức năng đã tồn tại.";
                }
                else
                {
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Insert into ChucNangCuaNhomND (ChucNangid, NNDID, Xem, Them, Sua, Xoa) VALUES (@ChucNangid, @NNDID, @Xem, @Them, @Sua, @Xoa)";
                        cmd.Parameters.AddWithValue("@ChucNangid", item.ChucNang);
                        cmd.Parameters.AddWithValue("@NNDID", item.NNDID);
                        cmd.Parameters.AddWithValue("@Xem", item.Xem);
                        cmd.Parameters.AddWithValue("@Them", item.Them);
                        cmd.Parameters.AddWithValue("@Sua", item.Sua);
                        cmd.Parameters.AddWithValue("@Xoa", item.Xoa);
                        cmd.Connection = SQLCon;
                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm quyền chức năng thành công";
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
        private bool KiemTraTrungChucNang(ThemChucNangCuaNNDMOD item)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();
                string checkQuery = "SELECT COUNT(*) FROM ChucNangCuaNhomND WHERE ChucNangid = @ChucNangid AND NNDID = @NNDID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@ChucNangid", item.ChucNang);
                    checkCmd.Parameters.AddWithValue("@NNDID", item.NNDID);
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }

        public BaseResultMOD SuaCNCNND(ChucNangCuaNNDMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using(SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update ChucNangCuaNhomND set ChucNangid=@ChucNangid ,NNDID=@NNDID,Xem=@Xem,Them=@Them,Sua=@Sua,Xoa=@Xoa where idChucNangCuaNND=@idChucNangCuaNND ";
                    cmd.Parameters.AddWithValue("@idChucNangCuaNND", item.idChucNangCuaNND);
                    cmd.Parameters.AddWithValue("@ChucNangid",item.ChucNang);
                    cmd.Parameters.AddWithValue("@NNDID", item.NNDID);
                    cmd.Parameters.AddWithValue("@Xem", item.Xem);
                    cmd.Parameters.AddWithValue("@Them", item.Them);
                    cmd.Parameters.AddWithValue("@Sua", item.Sua);
                    cmd.Parameters.AddWithValue("@Xoa", item.Xoa);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Sửa thành công";
                    result.Data = 1;


                }

            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD XoaCNCNND(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from ChucNangCuaNhomND where idChucNangCuaNND =@idChucNangCuaNND";
                    cmd.Parameters.AddWithValue("@idChucNangCuaNND", id);
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
                        result.Message = "ID không hợp lệ";
                    }
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }
        public ChucNangCuaNNDMOD2 ChiTietCNCN(int id)
        {
            ChucNangCuaNNDMOD2 item = null;
            try
            {
                using(SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT idChucNangCuaNND ,ND.NNDID, ND.TenNND,CN.ChucNangid, CN.TenChucNang , Xem,Them , Sua ,Xoa
                                        FROM ChucNangCuaNhomND CNND
                                        INNER JOIN NhomNguoiDung as ND ON CNND.NNDID = ND.NNDID
                                        INNER JOIN ChucNang as CN ON CNND.ChucNangid = CN.ChucNangid
                                        INNER JOIN NguoiDungTrongNhom as NDTN ON ND.NNDID = NDTN.NNDID
                                        where idChucNangCuaNND = @idChucNangCuaNND;";
                    cmd.Parameters.AddWithValue("@idChucNangCuaNND", id);
                   

                    cmd.Connection = SQLCon;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        item = new ChucNangCuaNNDMOD2();
                        item.idChucNangCuaNND = reader.GetInt32(0);
                        item.NNDID = reader.GetInt32(1);
                        item.TenNND = reader.GetString(2);
                        item.ChungNangid = reader.GetInt32(3);
                        item.TenChucNang = reader.GetString(4);

                        item.Xem = reader.GetBoolean(5);
                        item.Them = reader.GetBoolean(6);
                        item.Sua = reader.GetBoolean(7);

                        item.Xoa = reader.GetBoolean(8);

                    }
                    reader.Close();

                }
                
            }catch(Exception ex)
            {
                throw;
            }
            return item;
        }
    }
}

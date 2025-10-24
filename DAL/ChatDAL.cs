using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.MOD;
using COM.MOD.SanPham;
using COM.ULT;

namespace COM.DAL
{
    public class ChatDAL
    {
        public void LuuTinNhan(ChatMOD chat)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO ChatMessages (RoomID,FromUserID, Message, SentAt,IsFromAdmin , IsSeenByAdmin ,IsSeenByUser) VALUES (@RoomID,@FromUserID,@Message, @SentAt,@IsFromAdmin,@IsSeenByAdmin ,@IsSeenByUser)", conn);
                cmd.Parameters.AddWithValue("@RoomID", chat.RoomID);
                cmd.Parameters.AddWithValue("@FromUserID", chat.FromUserID);
                cmd.Parameters.AddWithValue("@Message", chat.Message);
                cmd.Parameters.AddWithValue("@SentAt", chat.SentAt);
                cmd.Parameters.AddWithValue("@IsFromAdmin", chat.IsFromAdmin);
                cmd.Parameters.AddWithValue("@IsSeenByAdmin", chat.IsSeenByAdmin);
                cmd.Parameters.AddWithValue("@IsSeenByUser", chat.IsSeenByUser);
                cmd.ExecuteNonQuery();
            }
        }
        public void MarkAsSeenByAdmin(int roomID)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE ChatMessages SET IsSeenByAdmin = 1 WHERE RoomID = @RoomID AND IsFromAdmin = 0", conn);
                cmd.Parameters.AddWithValue("@RoomID", roomID);
                cmd.ExecuteNonQuery();
            }
        }
        public int CreateRoom (int userID)
        {
            int roomID = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT cr.RoomID 
                                        FROM ChatRooms cr
                                        LEFT JOIN ChatRoomMembers crm on cr.RoomID = crm.RoomID
                                        WHERE UserID = @UserID";
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    object result = cmd.ExecuteScalar();
                    if(result != null && result != DBNull.Value)
                    {
                        roomID = Convert.ToInt32(result);
                        return roomID;
                    }

                }
                // không có room => tạo mới
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    SqlCommand createRoomCmd = new SqlCommand();
                    createRoomCmd.Connection = conn;

                    createRoomCmd.CommandType = CommandType.Text;
                    createRoomCmd.CommandText = @"
                    INSERT into ChatRooms (RoomName) 
                    values (@RoomName);
                    Select SCOPE_IDENTITY();"; //lấy ra giá trị vừa tăng (mới nhất) trong bảng 

                    createRoomCmd.Parameters.AddWithValue("@RoomName", $"Room_{userID}");
                    object newRoomID = createRoomCmd.ExecuteScalar();
                    roomID = Convert.ToInt32(newRoomID);
                }
                // thêm user vào room vừa tạo 
                ThemUserVaoRoom(userID, roomID);
                /*using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    SqlCommand insertUser = new SqlCommand();
                    insertUser.CommandType = CommandType.Text;
                    insertUser.CommandText = @"INSERT into ChatRoomMembers (RoomID,UserID) Values (@RoomID,@UserID)";
                    insertUser.Parameters.AddWithValue("@RoomID", roomID);
                    insertUser.Parameters.AddWithValue("@UserID", userID);
                    insertUser.ExecuteNonQuery();

                }*/
            }
            catch (Exception)
            {
                throw;
            }
            return roomID; // trả về room vừa tạo
        }
        public BaseResultMOD ThemUserCoChucNangVaoRoom(int roomID)
        {
            var result = new BaseResultMOD();
            var list = LayUserQuanLyChatChuaCoTrongRoom(roomID);
            if(list.Count > 0)
            {   
                ThemUserVaoRoom(list, roomID);
            }
            result.Status = 1;
            result.Message = "Đã thêm user quản lý chat vào room";

            return result;
        }
        public void ThemUserVaoRoom(object userIDs, int roomID)
        {
            List<int> listUserIDs = new List<int>();
            if(userIDs is int singleID)
            {
                listUserIDs.Add(singleID);
            }
            else if(userIDs is List<int> list)
            {
                listUserIDs = list;
            }else
            {
                throw new ArgumentException("Kiểu dữ liệu userIDs không hợp lệ. Phải là int hoặc List<int>.");
            }
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    foreach( var UserID in listUserIDs)
                    {
                        
                        SqlCommand insertUser = new SqlCommand();
                        insertUser.CommandType = CommandType.Text;
                        insertUser.Connection = conn;
                        insertUser.Transaction = trans;

                        insertUser.CommandText = @"INSERT into ChatRoomMembers (RoomID,UserID) Values (@RoomID,@UserID)";
                        insertUser.Parameters.AddWithValue("@RoomID", roomID);
                        insertUser.Parameters.AddWithValue("@UserID", UserID);
                        insertUser.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }


            }
        }
        public List<int> LayUserQuanLyChatChuaCoTrongRoom(int roomID)
        {
            List<int> list = new List<int>(); 
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    sqlconn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlconn;
                    //lấy ra những id của các nhóm có chức năng quản lý chat
                    cmd.CommandText = @"SELECT DISTINCT u.UserID FROM Users u
                                        LEFT JOIN NguoiDungTrongNhom NDTN on u.UserID = NDTN.UserID
                                        LEFT JOIN NhomNguoiDung NND on NDTN.NNDID = NND.NNDID
                                        LEFT JOIN ChucNangCuaNhomND CNCNND on NND.NNDID =CNCNND.NNDID
                                        LEFT JOIN ChucNang CN on CNCNND.ChucNangID = CN.ChucNangID
                                        WHERE CN.TenChucNang = 'QuanLyChat' 
                                        AND CNCNND.Xem=1
                                        AND u.UserID NOT IN (SELECT crm.UserID FROM ChatRoomMembers crm WHERE crm.RoomID = @RoomID); ";
                    cmd.Parameters.AddWithValue("@RoomID", roomID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(Convert.ToInt32(reader["UserID"]));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return list;

        }
        public BaseResultMOD getTinNhanChuaDoc(int page, int ProductPerPage)
        {
            //const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<TinNhanChuaDocMOD> dssp = new List<TinNhanChuaDocMOD>();
                int totalItems = 0;
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    //count
                    SQLCon.Open();
                    var cmdCount = new SqlCommand();
                    cmdCount.CommandType = CommandType.Text;
                    cmdCount.CommandText = @"WITH LastMessage AS (
                                            SELECT 
                                                cm.RoomID,
                                                u.FullName,
                                                cm.Message,
                                                cm.SentAt,
                                                cm.IsSeenByAdmin,
                                                ROW_NUMBER() OVER (PARTITION BY cm.RoomID ORDER BY cm.SentAt DESC, cm.ID DESC) AS RowNum
                                            FROM ChatMessages cm
                                            INNER JOIN Users u ON cm.FromUserID = u.UserID
                                        )
                                        SELECT 
                                            COUNT(*) OVER() AS TotalItem
                                        FROM LastMessage
                                        WHERE RowNum = 1 AND IsSeenByAdmin = 0
                                        ORDER BY RoomID  ;";
                    cmdCount.Connection = SQLCon;
                    totalItems = (int)cmdCount.ExecuteScalar();

                    SqlCommand cmd = new SqlCommand();



                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"
                                        WITH LastMessage AS (
                                        SELECT 
	                                    cm.ID,
                                            cm.RoomID,
                                            u.FullName,
                                            cm.Message,
                                            cm.SentAt,
		                                    cm.IsSeenByAdmin,
                                            ROW_NUMBER() OVER (PARTITION BY cm.RoomID ORDER BY cm.SentAt DESC, cm.ID DESC) AS RowNum
                                        FROM ChatMessages cm
                                        INNER JOIN Users u ON cm.FromUserID = u.UserID
                                    )
                                    SELECT ID,RoomID, FullName, Message, SentAt ,IsSeenByAdmin
                                    FROM LastMessage
                                    WHERE RowNum = 1 AND IsSeenByAdmin = 0
                                    ORDER BY ID  
                                    OFFSET @StartPage ROWS  
                                    FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TinNhanChuaDocMOD item = new TinNhanChuaDocMOD();
                        item.ID = reader.GetInt32(0);
                        item.RoomID = reader.GetInt32(1);
                        item.FullName = reader.GetString(2);
                        item.Message = reader.GetString(3);
                        item.SentAt = reader.GetDateTime(4);
                        item.IsSeenByAdmin = reader.GetBoolean(5);
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
        public BaseResultMOD getAllTinNhan(int page, int ProductPerPage)
        {
            //const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<TinNhanChuaDocMOD> dssp = new List<TinNhanChuaDocMOD>();
                int totalItems = 0;
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    //count
                    SQLCon.Open();
                    var cmdCount = new SqlCommand();
                    cmdCount.CommandType = CommandType.Text;
                    cmdCount.CommandText = @"WITH LastMessage AS (
                                            SELECT 
                                                cm.RoomID,
                                                u.FullName,
                                                cm.Message,
                                                cm.SentAt,
                                                cm.IsSeenByAdmin,
                                                ROW_NUMBER() OVER (PARTITION BY cm.RoomID ORDER BY cm.SentAt DESC, cm.ID DESC) AS RowNum
                                            FROM ChatMessages cm
                                            INNER JOIN Users u ON cm.FromUserID = u.UserID
                                        )
                                        SELECT 
                                            COUNT(*) OVER() AS TotalItem
                                        FROM LastMessage
                                        WHERE RowNum = 1
                                        ORDER BY RoomID  ;";
                    cmdCount.Connection = SQLCon;
                    totalItems = (int)cmdCount.ExecuteScalar();

                    SqlCommand cmd = new SqlCommand();



                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"
                                        WITH LastMessage AS (
                                        SELECT 
	                                    cm.ID,
                                            cm.RoomID,
                                            u.FullName,
                                            cm.Message,
                                            cm.SentAt,
		                                    cm.IsSeenByAdmin,
                                            ROW_NUMBER() OVER (PARTITION BY cm.RoomID ORDER BY cm.SentAt DESC, cm.ID DESC) AS RowNum
                                        FROM ChatMessages cm
                                        INNER JOIN Users u ON cm.FromUserID = u.UserID
                                    )
                                    SELECT ID,RoomID, FullName, Message, SentAt ,IsSeenByAdmin
                                    FROM LastMessage
                                    WHERE RowNum = 1
                                    ORDER BY ID  
                                    OFFSET @StartPage ROWS  
                                    FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TinNhanChuaDocMOD item = new TinNhanChuaDocMOD();
                        item.ID = reader.GetInt32(0);
                        item.RoomID = reader.GetInt32(1);
                        item.FullName = reader.GetString(2);
                        item.Message = reader.GetString(3);
                        item.SentAt = reader.GetDateTime(4);
                        item.IsSeenByAdmin = reader.GetBoolean(5);
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
        public BaseResultMOD getAllTinNhanRoom(int page, int ProductPerPage, int RoomID)
        {
            //const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<TinNhanRoomMOD> dssp = new List<TinNhanRoomMOD>();
                int totalItems = 0;
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    //count
                    SQLCon.Open();
                    var cmdCount = new SqlCommand();
                    cmdCount.CommandType = CommandType.Text;
                    cmdCount.CommandText = @"SELECT Distinct COUNT(*) OVER() AS TotalItem
                                            FROM ChatMessages cm
                                            INNER JOIN Users u on cm.FromUserID = u.UserID
                                            WHERE cm.RoomID = @RoomID;";
                    cmdCount.Parameters.AddWithValue("@RoomID", RoomID);
                    cmdCount.Connection = SQLCon;
                    totalItems = (int)cmdCount.ExecuteScalar();

                    SqlCommand cmd = new SqlCommand();



                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"
                                       SELECT 
                                        cm.ID,
                                        cm.RoomID,
                                        u.FullName,
                                        cm.Message,
                                        cm.SentAt,
                                        cm.IsFromAdmin,
                                        cm.IsSeenByAdmin,
                                        cm.IsSeenByUser
                                    FROM ChatMessages cm
                                    INNER JOIN Users u ON cm.FromUserID = u.UserID
                                    WHERE cm.RoomID = 5
                                    ORDER BY cm.SentAt DESC, cm.ID DESC
                                    OFFSET @StartPage ROWS  
                                    FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@RoomID", RoomID);
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TinNhanRoomMOD item = new TinNhanRoomMOD();
                        item.ID = reader.GetInt32(0);
                        item.RoomID = reader.GetInt32(1);
                        item.FullName = reader.GetString(2);
                        item.Message = reader.GetString(3);
                        item.SentAt = reader.GetDateTime(4);
                        item.IsFromAdmin = reader.GetBoolean(5);
                        item.IsSeenByAdmin = reader.GetBoolean(6);
                        item.IsSeenByUser = reader.GetBoolean(7);
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

        public List<ChatMOD> LayLichSuTinNhan(int currentUserId, bool isAdmin)
        {
            List<ChatMOD> list = new List<ChatMOD>();

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                SqlCommand cmd;

                if (isAdmin)
                {
                    cmd = new SqlCommand(@"SELECT * FROM ChatMessages ORDER BY SentAt", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"SELECT * FROM ChatMessages 
                                       WHERE FromUserID = @UserID OR ToUserID = @UserID
                                       ORDER BY SentAt", conn);
                    cmd.Parameters.AddWithValue("@UserID", currentUserId);
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ChatMOD
                        {
                            ID = reader.GetInt32(0),
                            RoomID = reader.GetInt32(1),
                            FromUserID = reader.GetInt32(2),
                            Message = reader.GetString(3),
                            SentAt = reader.GetDateTime(4)
                        });
                    }
                }
            }

            return list;
        }

    }

}

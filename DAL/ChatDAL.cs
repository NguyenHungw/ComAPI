using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.MOD;
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
                SqlCommand cmd = new SqlCommand("INSERT INTO ChatMessages (FromUserID, ToUserID, Message, SentAt) VALUES (@FromUserID,@ToUserID,@Message, @SentAt)", conn);
                cmd.Parameters.AddWithValue("@FromUserID", chat.FromUserID);
                cmd.Parameters.AddWithValue("@ToUserID", chat.ToUserID);
                cmd.Parameters.AddWithValue("@Message", chat.Message);
                cmd.Parameters.AddWithValue("@SentAt", chat.SentAt);
                cmd.ExecuteNonQuery();
            }
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
                            FromUserID = reader.GetInt32(1),
                            ToUserID = reader.GetInt32(2),
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

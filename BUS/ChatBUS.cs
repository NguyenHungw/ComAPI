using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.DAL;
using COM.MOD;
using Microsoft.AspNetCore.SignalR;
using COM;
namespace COM.BUS
{
    public class ChatBUS : Hub
    {
        private readonly ChatDAL _dal;

        

        public void XuLyVaLuuTinNhan(int nguoiGui,int nguoiNhan,string noiDung)
        {
            ChatMOD mess = new ChatMOD
            {
                FromUserID = nguoiGui,
                ToUserID = nguoiNhan,
                Message = noiDung,
                SentAt = DateTime.UtcNow,
                
            };
            _dal.LuuTinNhan(mess);
        }
    }
}

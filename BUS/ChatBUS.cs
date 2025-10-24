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
        public ChatBUS()
        {
            _dal = new ChatDAL(); // khởi tạo để dùng
        }
        public void XuLyVaLuuTinNhan(int nguoiGui,int RoomID, string noiDung)
        {
            ChatMOD mess = new ChatMOD
            {
                RoomID = RoomID,
                FromUserID = nguoiGui,
                Message = noiDung,
                SentAt = DateTime.UtcNow,
                
            };
            _dal.LuuTinNhan(mess);
        }
        public BaseResultMOD CreateChatRoomBUS(int userID)
        {
            var result = new BaseResultMOD();
            if(userID > 0) { 
                int roomID = _dal.CreateRoom(userID);
                result.Status = 1;
                result.Message = "Tạo room thành công";
                result.Data = roomID;
            }
            else
            {
                result.Status = 0;
                result.Message = "Lỗi: userID không hợp lệ.";
            }
            return result;

        }
        public BaseResultMOD InsertUserIntoRoom(int roomID)
        {
            var result = new BaseResultMOD();
            if (roomID > 0)
            {
                result = _dal.ThemUserCoChucNangVaoRoom(roomID);
                result.Status = 1;
                result.Message = "Thêm thành công";
            }
            else
            {
                result.Status = 0;
            }
            return result;  
        }
        public BaseResultMOD TinNhanChuaDocBUS (int page , int size)
        {
            var result = new BaseResultMOD();
                if(page > 0)
            {
                result.Status = 1;
                result = _dal.getTinNhanChuaDoc(page, size);
            }
            else
            {
                result.Status = 0;
            }
                return result;
        }
        public BaseResultMOD getAllTinNhanBUS(int page, int size)
        {
            var result = new BaseResultMOD();
            if (page > 0)
            {
                result.Status = 1;
                result = _dal.getAllTinNhan(page, size);
            }
            else
            {
                result.Status = 0;
            }
            return result;
        }
        public BaseResultMOD getAllTinNhanRoomBUS(int page, int size,int RoomID)
        {
            var result = new BaseResultMOD();
            if (page > 0)
            {
                result.Status = 1;
                result = _dal.getAllTinNhanRoom(page, size,RoomID);
            }
            else
            {
                result.Status = 0;
            }
            return result;
        }
    }
}

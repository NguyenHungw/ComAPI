using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using COM.BUS;
using COM.MOD;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using COM.DAL;

namespace ComAPI.Hubs
{
    public class ChatHub : Hub
    {

        private readonly ILogger<ChatHub> _logger;
        private readonly ChatDAL _chatDAL;
        public static HashSet<int> AdminOnlineRooms = new HashSet<int>();

        public async Task JoinRoom(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"room_{roomId}");
            Console.WriteLine($"[USER JOIN] {Context.ConnectionId} joined room_{roomId}");

        }
        public async Task AdminJoinRoom(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"room_{roomId}");
            AdminOnlineRooms.Add(roomId);
            Console.WriteLine($"[ADMIN VIEW] Admin đang xem room_{roomId}");
        }

        public async Task AdminLeaveRoom(int roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"room_{roomId}");
            AdminOnlineRooms.Remove(roomId);
            Console.WriteLine($"[ADMIN LEAVE] Admin rời room_{roomId}");
        }
        public async Task JoinAsAdmin()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
            Console.WriteLine($"[ADMIN JOIN] {Context.ConnectionId} joined admins group");
        }

        public async Task SendMessageToRoom(ChatMOD chat)
        {
            //Gửi message đến room
            chat.IsSeenByUser = true;
            chat.IsFromAdmin = false;
            chat.IsSeenByAdmin = false;

            new ChatDAL().LuuTinNhan(chat);

            //kiểm tra xem admin có đang xem room này ko
            if (AdminOnlineRooms.Contains(chat.RoomID))
            {
                new ChatDAL().MarkAsSeenByAdmin(chat.RoomID);
                chat.IsSeenByAdmin=true;
            }

            await Clients.Group($"room_{chat.RoomID}")
                         .SendAsync("ReceiveMessage", chat);
            //nếu admin ko xem room này thì gửi thông báo
            if (!chat.IsSeenByAdmin)
            {
                await Clients.Group("admins")
                         .SendAsync("NewMessageAlert", chat);
            }
            

        }



/*
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
            _chatDAL = new ChatDAL();
        }
*/
       /* public async Task SendMessage(int RoomID,int fromUserId, string message)
        {
            var chat = new ChatMOD
            {
                RoomID = RoomID,
                FromUserID = fromUserId,
                Message = message,
                SentAt = DateTime.Now
            };

            //  Lưu vào database
            _chatDAL.LuuTinNhan(chat);

            // Gửi realtime cho tất cả client (hoặc có thể gửi riêng)
            await Clients.All.SendAsync("ReceiveMessage", fromUserId, message, chat.SentAt);
        }*/
    }

    }

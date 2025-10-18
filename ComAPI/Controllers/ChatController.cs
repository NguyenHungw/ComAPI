using COM.BUS;
using COM.DAL;
using COM.MOD;
using ComAPI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Data.SqlClient;
using System.Security.Claims;

namespace ComAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly ChatBUS _bus;
        private readonly IHubContext<ChatHub> _hub;

        public ChatController(IConfiguration config, IHubContext<ChatHub> hub)
        {
            //_bus = new ChatBUS(config.GetConnectionString("DefaultConnection")!);
            _hub = hub;
        }
        //api đánh dấu xem room 
        [HttpPut("MarkAsRead/{roomId}")]
        [AllowAnonymous]

        public IActionResult MarkAsRead(int roomId)
        {
            new ChatDAL().MarkAsSeenByAdmin(roomId);
            return Ok(new { message = $"Marked all messages in room_{roomId} as read" });
        }

        [HttpPost("sendGR")]
        //[Authorize]
        [AllowAnonymous]

        public async Task<IActionResult> GuiTinNhanGR([FromBody] ChatMOD chat)
        {
            //kiểm tra role admin/user ở đây xong r chia ra if else
            var isAdmin = User.IsInRole("Admin");
            if(isAdmin)
            {
                chat.IsSeenByUser = false;
                chat.IsFromAdmin = true;
                chat.IsSeenByAdmin = true;
            }
            else
            {
                chat.IsSeenByUser = true;
                chat.IsFromAdmin = false;
                chat.IsSeenByAdmin = false;
            }
            new ChatDAL().LuuTinNhan(chat);
            await _hub.Clients.Group($"room_{chat.RoomID}")
                              .SendAsync("ReceiveMessage", chat);
            await _hub.Clients.Group("admins")
                       .SendAsync("NewMessageAlert", chat);

            // await _hub.Clients.All.SendAsync("ReceiveMessage", chat);

            return Ok(new { message = "Đã gửi và đẩy realtime" });
        }

        [HttpPost("send")]
        //[Authorize]
        [AllowAnonymous]

        public async Task<IActionResult> GuiTinNhan([FromBody] ChatMOD chat)
        {
            //var fromUserId = chat.FromUserID;

            //Console.WriteLine($"FromUserID: {fromUserId}");
            //var isAdmin = User.IsInRole("Admin");

/*            if (!isAdmin)
                return Forbid("Bạn chỉ được phép gửi tin nhắn đến Admin.");
*/

            //chat.FromUserID = Convert.ToInt32(fromUserId);

            new ChatDAL().LuuTinNhan(chat);

            // Gửi realtime đến admin
            await _hub.Clients.User(chat.Message.ToString()).SendAsync("ReceiveMessage", chat);

            return Ok(new { message = "Đã gửi và đẩy realtime!" });
        }
        [HttpGet("history")]
        //[Authorize]
        [AllowAnonymous]

        public IActionResult LayLichSu()
        {
            var currentUserId = int.Parse(User.FindFirstValue("ID"));
            var isAdmin = User.IsInRole("Admin");
            var messages = new ChatDAL().LayLichSuTinNhan(currentUserId, isAdmin);

            return Ok(messages);
        }
        [HttpPost("CreateRoom")]
        //[Authorize]
        [AllowAnonymous]
        public IActionResult CreateRoom(int UserID)
        {
            if (UserID < 1) return BadRequest();
            else
            {
                var Result = new ChatBUS().CreateChatRoomBUS(UserID);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpPost("InsertUser")]
        //[Authorize]
        [AllowAnonymous]

        public IActionResult InsertRoom(int RoomID)
        {
            if (RoomID < 1) return BadRequest();
            else
            {
                var Result = new ChatBUS().InsertUserIntoRoom(RoomID);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpGet("GetTinChuaDoc")]
        [AllowAnonymous]
        public IActionResult getChatMess (int page , int size)
        {
            if(page < 1) return BadRequest();
            else
            {
                var Result = new ChatBUS().TinNhanChuaDocBUS(page, size);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
    }
}

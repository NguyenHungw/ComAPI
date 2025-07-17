using COM.BUS;
using COM.DAL;
using COM.MOD;
using ComAPI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> GuiTinNhan([FromBody] ChatMOD chat)
        {
            var fromUserId = chat.FromUserID;

            Console.WriteLine($"FromUserID: {fromUserId}");
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && chat.ToUserID != 1)
                return Forbid("Bạn chỉ được phép gửi tin nhắn đến Admin.");

            chat.FromUserID = Convert.ToInt32(fromUserId);

            new ChatDAL().LuuTinNhan(chat);

            // Gửi realtime đến admin
            await _hub.Clients.User(chat.ToUserID.ToString()).SendAsync("ReceiveMessage", chat);

            return Ok(new { message = "Đã gửi và đẩy realtime!" });
        }
        [HttpGet("history")]
        [Authorize]
        public IActionResult LayLichSu()
        {
            var currentUserId = int.Parse(User.FindFirstValue("ID"));
            var isAdmin = User.IsInRole("Admin");

            var messages = new ChatDAL().LayLichSuTinNhan(currentUserId, isAdmin);

            return Ok(messages);
        }
    }
}

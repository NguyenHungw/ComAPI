using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using COM.BUS;
using COM.MOD;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComAPI.Hubs
{
    public class ChatHub : Hub
    {


        private readonly ILogger<ChatHub> _logger;
        public async Task SendMessage(string user, string message)
            => await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

}

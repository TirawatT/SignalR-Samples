using Microsoft.AspNetCore.SignalR;
using ServerSignalR.Models.Hubs;

namespace ServerSignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
    => await Clients.All.SendAsync("ReceiveMessage", user, message);

        public async Task SendMessageToCaller(string user, string message)
    => await Clients.Caller.SendAsync("ReceiveMessage", user, message);

        public async Task SendMessageToGroup(string user, string message)
            => await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message);

        public async Task ClientSetup(string user, string message)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ClientSetup", user, message);
            //await Clients.Caller.SendAsync("ClientSetup", user, message);

        }
        public async Task Test(string user, string message)
        {
            await Clients.Caller.SendAsync("Test", user, message);

        }
        public override Task OnConnectedAsync()
        {
            var data = new ChatHubDto()
            {
                Id = Context.ConnectionId,
                User = Context.ConnectionId,
                Message = "OnConnectedAsync"
            };
            //Clients.Caller.SendAsync("Connected", "aa", Context.ConnectionId );
            Clients.Caller.SendAsync("Connected", data);
            return base.OnConnectedAsync();
        }

        public object GetClient()
        {
            var a = Context;
            var res = Clients.Caller;
            return res;
        }
        public object hello()
        {
            var res = Clients.All.SendAsync("ReceiveMessage", "user", "hello");
            return res;
        }
    }
}

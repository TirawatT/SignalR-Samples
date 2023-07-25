using Microsoft.AspNetCore.SignalR;
using ServerSignalR.Hubs;

namespace ServerSignalR.Services
{
    public class ChatService
    {
        private IHubContext<ChatHub> _hub;

        public ChatService(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }
        public void Broadcast(string txt)
        {
            var result = _hub.Clients.All.SendAsync("ReceiveMessage", "server", txt);

        }
        public object GetClient()
        {
            return new object();

        }
    }
}

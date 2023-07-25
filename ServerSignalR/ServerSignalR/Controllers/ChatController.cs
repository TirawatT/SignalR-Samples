using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ServerSignalR.Hubs;
using ServerSignalR.Services;

namespace ServerSignalR.Controllers
{
    [ApiController]
    [Route("/api/hub/[controller]/[action]")]
    public class ChatController : ControllerBase
    {
        private IHubContext<ChatHub> _hub;
        private ChatService _service;

        public ChatController(IHubContext<ChatHub> hub, ChatService service)
        {
            _hub = hub;
            _service = service;
        }
        [HttpGet]
        public IActionResult GetClient()
        {
            var result = _service.GetClient();
            return Ok(result);
        }
        [HttpGet]

        public IActionResult Broadcast(string txt = "hello")
        {
            //var result = _hub.Clients.All.SendAsync("ReceiveMessage","server", txt);
            _service.Broadcast(txt);
            return Ok(new { Status = "Send hello" });
        }
    }
}
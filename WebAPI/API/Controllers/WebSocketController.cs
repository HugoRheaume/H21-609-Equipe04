using API.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using System.Web.WebSockets;

namespace API.Controllers
{
    
    public class WebSocketController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage JoinWaitingRoom()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(ProcessJoinWaitingRoom);
            }

            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);

        }
        private async Task ProcessJoinWaitingRoom(AspNetWebSocketContext context)
        {
            RoomHandler handler = new RoomHandler();
            await handler.ProcessWebSocketRequestAsync(context);
        }
    }
}
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
        [Route("websocket/connect")]
        public HttpResponseMessage Connect()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(ProcessConnection);
            }

            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);

        }
        private async Task ProcessConnection(AspNetWebSocketContext context)
        {
            Client handler = new Client();
            await handler.ProcessWebSocketRequestAsync(context);
        }
    }
}
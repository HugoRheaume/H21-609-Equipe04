using API.Models;
using API.Service;
using API.WebSocket.Command;
using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket
{
    public class Client : WebSocketHandler
    {
        private static WebSocketCollection socketClients = new WebSocketCollection();
        private AuthService service = new AuthService(new ApplicationDbContext());
        public string Username;
        public string ShareCode;
        public string Picture = "../../assets/png_64/" + Global.random.Next(1, 15).ToString() + ".png";
        public ApplicationUser connectedUser;

        public override void OnOpen()
        {
            if(!socketClients.Contains(this))
            { 
                socketClients.Add(this);
            }
            base.OnOpen();
        }
        public override void OnMessage(string message)
        {
            BaseCommand bc = Json.Decode<BaseCommand>(message);
            
            if (bc.CommandName == null || bc.CommandName == "" || !Global.CommandList.ContainsKey(bc.CommandName))
            {
                LogService.Log(this, MessageType.ErrorInvalidRequest);
                return;
            }
            if(bc.Token != null)
            {
                connectedUser = service.GetUserWithToken(bc.Token);
                if (connectedUser != null)
                {
                    this.Picture = connectedUser.Picture;
                    this.Username = connectedUser.Name;
                }
            }
            var command = Global.CommandList[bc.CommandName];
            command.Handle(message);
            command.Run(this);
         
        }
        public override void OnClose()
        {
            if (RoomService.IsRoomOwner(this.ShareCode, this.connectedUser))
            {
                RoomService.DestroyRoom(this.ShareCode);
                return;
            }

            RoomService.RemoveUser(this.ShareCode, this.Username);
            RoomService.Broadcast(this.ShareCode, MessageType.LogRoomLeft);
            socketClients.Remove(this);
            base.OnClose();
        }
        
    }
}
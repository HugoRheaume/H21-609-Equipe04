using API.Models;
using API.Service;
using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket
{
    public class RoomHandler : WebSocketHandler
    {
        private static WebSocketCollection socketClients = new WebSocketCollection();
        private string Username;
        private string ShareCode;
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
            try
            {
                WebSocketDTO ws = Json.Decode<WebSocketDTO>(message);
                switch (ws.MessageType)
                {
                    case MessageType.CreateRoom:
                        CreateRoom(message);
                        break;
                    case MessageType.JoinRoom:
                        JoinRoom(message);
                        break;                    
                    default:
                        LogService.LogError(this, ErrorType.InvalidRequest);
                        break;
                }
            }
            catch (Exception e)
            {
               LogService.Log(this, e.ToString());
            }
         
        }
        public override void OnClose()
        {
            //Is the master quizzer
            if (!RoomService.IsShareCodeExist(this.ShareCode))
                return;
            RoomService.RemoveUser(this.ShareCode, this.Username);
            RoomService.Broadcast(this.ShareCode, this.Username + " has left the lobby");
            socketClients.Remove(this);
            base.OnClose();
        }
        private void CreateRoom(string message)
        {
            CreateRoomWS ws = Json.Decode<CreateRoomWS>(message);
            this.Username = ws.Owner;
            string waitingRoomCode = RoomService.NewRoom(new RoomUser(ws.Owner, this));
            this.ShareCode = waitingRoomCode;
            ws.ShareCode = waitingRoomCode;
            LogService.Log(this, ws);
            
            //LogService.Log(this, "Waiting Room #"+waitingRoomCode+ " created");

        }
        public void JoinRoom(string message)
        {
            JoinRoomWS ws = Json.Decode<JoinRoomWS>(message);

            if(!RoomService.IsShareCodeExist(ws.ShareCode))
            {
                LogService.LogError(this, ErrorType.ShareCodeNotExist);
                return;
            }
            if(RoomService.IsUserExist(ws.ShareCode, ws.Username))
            {
                LogService.LogError(this, ErrorType.UserAlreadyJoined);
                return;
            }
            
            

            this.Username = ws.Username;
            this.ShareCode = ws.ShareCode;
            RoomUser r = new RoomUser(this.Username, this);
            RoomService.AddUser(ws.ShareCode, r);
            RoomService.Broadcast(ws.ShareCode, r.Username + " join the lobby");           
           
        }
        
    }
}
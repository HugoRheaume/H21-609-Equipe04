using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket.Command
{
    public class JoinRoomCommand: BaseCommand
    {

        public string ShareCode { get; set; }
        public string Username { get; set; }

        public JoinRoomCommand() 
        {
            this.CommandName = "Room.Join";
        }


        public override void Handle(string message)
        {
            JoinRoomCommand jrc = Json.Decode<JoinRoomCommand>(message);

            this.ShareCode = jrc.ShareCode;
            this.Username = jrc.Username;
        }
        public override void Run(Client client)
        {
            

            if (!RoomService.IsShareCodeExist(this.ShareCode))
            {
                LogService.Log(client, MessageType.ErrorShareCodeNotExist);
                return;
            }
            if (RoomService.IsUserExist(this.ShareCode, client))
            {
                LogService.Log(client, MessageType.ErrorUserAlreadyJoined);
                return;
            }
            client.ShareCode = this.ShareCode;
            
            RoomService.AddUser(this.ShareCode, client);
            RoomService.Broadcast(this.ShareCode, MessageType.LogRoomJoined);
            
        }
    }
}
using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket.Command
{
    public class LeaveRoomCommand: BaseCommand
    {
        public string ShareCode { get; set; }
        public string Username { get; set; }

        public LeaveRoomCommand()
        {
            this.CommandName = "Room.Leave";
        }


        public override void Handle(string message)
        {
            LeaveRoomCommand lrc = Json.Decode<LeaveRoomCommand>(message);

            this.ShareCode = lrc.ShareCode;
            this.Username = lrc.Username;
        }
        public override void Run(Client client)
        {


            if (!RoomService.IsShareCodeExist(this.ShareCode))
            {
                LogService.Log(client, MessageType.ErrorShareCodeNotExist);
                return;
            }
            if (!RoomService.IsUserExist(this.ShareCode, this.Username))
            {
                LogService.Log(client, MessageType.ErrorNotInRoom);
                return;
            }


            client.ShareCode = this.ShareCode;
            client.Username = this.Username;

            RoomService.RemoveUser(ShareCode, Username);
            LogService.Log(client, MessageType.LogRoomLeft);
            RoomService.Broadcast(ShareCode, MessageType.LogRoomLeft);
        }
    }
}
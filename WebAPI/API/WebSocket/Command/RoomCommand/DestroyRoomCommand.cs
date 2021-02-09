using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket.Command
{
    public class DestroyRoomCommand : BaseCommand
    {
        public string ShareCode { get; set; }
        public string Username { get; set; }
        public DestroyRoomCommand()
        {
            this.CommandName = "Room.Destroy";
        }

        public override void Handle(string message)
        {
            DestroyRoomCommand drc = Json.Decode<DestroyRoomCommand>(message);

            this.ShareCode = drc.ShareCode;
            this.Username = drc.Username;
        }

        public override void Run(Client client)
        {
            if (RoomService.IsRoomOwner(this.ShareCode, client.connectedUser))
                RoomService.DestroyRoom(this.ShareCode);
            else
                LogService.Log(client, MessageType.ErrorNotOwnerOfRoom);
        }
    }
}
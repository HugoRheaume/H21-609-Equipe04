using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket.Command
{
    public class CreateRoomCommand : BaseCommand
    {
        public string Owner { get; set; }
        public string ShareCode { get; set; }

        public CreateRoomCommand()
        {
            this.CommandName = "Room.Create";
        }


        public override void Handle(string message)
        {
            CreateRoomCommand crc =  Json.Decode<CreateRoomCommand>(message);

            this.Owner = crc.Owner;
            this.ShareCode = crc.ShareCode;
        }
        public override void Run(Client handler)
        {
            string waitingRoomCode = RoomService.NewRoom(new RoomUser(this.Owner, handler));
            this.ShareCode = waitingRoomCode;
            handler.Username = this.Owner;
            handler.ShareCode = waitingRoomCode;
            LogService.Log(handler, this);
            LogService.Log(handler, MessageType.LogRoomCreated);

            //LogService.Log(this, "Waiting Room #"+waitingRoomCode+ " created");
        }
    }
}
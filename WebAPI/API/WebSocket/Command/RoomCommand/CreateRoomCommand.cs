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
        public string ShareCode { get; set; }
        public string QuizShareCode { get; set; }


        public CreateRoomCommand()
        {
            this.CommandName = "Room.Create";
        }


        public override void Handle(string message)
        {
            CreateRoomCommand crc =  Json.Decode<CreateRoomCommand>(message);
            this.Token = crc.Token;
            this.QuizShareCode = crc.QuizShareCode;
        }
        public override void Run(Client client)
        {
            if(this.Token == null)
            {
                LogService.Log(client, MessageType.ErrorNotConnected);
                return;
            }
            if(this.QuizShareCode == null)
            {
                LogService.Log(client, MessageType.ErrorInvalidQuiz);
                return;
            }

            string waitingRoomCode = RoomService.NewRoom(client, this.QuizShareCode);
            this.ShareCode = waitingRoomCode;
            client.ShareCode = waitingRoomCode;
            LogService.Log(client, this);
            LogService.Log(client, MessageType.LogRoomCreated);            
        }
    }
}
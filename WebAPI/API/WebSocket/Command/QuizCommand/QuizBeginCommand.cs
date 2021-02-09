using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket.Command.QuizCommand
{
    public class QuizBeginCommand : BaseCommand
    {
        
        public QuizBeginCommand()
        {
            this.CommandName = "Quiz.Begin";
        }
        public override void Handle(string message)
        {
            QuizBeginCommand qbc = Json.Decode<QuizBeginCommand>(message);
            
        }

        public override void Run(Client client)
        {
            if(!RoomService.IsRoomOwner(client.ShareCode, client.connectedUser))
            {
                LogService.Log(client, MessageType.ErrorNotOwnerOfRoom);
                return;
            }
            RoomService.SetRoomDisable(client.ShareCode);
            

        }
    }
}
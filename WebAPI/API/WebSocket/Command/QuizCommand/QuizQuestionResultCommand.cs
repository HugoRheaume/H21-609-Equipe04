using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.WebSocket.Command.QuizCommand
{
    public class QuizQuestionResultCommand : BaseCommand
    {
        public QuizQuestionResultCommand()
        {
            this.CommandName = "Quiz.QuestionResult";
        }

        public override void Handle(string message)
        {
            
        }

        public override void Run(Client client)
        {
            if (!RoomService.IsRoomOwner(client.ShareCode, client.connectedUser))
            {
                LogService.Log(client, MessageType.ErrorNotOwnerOfRoom);
                return;
            }
            RoomService.GetRooms[client.ShareCode].GetRoomQuizState.IsAcceptingAnswer = false;
            LogService.LogRoom(client.ShareCode, this);
        }
    }
}
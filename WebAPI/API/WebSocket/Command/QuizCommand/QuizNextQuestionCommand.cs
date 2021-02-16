using API.Models.Question;
using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket.Command.QuizCommand
{
    public class QuizNextQuestionCommand : BaseCommand
    {
        public int questionIndex { get; set; }
        public QuestionDTO question { get; set; }
        public QuizNextQuestionCommand()
        {
            this.CommandName = "Quiz.Next";
        }
        public override void Handle(string message)
        {
            QuizNextQuestionCommand qnq = Json.Decode<QuizNextQuestionCommand>(message);
            this.questionIndex = qnq.questionIndex;
            
        }

        public override void Run(Client client)
        {
            if (!RoomService.IsRoomOwner(client.ShareCode, client.connectedUser))
            {
                LogService.Log(client, MessageType.ErrorNotOwnerOfRoom);
                return;
            }

            this.question = RoomService.GetRooms[client.ShareCode].GetRoomQuizState.NextQuestion(this.questionIndex);
            LogService.LogRoom(client.ShareCode, this);
            LogService.Log(client, this);
        }
    }
}
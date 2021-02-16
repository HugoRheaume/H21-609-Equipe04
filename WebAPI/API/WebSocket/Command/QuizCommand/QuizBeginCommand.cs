using API.Models;
using API.Models.Question;
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
        public string ShareCode { get; set; }
        public string QuizShareCode { get; set; }


        private IQuestionService service = new QuestionService(new ApplicationDbContext());
        public QuizBeginCommand()
        {
            this.CommandName = "Quiz.Begin";
        }
        public override void Handle(string message)
        {
            QuizBeginCommand qbc = Json.Decode<QuizBeginCommand>(message);
            this.ShareCode = qbc.ShareCode;
            


        }

        public override void Run(Client client)
        {
            if(!RoomService.IsRoomOwner(client.ShareCode, client.connectedUser))
            {
                LogService.Log(client, MessageType.ErrorNotOwnerOfRoom);
                return;
            }
            RoomService.SetRoomDisable(client.ShareCode);
            this.QuizShareCode = RoomService.GetQuizFromRoom(this.ShareCode).ShareCode;
            
            
            LogService.Log(client, this);
            LogService.LogRoom(this.ShareCode, this);

            QuizNextQuestionCommand qnq = new QuizNextQuestionCommand() { questionIndex = 0};
            qnq.Run(client);


        }
    }
}
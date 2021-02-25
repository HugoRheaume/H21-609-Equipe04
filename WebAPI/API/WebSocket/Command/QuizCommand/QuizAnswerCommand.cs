using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket.Command.QuizCommand
{
    public class QuizAnswerCommand : BaseCommand
    {
        //For futur use
        public int questionId { get; set; }
        public int quizId { get; set; }

        public int score { get; set; }

        public QuizAnswerCommand()
        {
            this.CommandName = "Quiz.Answer";
        }
        public override void Handle(string message)
        {
            QuizAnswerCommand qac = Json.Decode<QuizAnswerCommand>(message);
            this.score = qac.score;
        }

        public override void Run(Client client)
        {
            RoomService.GetRooms[client.ShareCode].GetRoomQuizState.UpdateScores(client.connectedUser.Token, this.score);
        }
    }
}
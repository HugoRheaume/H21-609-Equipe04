using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.WebSocket.Command.QuizCommand
{
    public class QuizScoreboardCommand : BaseCommand
    {
        public List<QuizUserScoreDTO> scores { get; set; }
        public int nbGoodAnswer { get; set; }
        public string shareCode { get; set; }
        public QuizScoreboardCommand()
        {
            this.CommandName = "Quiz.Scoreboard";
        }
        public override void Run(Client client)
        {
            scores = RoomService.GetRooms[this.shareCode].GetRoomQuizState.GetScores();
            this.nbGoodAnswer = RoomService.GetRooms[this.shareCode].GetRoomQuizState.GetNbGoodAnswer;
            LogService.Log(RoomService.GetOwner(this.shareCode), this);
        }
    }
}
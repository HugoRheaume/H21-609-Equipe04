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
        public string shareCode { get; set; }
        public QuizScoreboardCommand()
        {
            this.CommandName = "Quiz.Scoreboard";
        }
        public override void Run(Client client)
        {
            
            scores = RoomService.GetRooms[this.shareCode].GetRoomQuizState.GetScores();
            LogService.Log(RoomService.GetOwner(this.shareCode), this);
        }
    }
}
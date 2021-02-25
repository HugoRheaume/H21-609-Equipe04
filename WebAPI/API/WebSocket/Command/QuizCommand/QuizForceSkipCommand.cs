using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.WebSocket.Command.QuizCommand
{
    public class QuizForceSkipCommand : BaseCommand
    {
        public QuizForceSkipCommand()
        {
            this.CommandName = "Quiz.ForceSkip";
        }
        public override void Run(Client client)
        {
            LogService.Log(client, this);
        }
    }

}
using API.WebSocket.Command;
using API.WebSocket.Command.QuizCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace API
{
    public enum MessageType { LogRoomCreated = 200, LogRoomJoined, LogRoomLeft, LogRoomDeleted, LogRoomDisable, ErrorShareCodeNotExist = 400, ErrorNotConnected, ErrorUserAlreadyJoined, ErrorNotOwnerOfRoom, ErrorInvalidRequest, ErrorNotInRoom, ErrorInvalidToken, ErrorInvalidQuiz }
    public static class Global
    {
        public const string ALPHANUMERIC_CHARACTER_LIST = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static Random random = new Random();

        public static Dictionary<string, BaseCommand> CommandList = new Dictionary<string, BaseCommand>() 
        {
            //Better than switch case i'm telling you
            { "Room.Create", new CreateRoomCommand() },
            { "Room.Join", new JoinRoomCommand() },
            { "Room.UserState", new RoomUserStateCommand() },
            { "Room.Leave", new LeaveRoomCommand()},
            { "Room.Destroy", new DestroyRoomCommand()},
            { "Log.Message", new LogMessageCommand()},

            { "Quiz.Begin", new QuizBeginCommand()},
            { "Quiz.Next", new QuizNextQuestionCommand()},
            { "Quiz.Answer", new QuizAnswerCommand()},
            { "Quiz.Scoreboard", new QuizScoreboardCommand()},
            { "Quiz.QuestionResult", new QuizQuestionResultCommand()},
            { "Quiz.ForceSkip", new QuizForceSkipCommand()},

        };

        public static string GenerateAlphanumeric()
        {
            StringBuilder code = new StringBuilder();
            char ch;

            for (int i = 0; i < 6; i++)
            {
                ch = ALPHANUMERIC_CHARACTER_LIST[random.Next(0, ALPHANUMERIC_CHARACTER_LIST.Length)];
                code.Append(ch);
            }

            return code.ToString();
        }
    }
}
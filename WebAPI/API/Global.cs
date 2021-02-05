using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API
{
    public enum MessageType { QuizInfo, JoinRoom, CreateRoom, LogMessage, RoomSate, ErrorMessage }
    public enum ErrorType { ShareCodeNotExist, InvalidRequest, UserAlreadyJoined, RoomDeleted }
    public enum LogMessageType { RoomCreated }
    public static class Global
    {
        public const string ALPHANUMERIC_CHARACTER_LIST = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static Random random = new Random();

    }
}
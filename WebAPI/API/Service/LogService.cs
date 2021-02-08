using API.WebSocket;
using API.WebSocket.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.Service
{
    
    public static class LogService
    {
        public static void Log(Client handler, object value)
        {
            handler.Send(Json.Encode(value));
        }
        public static void LogRoom(string shareCode, object value)
        {
            throw new NotImplementedException();
        }
        public static void LogAll(object value)
        {
            throw new NotImplementedException();
        }
        public static void Log(Client handler, MessageType type)
        {
            LogMessageCommand command = new LogMessageCommand();
            command.MessageType = type;
            handler.Send(Json.Encode(command));
        }

    }
}
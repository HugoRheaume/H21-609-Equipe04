using API.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.Service
{
    
    public static class LogService
    {
        public static void Log(RoomHandler handler, string value)
        {
            handler.Send(Json.Encode(new SendLogMessage(value)));
        }
        public static void Log(RoomHandler handler, object value)
        {
            handler.Send(Json.Encode(value));
        }
        public static void Log(RoomHandler handler, LogMessageType type)
        {
            handler.Send(Json.Encode(new LogMessage(type)));
        }

        public static void LogError(RoomHandler handler, ErrorType type)
        {
            handler.Send(Json.Encode(new ErrorMessage(type)));
        }
    }
}
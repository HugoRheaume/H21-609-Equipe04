using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.WebSocket
{
    public class WebSocketDTO
    {
        public MessageType MessageType { get; set; }
    }
    public class CreateRoomWS : WebSocketDTO
    {
        public string Owner { get; set; }
        public string ShareCode { get; set; }
    }

    public class JoinRoomWS : WebSocketDTO
    {
        public string ShareCode { get; set; }
        public string Username { get; set; }

    }
    public class RoomStateWS : WebSocketDTO
    {
        public List<string> users { get; set; }
        public RoomStateWS()
        {
            this.MessageType = MessageType.RoomSate;
        }
    }
    public class SendLogMessage : WebSocketDTO
    {
        public SendLogMessage(string value)
        {
            this.Message = value;
            this.MessageType = MessageType.LogMessage;
        }
        public string Message { get; set; }
    }
    public class LogMessage: WebSocketDTO
    {
        public LogMessage(LogMessageType message)
        {
            this.MessageType = MessageType.LogMessage;
            this.LogMessageType = message;
        }
        public LogMessageType LogMessageType;
    }
    public class ErrorMessage: WebSocketDTO
    {
        public ErrorMessage(ErrorType error)
        {
            this.MessageType = MessageType.ErrorMessage;
            this.ErrorType = error;
        }
        public ErrorType ErrorType;
    }
}
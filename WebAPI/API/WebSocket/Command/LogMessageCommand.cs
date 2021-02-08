using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket.Command
{
    public class LogMessageCommand : BaseCommand
    {
        public MessageType  MessageType { get; set;}
        public string ShareCode { get; set; }

        public LogMessageCommand()
        {
            this.CommandName = "Log.Message";
        }
        public override void Handle(string message)
        {
            LogMessageCommand lmc = Json.Decode<LogMessageCommand>(message);
            this.MessageType = lmc.MessageType;
            this.ShareCode = lmc.ShareCode;

        }

        public override void Run(Client handler)
        {
            if(RoomService.IsShareCodeExist(this.ShareCode))
            {
                
                    
            }

            LogService.Log(handler, this);
        }
    }
}
using API.WebSocket;
using API.WebSocket.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            string m = JsonConvert.SerializeObject(value, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            handler.Send(m);
        }
        public static void LogRoom(string shareCode, object value)
        {
            foreach (Client item in RoomService.GetRooms[shareCode].Users)
            {
                Log(item, value);
            }
        }
        public static void LogAll(object value)
        {
            throw new NotImplementedException();
        }
        public static void Log(Client handler, MessageType type)
        {
            LogMessageCommand command = new LogMessageCommand();
            command.MessageType = type;
            string m = JsonConvert.SerializeObject(command, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            handler.Send(m);
        }

    }
}
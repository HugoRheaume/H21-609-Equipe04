using API.Models;
using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket.Command
{
    public class RoomUserStateCommand: BaseCommand
    {
        public List<UserWS> users { get; set; }

        public RoomUserStateCommand()
        {
            this.CommandName = "Room.UserState";
        }
    }
}
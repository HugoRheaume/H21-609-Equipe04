using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.WebSocket
{
    public class RoomUser
    {
        //public ApplicationUser user { get; set; }
        public string Username { get; set; }
        public ApplicationUser User { get; set; }
        public RoomHandler Handler { get; set; }

        public RoomUser(string username, RoomHandler handler)
        {
            this.Username = username;
            this.Handler = handler;
        }
    }
}
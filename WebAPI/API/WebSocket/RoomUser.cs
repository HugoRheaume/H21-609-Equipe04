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
        public UserWS User { get; set; }
        public Client Handler { get; set; }

        public RoomUser(string username, Client handler)
        {
            this.Username = username;
            this.Handler = handler;
        }
    }
}
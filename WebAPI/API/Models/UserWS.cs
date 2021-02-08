using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class UserWS
    {
        public UserWS(string username)
        {
            this.Username = username;
            this.Picture = Global.random.Next(1, 15).ToString();
        }
        public string Username { get; set; }
        public string Picture { get; set; }
    }
}
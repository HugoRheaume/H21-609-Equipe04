using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.WebSocket.Command
{

    public class BaseCommand
    {
        public string CommandName { get; set; }
        public string Token { get; set; }
        public virtual void Handle(string message) { }
        public virtual void Run(Client handler) { }
    }
}
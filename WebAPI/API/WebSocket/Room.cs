using API.Controllers;
using Microsoft.ServiceModel.WebSockets;
using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace API.WebSocket
{
    public class Room
    {
        public List<RoomUser> Users = new List<RoomUser>();

        private string m_shareCode;

        public string GetShareCode
        {
            get { return m_shareCode; }
        }
        private RoomUser m_owner;

        public RoomUser GetOwner
        {
            get { return m_owner; }
            
        }
        public RoomUser SetOwner
        {
            set { m_owner = value; }
        }



        public Room()
        {
            m_shareCode = GenerateAlphanumeric();
            
        }

        
        private string GenerateAlphanumeric()
        {
            StringBuilder code = new StringBuilder();
            char ch;
            
            for (int i = 0; i < 6; i++)
            {
                ch = Global.ALPHANUMERIC_CHARACTER_LIST[Global.random.Next(0, Global.ALPHANUMERIC_CHARACTER_LIST.Length)];
                code.Append(ch);
            }
            
            return code.ToString();
        }


    }
}
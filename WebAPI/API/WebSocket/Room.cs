using API.Controllers;
using API.Models;
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
        public List<Client> Users = new List<Client>();

        private string m_shareCode;
        public string GetShareCode
        {
            get { return m_shareCode; }
        }
        private bool m_enable;

        public bool IsEnable
        {
            get { return m_enable; }
            set { m_enable = value; }
        }
        private ApplicationUser m_owner;
        public ApplicationUser GetOwner
        {
            get { return m_owner; }
        }

        public Client handler;
        public Room(Client client, ApplicationUser owner)
        {
            m_shareCode = GenerateAlphanumeric();
            m_enable = true;
            m_owner = owner;
            handler = client;
            
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
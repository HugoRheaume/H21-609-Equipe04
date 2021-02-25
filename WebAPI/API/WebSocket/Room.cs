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

        private int m_currentIndex = 0;
        public int GetIndex
        {
            get { return m_currentIndex; }
            set { m_currentIndex = value; }
        }

        private RoomQuizState m_roomQuizState;

        public RoomQuizState GetRoomQuizState
        {
            get { return m_roomQuizState; }
        }


        public Client handler;
        public Room(Client client, Quiz quiz)
        {
            m_shareCode = Global.GenerateAlphanumeric();
            m_enable = true;
            m_owner = client.connectedUser;
            handler = client;
            m_roomQuizState = new RoomQuizState(quiz, ref this.Users, ref this.handler);
        }

        
       


    }
}
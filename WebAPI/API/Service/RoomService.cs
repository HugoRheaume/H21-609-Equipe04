using API.Models;
using API.Models.Quiz;
using API.WebSocket;
using API.WebSocket.Command;
using Microsoft.Web.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.Service
{
    public static class RoomService
    {
        private static Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        public static ApplicationDbContext db;
        public static Dictionary<string, Room> GetRooms
        { 
            get {return rooms; }
            set { rooms = value; }
        }
        //Crée une nouvelle salle d'attente dont le client est le owner
        public static string NewRoom(Client client, string quizShareCode)
        {
            Quiz q = new ApplicationDbContext().ListQuiz.FirstOrDefault(x => x.ShareCode == quizShareCode);
            if(q == null)
                return MessageType.ErrorInvalidQuiz.ToString();
            Room room = new Room(client, q);
            rooms.Add(room.GetShareCode, room);
            return room.GetShareCode;
        }
        //Ajout d'un nouveau client dans la salle d'attente spécifié
        public static void AddUser(string shareCode, Client client)
        {
            if(!IsShareCodeExist(shareCode))
            {
                LogService.Log(client, MessageType.ErrorShareCodeNotExist);
                return;
            }
            if (!rooms[shareCode].IsEnable)
            {
                LogService.Log(client, MessageType.LogRoomDisable);
                return;
            }    
            rooms[shareCode].Users.Add(client);
        }
        //Retire le client spécifié de la salle d'attente spécifié
        public static void RemoveUser(string sharecode, Client client)
        {
            if (!IsShareCodeExist(sharecode))
                return;
            rooms[sharecode].Users.Remove(client);
        }
        //Détruit la salle d'attente, disponible uniquement par le owner de la salle d'attente
        public static void DestroyRoom(string shareCode)
        {
            if(!IsShareCodeExist(shareCode))
                return;

            foreach (var item in rooms[shareCode].Users)
            {
                item.CurrentScore = 0;
                LogService.Log(item, MessageType.LogRoomDeleted);
            }
            rooms.Remove(shareCode);
        }
        //Lorsque le quiz est commencé,la salle d'attente n'est plus disponnible
        public static void SetRoomDisable(string shareCode)
        {
            rooms[shareCode].IsEnable = false;

        }
        //Pour envoyer des messages aux clients d'une salle d'attente spécifié
        public static void Broadcast(string shareCode, MessageType messageType)
        {
            WebSocketCollection clients = new WebSocketCollection();
            foreach (var u in rooms[shareCode].Users)
            {
                LogService.Log(u, messageType);
                clients.Add(u);
            }
            clients.Add(rooms[shareCode].handler);

            RoomUserStateCommand command = Global.CommandList["Room.UserState"] as RoomUserStateCommand;
            command.users = GetUsers(shareCode);

            string m = JsonConvert.SerializeObject(command, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            clients.Broadcast(m);

        }
        #region CHECKER
        //======================================================
        //Si le user est le owner de la salle d'attente
        internal static bool IsRoomOwner(string shareCode, ApplicationUser user)
        {
            if (!IsShareCodeExist(shareCode))
                return false;
            if (user == null)
                return false;
            if (rooms[shareCode].GetOwner == user)
                return true;
            else
                return false;

        }
        //Si le sharecode de la salle d'attente existe
        internal static bool IsShareCodeExist(string shareCode)
        {
            if (shareCode == null)
                return false;

            if (!rooms.ContainsKey(shareCode))
                return false;
            return true;
        }
        //Si le client existe dans une salle d'attente spécifé
        internal static bool IsUserExist(string shareCode, Client client)
        {
            if (!IsShareCodeExist(shareCode))
                return false;
            if (GetUsername(shareCode, client) == null)
                return false;
            return true;
        }
        #endregion

        #region UTIL
        //======================================================
        public static Client GetOwner(string shareCode)
        {
            return rooms[shareCode].handler;
        }
        public static Quiz GetQuizFromRoom(string shareCode)
        {
            if (!IsShareCodeExist(shareCode))
                return null;
            return rooms[shareCode].GetRoomQuizState.GetQuiz;

        }
        private static List<UserDTO> GetUsers(string shareCode)
        {
            List<UserDTO> users = new List<UserDTO>();
            foreach (var u in rooms[shareCode].Users)
            {
                users.Add(new UserDTO(u.connectedUser));
            }
            //users.Add(Rooms[shareCode].GetOwner.Username);
            return users;
        }        
        private static string GetUsername(string shareCode, Client client)
        {
            if (!IsShareCodeExist(shareCode))
                return null;
            if (client == null)
                return null;

            foreach (var item in rooms[shareCode].Users)
            {
                if (item == client)
                {
                    return item.connectedUser.Name;
                }
            }
            return null;
        }
        //======================================================
        #endregion

    }
}
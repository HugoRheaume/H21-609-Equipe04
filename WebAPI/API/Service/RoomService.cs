using API.Models;
using API.WebSocket;
using API.WebSocket.Command;
using Microsoft.Web.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        internal static Dictionary<string, Room> GetRooms
        { 
            get {return rooms; }
        }
        internal static string NewRoom(Client client, string quizShareCode)
        {

            Room room = new Room(client, new ApplicationDbContext().ListQuiz.FirstOrDefault(x => x.ShareCode == quizShareCode));
            rooms.Add(room.GetShareCode, room);
            return room.GetShareCode;
        }
        internal static void AddUser(string shareCode, Client client)
        {
            if(!rooms[shareCode].IsEnable)
            {
                LogService.Log(client, MessageType.LogRoomDisable);
                return;
            }    
            rooms[shareCode].Users.Add(client);
        }
        internal static void RemoveUser(string sharecode, Client client)
        {
            if (!IsShareCodeExist(sharecode))
                return;
            rooms[sharecode].Users.Remove(client);
        }
        internal static void DestroyRoom(string shareCode)
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
        internal static void SetRoomDisable(string shareCode)
        {
            rooms[shareCode].IsEnable = false;

        }
        internal static void Broadcast(string shareCode, MessageType messageType)
        {
            WebSocketCollection clients = new WebSocketCollection();
            foreach (var u in rooms[shareCode].Users)
            {
                LogService.Log(u, messageType);
                clients.Add(u);
            }
            clients.Add(rooms[shareCode].handler);

            //clients.Broadcast(Json.Encode(LogService.Log()));
            RoomUserStateCommand command = Global.CommandList["Room.UserState"] as RoomUserStateCommand;
            command.users = GetUsers(shareCode);

            string m = JsonConvert.SerializeObject(command, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            clients.Broadcast(m);

        }
        #region CHECKER
        //======================================================
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
        internal static bool IsShareCodeExist(string shareCode)
        {
            if (shareCode == null)
                return false;

            if (!rooms.ContainsKey(shareCode))
                return false;
            return true;
        }
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
        internal static Quiz GetQuizFromRoom(string shareCode)
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
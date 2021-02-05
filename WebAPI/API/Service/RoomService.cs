using API.WebSocket;
using Microsoft.Web.WebSockets;
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

        internal static string NewRoom(RoomUser owner)
        {
            Room room = new Room();
            room.SetOwner = owner;
            rooms.Add(room.GetShareCode, room);
            return room.GetShareCode;
        }
        internal static void AddUser(string shareCode, RoomUser roomUser)
        {
            rooms[shareCode].Users.Add(roomUser);
        }
        internal static void RemoveUser(string sharecode, string username)
        {
            RoomUser r = null;
            foreach (var item in rooms[sharecode].Users)
            {
                if (item.Username == username)
                    r = item;
            }
            rooms[sharecode].Users.Remove(r);
        }
        internal static void DeleteRoom(string shareCode)
        {
            if(!IsShareCodeExist(shareCode))
                return;

            foreach (var item in rooms[shareCode].Users)
            {
                LogService.LogError(item.Handler, ErrorType.RoomDeleted);
            }
            rooms.Remove(shareCode);
        }




        internal static void Broadcast(string value)
        {
            foreach (KeyValuePair<string, Room> room in rooms)
            {
                foreach (var u in room.Value.Users)
                {
                    u.Handler.Send(value);
                }
            }
        }
        internal static void Broadcast(string shareCode, string value)
        {
            WebSocketCollection clients = new WebSocketCollection();
            foreach (var u in rooms[shareCode].Users)
            {
                clients.Add(u.Handler);
            }
            //clients.Add(rooms[shareCode].GetOwner.Handler);

            clients.Broadcast(Json.Encode(new SendLogMessage(value.ToString())));
            RoomStateWS r = new RoomStateWS();
            r.users = GetUsers(shareCode);

            clients.Broadcast(Json.Encode(r));          
           
        }

        internal static bool IsShareCodeExist(string shareCode)
        {
            if (shareCode == null)
                return false;

            if (!rooms.ContainsKey(shareCode))
                return false;
            return true;
        }
        internal static bool IsUserExist(string shareCode, string username)
        {
            if (!IsShareCodeExist(shareCode))
                return false;
            else if (GetUsername(shareCode, username) == null)
                return false;
            else
            return true;

        }

        #region UTIL
        //======================================================
        private static List<string> GetUsers(string shareCode)
        {
            List<string> users = new List<string>();
            foreach (var u in rooms[shareCode].Users)
            {
                users.Add(u.Username);
            }
            users.Add(rooms[shareCode].GetOwner.Username);
            return users;
        }        
        private static string GetUsername(string shareCode, string username)
        {
            if (!IsShareCodeExist(shareCode))
                return null;
            if (username == null)
                return null;

            foreach (var item in rooms[shareCode].Users)
            {
                if (item.Username == username)
                {
                    return item.Username;
                }
            }
            return null;
        }
        //======================================================
        #endregion

    }
}
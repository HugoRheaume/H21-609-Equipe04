using API.Models;
using API.WebSocket;
using API.WebSocket.Command;
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
            if (!IsShareCodeExist(sharecode))
                return;

            RoomUser r = null;
            foreach (var item in rooms[sharecode].Users)
            {
                if (item.Username == username)
                    r = item;
            }
            rooms[sharecode].Users.Remove(r);
        }
        internal static void DestroyRoom(string shareCode)
        {
            if(!IsShareCodeExist(shareCode))
                return;

            foreach (var item in rooms[shareCode].Users)
            {
                LogService.Log(item.Handler, MessageType.LogRoomDeleted);
            }
            rooms.Remove(shareCode);
        }
        internal static void Broadcast(string shareCode, MessageType messageType)
        {
            WebSocketCollection clients = new WebSocketCollection();
            foreach (var u in rooms[shareCode].Users)
            {
                LogService.Log(u.Handler, messageType);
                clients.Add(u.Handler);
            }
            clients.Add(rooms[shareCode].GetOwner.Handler);

            //clients.Broadcast(Json.Encode(LogService.Log()));
            RoomUserStateCommand command = Global.CommandList["Room.UserState"] as RoomUserStateCommand;
            command.users = GetUsers(shareCode);

            clients.Broadcast(Json.Encode(command));
           
        }
        #region CHECKER
        //======================================================
        internal static bool IsRoomOwner(string shareCode, string username)
        {
            if (!IsShareCodeExist(shareCode))
                return false;
            if (username == rooms[shareCode].GetOwner.Username)
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
        internal static bool IsUserExist(string shareCode, string username)
        {
            if (!IsShareCodeExist(shareCode))
                return false;
            else if (GetUsername(shareCode, username) == null)
                return false;
            else
            return true;

        }
        #endregion
        #region UTIL
        //======================================================
        private static List<UserWS> GetUsers(string shareCode)
        {
            List<UserWS> users = new List<UserWS>();
            foreach (var u in rooms[shareCode].Users)
            {

                users.Add(new UserWS(u.Username));
            }
            //users.Add(rooms[shareCode].GetOwner.Username);
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
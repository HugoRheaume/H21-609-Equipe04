using API.Models;
using API.Service;
using API.WebSocket.Command;
using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace API.WebSocket
{
    public class Client : WebSocketHandler
    {
        private static WebSocketCollection socketClients = new WebSocketCollection();
        private AuthService service = new AuthService(new ApplicationDbContext());
        public int CurrentScore = 0;
        public bool IsAnswer = false;
        public string ShareCode;        
        public ApplicationUser connectedUser;

        public override void OnOpen()
        {
            if(!socketClients.Contains(this))
            { 
                socketClients.Add(this);
            }
            base.OnOpen();
        }
        public override void OnMessage(string message)
        {
            BaseCommand bc = Json.Decode<BaseCommand>(message);
            //Regarde si la commande est valide
            if (bc.CommandName == null || bc.CommandName == "" || !Global.CommandList.ContainsKey(bc.CommandName))
            {
                LogService.Log(this, MessageType.ErrorInvalidRequest);
                return;
            }
            //Si le tokent est présent dans la requête
            if (bc.Token == null)
            {
                LogService.Log(this, MessageType.ErrorInvalidToken);
                return;
            }
            //Get l'utilisateur dans la database selon le token
            connectedUser = service.GetUserWithToken(bc.Token);
            if (connectedUser == null)
            {
                LogService.Log(this, MessageType.ErrorInvalidToken);
                return;
            }
            //Cast de la commande selon le nom de la commande
            var command = Global.CommandList[bc.CommandName];
            //Execuition de la commande
            command.Handle(message);
            command.Run(this);
         
        }
        public override void OnClose()
        {
            if(!RoomService.IsShareCodeExist(this.ShareCode))
            {
                socketClients.Remove(this);
                base.OnClose();
                return;
            }
            if (RoomService.IsRoomOwner(this.ShareCode, this.connectedUser))
            {
                RoomService.DestroyRoom(this.ShareCode);
                return;
            }
            //Lorsqu'un client perd la connection il est retiré de la salle d'attente/quiz en direct
            RoomService.RemoveUser(this.ShareCode, this);
            RoomService.Broadcast(this.ShareCode, MessageType.LogRoomLeft);
            socketClients.Remove(this);
            base.OnClose();
        }
        
    }
}
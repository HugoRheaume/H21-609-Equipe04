using API;
using API.Models;
using API.Models.Question;
using API.Models.Quiz;
using API.Service;
using API.WebSocket;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class RoomServiceTest
    {
        private QuizService service;
        private Client client;
        private int quizId;

        //S'execute avant tous les tests
        [TestInitialize]
        public void Setup()
        {
            ApplicationUser u = new ApplicationUser() { Id = "0", Name = "User1", Token = "1234567890" };
            client = new Client() { connectedUser = u };
            service = new QuizService(new ApplicationDbContext());
            
            if(!service.CheckCodeExist("123123"))
            {
                Quiz quiz = new Quiz() { ShareCode = "123123", Date = DateTime.Now, Id = 0, Description = "", OwnerId = "0", IsPublic = false, Title = "Un Quiz", ListQuestions = new List<Question>() };
                quizId = quiz.Id;
                service.CreateQuiz(quiz, "User Test");
            }
            else
            {
                quizId = service.GetQuizByShareCode("123123").Id;
            }
        }
        //Création et destruction d'une salle d'attente valide
        [TestMethod]
        public void RoomCreateAndDeleteValid()
        {
            string sharecode = RoomService.NewRoom(client, "123123");
            Assert.AreEqual(6, sharecode.Length);
            Assert.AreEqual(1, RoomService.GetRooms.Count);

            RoomService.DestroyRoom(sharecode);

            Assert.AreEqual(0, RoomService.GetRooms.Count);
          
        }
        //Création d'une salle d'attente avec un client et test si le client est encore dans la salle d'attente
        [TestMethod]
        public void RoomCreateAndDeleteWithUser()
        {
            string sharecode = RoomService.NewRoom(client, "123123");
            ApplicationUser u = new ApplicationUser() { Id = "1", Name = "User2", Token = "0987654321" };
            Client client2 = new Client() { connectedUser = u };
            RoomService.AddUser(sharecode, client2);
            RoomService.DestroyRoom(sharecode);

            Assert.AreEqual(0, RoomService.GetRooms.Count);
            Assert.AreEqual(null, client2.ShareCode);
        }
        //Crée une salle d'attente et essaye de la détruire avec un share non existant
        [TestMethod]
        public void RoomCreateAndDeleteInvalid()
        {   
            string sharecode = RoomService.NewRoom(client, "123123");
            Assert.AreEqual(6, sharecode.Length);
            Assert.AreEqual(1, RoomService.GetRooms.Count);

            RoomService.DestroyRoom("");
            Assert.AreEqual(1, RoomService.GetRooms.Count);

            RoomService.DestroyRoom(sharecode);
            Assert.AreEqual(0, RoomService.GetRooms.Count);

        }
        //Test de création d'une salle d'attente invalide (sharecode d'un quiz non existant)
        [TestMethod]
        public void RoomCreateInvalid()
        {
            string sharecode = RoomService.NewRoom(client, "123");

            Assert.AreEqual(MessageType.ErrorInvalidQuiz.ToString(), sharecode);
            Assert.AreEqual(0, RoomService.GetRooms.Count);
        }
        //Test de création d'une salle d'attente invalide (sharecode d'un quiz non existant)
        [TestMethod]
        public void RoomCreateInvalid2()
        {
            string sharecode = RoomService.NewRoom(client, null);

            Assert.AreEqual(MessageType.ErrorInvalidQuiz.ToString(), sharecode);
            Assert.AreEqual(0, RoomService.GetRooms.Count);
        }

        //Rejoindre une room avec un sharecode valide
        [TestMethod]
        public void RoomAddUserValid()
        {
            
            string sharecode = RoomService.NewRoom(client, "123123");

            RoomService.AddUser(sharecode, client);

            Assert.AreEqual(1, RoomService.GetRooms[sharecode].Users.Count);
            Assert.AreEqual("User1", RoomService.GetRooms[sharecode].GetOwner.Name);
            Assert.AreEqual("User1", RoomService.GetRooms[sharecode].Users[0].connectedUser.Name);
            
        }
        //Test si on peut rejoindre avec un sharecode invalide
        [TestMethod]
        public void RoomAddUserInvalid()
        {
            
            string sharecode = RoomService.NewRoom(client, "123123");

            RoomService.AddUser("000000", client);

            Assert.AreEqual(0, RoomService.GetRooms[sharecode].Users.Count);
            
        }
        //Test si on peut rejoindre lorsque la room est disable
        [TestMethod]
        public void RoomDisable()
        {

            string sharecode = RoomService.NewRoom(client, "123123");
            RoomService.SetRoomDisable(sharecode);

            ApplicationUser u = new ApplicationUser() { Id = "1", Name = "User2", Token = "0987654321" };
            Client client2 = new Client() { connectedUser = u };
            RoomService.AddUser(sharecode, client2);
            
            Assert.AreEqual(0, RoomService.GetRooms[sharecode].Users.Count);
            

        }
        //S'execute après tout les tests
        [TestCleanup]
        public void Clean()
        {
            RoomService.GetRooms = new Dictionary<string, Room>();
            service.DeleteQuiz(quizId);
        }
    }
}

using System;
using System.Collections.Generic;
using API.Models;
using API.Models.Question;
using API.Models.Quiz;
using API.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class QuizServiceTest
    {
        [TestInitialize]
        public void Init()
        {
        }

        /// <summary>
        /// Test avec quiz fonctionnel qui se crée
        /// </summary>
        [TestMethod]
        public void TestServiceCreateQuiz()
        {
            Mock<QuizService> quizServiceMock = new Mock<QuizService> { CallBase = true };

            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test titre",
                Description = "test description",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };
            quiz.ListQuestions.Add(new Question());
            QuizResponseDTO responseExpected = new QuizResponseDTO
            {
                Id = 1,
                Author = "Tester",
                Title = "Test titre",
                Description = "test description",
                IsPublic = true,
                ShareCode = "",
                Date = DateTime.Now,
                NumberOfQuestions = 1
            };

            // Setup

            quizServiceMock.Setup(q => q.AddQuiz(It.IsAny<Quiz>())).Returns(quiz);
            QuizResponseDTO responseActual = quizServiceMock.Object.CreateQuiz(quiz, "Tester");

            // Vérifications

            quizServiceMock.Verify(q => q.AddQuiz(It.IsAny<Quiz>()));
            Assert.AreEqual(responseExpected.Id , responseActual.Id);
            Assert.AreEqual(responseExpected.Title , responseActual.Title);
            Assert.AreEqual(responseExpected.Description , responseActual.Description);
            Assert.AreEqual(responseExpected.NumberOfQuestions , responseActual.NumberOfQuestions);

        }

        /// <summary>
        /// Test quiz avec title trop long (plus de 250 caractères)
        /// </summary>
        [TestMethod]
        public void TestServiceCreateQuizTitleErrorTooLong()
        {
            Mock<QuizService> quizServiceMock = new Mock<QuizService> { CallBase = true };
            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quizTest quiz",
                Description = "Test quiz",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };

            // Vérification

            Assert.ThrowsException<TitleTooLong>(() => quizServiceMock.Object.CreateQuiz(quiz, ""));
        }

        /// <summary>
        /// Test quiz avec title trop court (moins de 4 caractères)
        /// </summary>
        [TestMethod]
        public void TestServiceCreateQuizTitleErrorTooShort()
        {
            Mock<QuizService> quizServiceMock = new Mock<QuizService> { CallBase = true };
            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "",
                Description = "Test quiz",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };

            // Vérification

            Assert.ThrowsException<TitleTooShort>(() => quizServiceMock.Object.CreateQuiz(quiz, ""));
        }

        /// <summary>
        /// Test quiz avec description trop longue (plus de 1000 caractères)
        /// </summary>
        [TestMethod]
        public void TestServiceCreateQuizDescriptionErrorTooLong()
        {
            Mock<QuizService> quizServiceMock = new Mock<QuizService> { CallBase = true };
            // 1001 caractères
            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test description",
                Description = "descriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescription",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };
            Assert.ThrowsException<DescriptionIsTooLong>(() => quizServiceMock.Object.CreateQuiz(quiz, ""));
        }

        [TestMethod]
        public void TestServiceCreateQuizQuizNull()
        {
            // Test si le quiz est null
            Mock<QuizService> quizServiceMock = new Mock<QuizService> { CallBase = true };
            Assert.ThrowsException<NullReferenceException>(() => quizServiceMock.Object.CreateQuiz(null, ""));
        }

        /// <summary>
        /// Test si le username est null
        /// </summary>
        [TestMethod]
        public void TestServiceCreateQuizUsernameNull()
        {

            Mock<QuizService> quizServiceMock = new Mock<QuizService> { CallBase = true };
            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test description",
                Description = "descriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescriptiondescription",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };
            Assert.ThrowsException<NullReferenceException>(() => quizServiceMock.Object.CreateQuiz(quiz, null));
        }

        /// <summary>
        /// Teste la modification d'un quiz avec des paramètres valides.
        /// </summary>
        [TestMethod]
        public void TestUpdateQuizOK()
        {
            Mock<QuizService> mock = new Mock<QuizService> { CallBase = true };

            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test titre",
                Description = "test description",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };
            quiz.ListQuestions.Add(new Question());
            mock.Setup(q => q.AddQuiz(It.IsAny<Quiz>())).Returns(quiz);
            mock.Setup(s => s.getQuizByIdInDB(It.IsAny<int>())).Returns(quiz);
            mock.Setup(s => s.saveQuiz()).Returns(true);

            QuizModifyDTO modified = new QuizModifyDTO()
            {
                Id = 1,
                Description = "Nouvelle description",
                IsPublic = false,
                Title = "Nouveau titre"
            };

            bool res = mock.Object.ModifyQuiz(modified);
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Teste la modification d'un quiz avec un paramètre nul.
        /// </summary>
        [TestMethod]
        public void TestUpdateQuizNull()
        {
            Mock<QuizService> mock = new Mock<QuizService> { CallBase = true };

            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test titre",
                Description = "test description",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };
            quiz.ListQuestions.Add(new Question());
            mock.Setup(q => q.AddQuiz(It.IsAny<Quiz>())).Returns(quiz);
            mock.Setup(s => s.getQuizByIdInDB(It.IsAny<int>())).Returns(quiz);
            mock.Setup(s => s.saveQuiz()).Returns(true);

            QuizModifyDTO modified = null;

            bool res = mock.Object.ModifyQuiz(modified);
            Assert.IsFalse(res);
        }

        /// <summary>
        /// Teste la modification d'un quiz avec une description et un titre nuls.
        /// </summary>
        [TestMethod]
        public void TestUpdateQuizNullParameters()
        {
            Mock<QuizService> mock = new Mock<QuizService> { CallBase = true };

            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test titre",
                Description = "test description",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };
            quiz.ListQuestions.Add(new Question());
            mock.Setup(q => q.AddQuiz(It.IsAny<Quiz>())).Returns(quiz);
            mock.Setup(s => s.getQuizByIdInDB(It.IsAny<int>())).Returns(quiz);
            mock.Setup(s => s.saveQuiz()).Returns(true);

            QuizModifyDTO modified = new QuizModifyDTO()
            {
                Description = null,
                Title = null
            };

            bool res = mock.Object.ModifyQuiz(modified);
            Assert.IsFalse(res);
        }

        /// <summary>
        /// Teste la modification d'un quiz avec une description vide où seulement avec des espaces blancs.
        /// </summary>
        [TestMethod]
        public void TestUpdateQuizDescriptionTooShort()
        {
            Mock<QuizService> mock = new Mock<QuizService> { CallBase = true };

            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test titre",
                Description = "test description",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };
            quiz.ListQuestions.Add(new Question());
            mock.Setup(q => q.AddQuiz(It.IsAny<Quiz>())).Returns(quiz);
            mock.Setup(s => s.getQuizByIdInDB(It.IsAny<int>())).Returns(quiz);
            mock.Setup(s => s.saveQuiz()).Returns(true);

            //Description vide
            QuizModifyDTO modified1 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "",
                IsPublic = false,
                Title = "Nouveau titre"
            };
            //Description seulement avec des espaces
            QuizModifyDTO modified2 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "                                       ",
                IsPublic = false,
                Title = "Nouveau titre"
            };

            bool res1 = mock.Object.ModifyQuiz(modified1);
            bool res2 = mock.Object.ModifyQuiz(modified2);
            Assert.AreEqual(res1, res2);
        }

        /// <summary>
        /// Teste la modification d'un quiz avec une description trop longue.
        /// </summary>
        [TestMethod]
        public void TestUpdateQuizDescriptionTooLong()
        {
            Mock<QuizService> mock = new Mock<QuizService> { CallBase = true };

            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test titre",
                Description = "test description",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };
            quiz.ListQuestions.Add(new Question());
            mock.Setup(q => q.AddQuiz(It.IsAny<Quiz>())).Returns(quiz);
            mock.Setup(s => s.getQuizByIdInDB(It.IsAny<int>())).Returns(quiz);
            mock.Setup(s => s.saveQuiz()).Returns(true);

            //Description avec 1000 caracères
            QuizModifyDTO modified1 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT",
                IsPublic = false,
                Title = "Nouveau titre"
            };
            //Description avec 1001 caractères
            QuizModifyDTO modified2 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT",
                IsPublic = false,
                Title = "Nouveau titre"
            };
            //Description avec 999 caractères
            QuizModifyDTO modified3 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT",
                IsPublic = false,
                Title = "Nouveau titre"
            };

            bool res1 = mock.Object.ModifyQuiz(modified1);
            bool res2 = mock.Object.ModifyQuiz(modified2);
            bool res3 = mock.Object.ModifyQuiz(modified3);
            Assert.IsTrue(modified1.Description.Length == 1000);
            Assert.IsTrue(modified2.Description.Length == 1001);
            Assert.IsTrue(modified3.Description.Length == 999);

            Assert.IsTrue(res1);
            Assert.IsTrue(res3);
            Assert.IsFalse(res2);
        }

        /// <summary>
        /// Teste la modification d'un quiz avec un titre trop court où seulement avec des espaces blancs.
        /// </summary>
        [TestMethod]
        public void TestUpdateQuizTitleTooShort()
        {
            Mock<QuizService> mock = new Mock<QuizService> { CallBase = true };

            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test titre",
                Description = "test description",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };
            quiz.ListQuestions.Add(new Question());
            mock.Setup(q => q.AddQuiz(It.IsAny<Quiz>())).Returns(quiz);
            mock.Setup(s => s.getQuizByIdInDB(It.IsAny<int>())).Returns(quiz);
            mock.Setup(s => s.saveQuiz()).Returns(true);

            //Titre avec 3 caracères
            QuizModifyDTO modified1 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "Nouvelle description",
                IsPublic = false,
                Title = "TTT"
            };
            //Titre avec 4 caractères
            QuizModifyDTO modified2 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "Nouvelle description",
                IsPublic = false,
                Title = "TTTT"
            };
            //Description avec 5 caractères
            QuizModifyDTO modified3 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "Nouvelle description",
                IsPublic = false,
                Title = "TTTTT"
            };
            //Description avec 0 caractères
            QuizModifyDTO modified4 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "Nouvelle description",
                IsPublic = false,
                Title = ""
            };
            //Description avec des espaces blancs
            QuizModifyDTO modified5 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "Nouvelle description",
                IsPublic = false,
                Title = "                        "
            };

            bool res1 = mock.Object.ModifyQuiz(modified1);
            bool res2 = mock.Object.ModifyQuiz(modified2);
            bool res3 = mock.Object.ModifyQuiz(modified3);
            bool res4 = mock.Object.ModifyQuiz(modified4);
            bool res5 = mock.Object.ModifyQuiz(modified5);
            Assert.IsTrue(modified1.Title.Length == 3);
            Assert.IsTrue(modified2.Title.Length == 4);
            Assert.IsTrue(modified3.Title.Length == 5);

            Assert.IsFalse(res1);
            Assert.IsTrue(res2);
            Assert.IsTrue(res3);
            Assert.IsFalse(res4);
            Assert.IsFalse(res5);
        }

        /// <summary>
        /// Teste la modification d'un quiz avec un titre trop long.
        /// </summary>
        [TestMethod]
        public void TestUpdateQuizTitleTooLong()
        {
            Mock<QuizService> mock = new Mock<QuizService> { CallBase = true };

            Quiz quiz = new Quiz
            {
                Id = 1,
                Title = "Test titre",
                Description = "test description",
                IsPublic = true,
                OwnerId = "1",
                ShareCode = "",
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };
            quiz.ListQuestions.Add(new Question());
            mock.Setup(q => q.AddQuiz(It.IsAny<Quiz>())).Returns(quiz);
            mock.Setup(s => s.getQuizByIdInDB(It.IsAny<int>())).Returns(quiz);
            mock.Setup(s => s.saveQuiz()).Returns(true);

            //Titre avec 249 caracères
            QuizModifyDTO modified1 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "Nouvelle description",
                IsPublic = false,
                Title = "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT"
            };
            //Titre avec 250 caractères
            QuizModifyDTO modified2 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "Nouvelle description",
                IsPublic = false,
                Title = "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT"
            };
            //Description 251 caractères
            QuizModifyDTO modified3 = new QuizModifyDTO()
            {
                Id = 1,
                Description = "Nouvelle description",
                IsPublic = false,
                Title = "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT"
            };

            bool res1 = mock.Object.ModifyQuiz(modified1);
            bool res2 = mock.Object.ModifyQuiz(modified2);
            bool res3 = mock.Object.ModifyQuiz(modified3);
            Assert.IsTrue(modified1.Title.Length == 249);
            Assert.IsTrue(modified2.Title.Length == 250);
            Assert.IsTrue(modified3.Title.Length == 251);

            Assert.IsTrue(res1);
            Assert.IsTrue(res2);
            Assert.IsFalse(res3);
        }

    }
}

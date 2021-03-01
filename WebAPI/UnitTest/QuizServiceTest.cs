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
        /// <summary>
        /// Test avec quiz fonctionnel qui se crée
        /// </summary>
        [TestMethod]
        public void TestServiceCreateQuiz()
        {
            Mock<QuizService> quizServiceMock = new Mock<QuizService> {CallBase = true};
            
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
            Mock<QuizService> quizServiceMock = new Mock<QuizService> {CallBase = true};
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
            Mock<QuizService> quizServiceMock = new Mock<QuizService> {CallBase = true};
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
            Mock<QuizService> quizServiceMock = new Mock<QuizService> {CallBase = true};
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
            Mock<QuizService> quizServiceMock = new Mock<QuizService> {CallBase = true};
            Assert.ThrowsException<NullReferenceException>(() => quizServiceMock.Object.CreateQuiz(null, ""));
        }

        /// <summary>
        /// Test si le username est null
        /// </summary>
        [TestMethod]
        public void TestServiceCreateQuizUsernameNull()
        {

            Mock<QuizService> quizServiceMock = new Mock<QuizService> {CallBase = true};
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
    }
}

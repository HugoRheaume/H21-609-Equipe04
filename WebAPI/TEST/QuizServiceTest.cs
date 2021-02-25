using System;
using System.Collections.Generic;
using API.Models;
using API.Models.Question;
using API.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class QuizServiceTest
    {

        [TestMethod]
        public void TestCreateService()
        {
            Mock<QuizService> quizServiceMock = new Mock<QuizService> {CallBase = true};
            // 1001 caractères
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
            QuizResponseDTO responseExprected = new QuizResponseDTO
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

            quizServiceMock.Setup(q => q.AddQuiz(It.IsAny<Quiz>())).Returns(quiz);
            QuizResponseDTO responseActual = quizServiceMock.Object.CreateQuiz(quiz, "Tester");

            quizServiceMock.Verify(q => q.AddQuiz(It.IsAny<Quiz>()));
            Assert.AreEqual(responseExprected.Id , responseActual.Id);
            Assert.AreEqual(responseExprected.Title , responseActual.Title);
            Assert.AreEqual(responseExprected.Description , responseActual.Description);
            Assert.AreEqual(responseExprected.NumberOfQuestions , responseActual.NumberOfQuestions);

        }

        [TestMethod]
        public void TestCreateServiceErrorTooLong()
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
            Assert.ThrowsException<TitleTooLong>(() => quizServiceMock.Object.CreateQuiz(quiz, ""));
        }

        
        [TestMethod]
        public void TestCreateServiceErrorTooShort()
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
            Assert.ThrowsException<TitleTooShort>(() => quizServiceMock.Object.CreateQuiz(quiz, ""));
        }

        [TestMethod]
        public void TestCreateServiceErrorDescriptionTooLong()
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


    }
}

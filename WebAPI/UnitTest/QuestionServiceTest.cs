using API.Models;
using API.Models.Question;
using API.Models.Quiz;
using API.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class QuestionServiceTest
    {
        Quiz quiz;

        [TestInitialize]
        public void TestInitialize()
        {
            quiz = new Quiz()
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
        }

        #region Unit test marco

        [TestMethod]
        public void TestCreateQuestion()
        {
            Mock<QuestionService> mock = new Mock<QuestionService> { CallBase = true };
            List<QuestionTrueFalse> questionTrueFalses = new List<QuestionTrueFalse>();
            questionTrueFalses.Add(new QuestionTrueFalse() { Id = 1, Answer = true, QuestionId = 1 });

            QuestionCreateDTO questionCreate = new QuestionCreateDTO()
            {
                Label = "Test Énoncé!",
                QuestionType = QuestionType.TrueFalse,
                TimeLimit = 20,
                QuizId = quiz.Id
            };

            Question question = new Question()
            {
                Id = 1,
                Label = questionCreate.Label,
                QuestionType = questionCreate.QuestionType,
                TimeLimit = questionCreate.TimeLimit,
                Quiz = quiz,
                QuestionTrueFalse = questionTrueFalses
            };

            quiz.ListQuestions.Add(question);

            mock.Setup(q => q.AddQuestionToDB(It.IsAny<Question>())).Returns(question);
            mock.Setup(q => q.GetQuizById(It.IsAny<int>())).Returns(quiz);

            QuestionDTO responseActual = mock.Object.AddQuestion(questionCreate);

            mock.Verify(q => q.AddQuestionToDB(It.IsAny<Question>()));
            mock.Verify(q => q.GetQuizById(It.IsAny<int>()));

            Assert.AreEqual("Test Énoncé!", responseActual.Label);
            Assert.AreEqual(20, responseActual.TimeLimit);
            Assert.AreEqual(QuestionType.TrueFalse, responseActual.QuestionType);
            Assert.AreEqual(1, responseActual.QuizId);
        }

        [TestMethod]
        public void TestCreateQuestionNoLabel()
        {
            Mock<QuestionService> mock = new Mock<QuestionService> { CallBase = true };
            List<QuestionTrueFalse> questionTrueFalses = new List<QuestionTrueFalse>();
            questionTrueFalses.Add(new QuestionTrueFalse() { Id = 1, Answer = true, QuestionId = 1 });

            QuestionCreateDTO questionCreate = new QuestionCreateDTO()
            {
                Label = "",
                QuestionType = QuestionType.TrueFalse,
                TimeLimit = 20,
                QuizId = quiz.Id
            };

            Question question = new Question()
            {
                Id = 1,
                Label = questionCreate.Label,
                QuestionType = questionCreate.QuestionType,
                TimeLimit = questionCreate.TimeLimit,
                Quiz = quiz,
                QuestionTrueFalse = questionTrueFalses
            };

            mock.Setup(q => q.GetQuizById(It.IsAny<int>())).Returns(quiz);
            Assert.ThrowsException<LabelNeeded>(() => mock.Object.AddQuestion(questionCreate));

        }

        [TestMethod]
        public void TestCreateQuestionLabelTooLong()
        {
            Mock<QuestionService> mock = new Mock<QuestionService> { CallBase = true };
            List<QuestionTrueFalse> questionTrueFalses = new List<QuestionTrueFalse>();
            questionTrueFalses.Add(new QuestionTrueFalse() { Id = 1, Answer = true, QuestionId = 1 });

            QuestionCreateDTO questionCreate = new QuestionCreateDTO()
            {
                Label = "Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé! Test Énoncé!  Test Énoncé! Test Énoncé!  Test Énoncé!!!!",
                QuestionType = QuestionType.TrueFalse,
                TimeLimit = 20,
                QuizId = quiz.Id
            };

            Question question = new Question()
            {
                Id = 1,
                Label = questionCreate.Label,
                QuestionType = questionCreate.QuestionType,
                TimeLimit = questionCreate.TimeLimit,
                Quiz = quiz,
                QuestionTrueFalse = questionTrueFalses
            };

            mock.Setup(q => q.GetQuizById(It.IsAny<int>())).Returns(quiz);
            Assert.ThrowsException<LabelTooLong>(() => mock.Object.AddQuestion(questionCreate));

        }

        [TestMethod]
        public void TestCreateQuestionTimeLimitUnder0()
        {
            Mock<QuestionService> mock = new Mock<QuestionService> { CallBase = true };
            List<QuestionTrueFalse> questionTrueFalses = new List<QuestionTrueFalse>();
            questionTrueFalses.Add(new QuestionTrueFalse() { Id = 1, Answer = true, QuestionId = 1 });

            QuestionCreateDTO questionCreate = new QuestionCreateDTO()
            {
                Label = "Test Énoncé!",
                QuestionType = QuestionType.TrueFalse,
                TimeLimit = -2,
                QuizId = quiz.Id
            };

            Question question = new Question()
            {
                Id = 1,
                Label = questionCreate.Label,
                QuestionType = questionCreate.QuestionType,
                TimeLimit = questionCreate.TimeLimit,
                Quiz = quiz,
                QuestionTrueFalse = questionTrueFalses
            };

            mock.Setup(q => q.GetQuizById(It.IsAny<int>())).Returns(quiz);
            Assert.ThrowsException<TimeNotInRange>(() => mock.Object.AddQuestion(questionCreate));

        }

        [TestMethod]
        public void TestCreateQuestionTimeLimitMore3600()
        {
            Mock<QuestionService> mock = new Mock<QuestionService> { CallBase = true };
            List<QuestionTrueFalse> questionTrueFalses = new List<QuestionTrueFalse>();
            questionTrueFalses.Add(new QuestionTrueFalse() { Id = 1, Answer = true, QuestionId = 1 });

            QuestionCreateDTO questionCreate = new QuestionCreateDTO()
            {
                Label = "Test Énoncé!",
                QuestionType = QuestionType.TrueFalse,
                TimeLimit = 3601,
                QuizId = quiz.Id
            };

            Question question = new Question()
            {
                Id = 1,
                Label = questionCreate.Label,
                QuestionType = questionCreate.QuestionType,
                TimeLimit = questionCreate.TimeLimit,
                Quiz = quiz,
                QuestionTrueFalse = questionTrueFalses
            };

            mock.Setup(q => q.GetQuizById(It.IsAny<int>())).Returns(quiz);
            Assert.ThrowsException<TimeNotInRange>(() => mock.Object.AddQuestion(questionCreate));

        }

        [TestMethod]
        public void TestCreateQuestionNoQuestionType()
        {
            Mock<QuestionService> mock = new Mock<QuestionService> { CallBase = true };
            List<QuestionTrueFalse> questionTrueFalses = new List<QuestionTrueFalse>();

            QuestionCreateDTO questionCreate = new QuestionCreateDTO()
            {
                Label = "Test Énoncé!",
                TimeLimit = 20,
                QuizId = quiz.Id
            };

            Question question = new Question()
            {
                Id = 1,
                Label = questionCreate.Label,
                TimeLimit = questionCreate.TimeLimit,
                Quiz = quiz,
            };

            mock.Setup(q => q.GetQuizById(It.IsAny<int>())).Returns(quiz);
            Assert.ThrowsException<NoQuestionType>(() => mock.Object.AddQuestion(questionCreate));

        }

        #endregion

    }
}

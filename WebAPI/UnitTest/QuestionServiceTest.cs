using API.Models;
using API.Models.Question;
using API.Service;
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
    public class QuestionServiceTest
    {
        [TestMethod]
        public void TestGetNextQuestion()
        {
            Mock<QuestionService> questionService = new Mock<QuestionService> { CallBase = true };

            List<QuestionDTO> list = new List<QuestionDTO>();
            QuestionDTO q1 = new QuestionDTO
            {
                Id = 1,
                QuizId = 1,
                QuizIndex = 0
            };
            list.Add(q1);
            QuestionDTO q2 = new QuestionDTO
            {
                Id = 2,
                QuizId = 1,
                QuizIndex = 1
            };
            list.Add(q2);
            QuestionDTO q3 = new QuestionDTO
            {
                Id = 3,
                QuizId = 1,
                QuizIndex = 2
            };
            list.Add(q3);

            questionService.Setup(s => s.GetQuestionByQuizId(It.IsAny<int>())).Returns(list);
            questionService.Setup(s => s.GetDBQuestionById(It.IsAny<int>())).Returns(new Question { Id = 2, Quiz = new Quiz { Id = 1 }, QuizIndex = 1 });

            QuestionDTO actual = questionService.Object.GetNextQuestion(1, 2);

            Assert.AreEqual(q3.Id, actual.Id);
        }

        [TestMethod]
        public void TestGetNextQuestionFirstQuestion()
        {
            Mock<QuestionService> questionService = new Mock<QuestionService> { CallBase = true };

            List<QuestionDTO> list = new List<QuestionDTO>();
            QuestionDTO q1 = new QuestionDTO
            {
                Id = 1,
                QuizId = 1,
                QuizIndex = 0
            };
            list.Add(q1);
            QuestionDTO q2 = new QuestionDTO
            {
                Id = 2,
                QuizId = 1,
                QuizIndex = 1
            };
            list.Add(q2);
            QuestionDTO q3 = new QuestionDTO
            {
                Id = 3,
                QuizId = 1,
                QuizIndex = 2
            };
            list.Add(q3);

            questionService.Setup(s => s.GetQuestionByQuizId(It.IsAny<int>())).Returns(list);
            questionService.Setup(s => s.GetDBQuestionById(It.IsAny<int>())).Returns(new Question());

            QuestionDTO actual = questionService.Object.GetNextQuestion(1, -1);

            Assert.AreEqual(q1.Id, actual.Id);
        }

        [TestMethod]
        public void TestGetNextQuestionLastQuestion()
        {
            Mock<QuestionService> questionService = new Mock<QuestionService> { CallBase = true };

            List<QuestionDTO> list = new List<QuestionDTO>();
            QuestionDTO q1 = new QuestionDTO
            {
                Id = 1,
                QuizId = 1,
                QuizIndex = 0
            };
            list.Add(q1);
            QuestionDTO q2 = new QuestionDTO
            {
                Id = 2,
                QuizId = 1,
                QuizIndex = 1
            };
            list.Add(q2);
            QuestionDTO q3 = new QuestionDTO
            {
                Id = 3,
                QuizId = 1,
                QuizIndex = 2
            };
            list.Add(q3);

            questionService.Setup(s => s.GetQuestionByQuizId(It.IsAny<int>())).Returns(list);
            questionService.Setup(s => s.GetDBQuestionById(It.IsAny<int>())).Returns(new Question { Id = 3, Quiz = new Quiz { Id = 1 }, QuizIndex = 2 });

            QuestionDTO actual = questionService.Object.GetNextQuestion(1, 3);

            Assert.AreEqual(0, actual.Id);
        }
    }
}

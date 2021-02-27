using API.Service;
using API.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models.Question;

namespace UnitTest
{
    [TestClass]
    class QuestionServiceTest
    {
        [TestMethod]
        public void TestModifyTrueOrFalseQuestion()
        {
            Mock<QuestionService> mock = new Mock<QuestionService> { CallBase = true };

            List<QuestionTrueFalse> answers = new List<QuestionTrueFalse>();
            answers.Add(new QuestionTrueFalse() { Id = 1, QuestionId = 1, Answer = true });

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

            Question question = new Question()
            {
                Id = 1,
                Label = "Question test",
                Quiz = quiz,
                Categories = new List<QuestionCategory>(),
                NeedsAllAnswers = false,
                QuestionType = QuestionType.TrueFalse,
                QuizIndex = 0,
                TimeLimit = 10,
                QuestionTrueFalse = answers
            };
        }
    }
}

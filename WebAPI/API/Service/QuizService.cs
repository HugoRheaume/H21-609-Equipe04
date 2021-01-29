using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;

namespace API.Service
{
    public class QuizService : BaseService
    {
        public QuizService(ApplicationDbContext db) : base(db) { }


        public QuizResponseDTO CreateQuiz(Quiz quiz, string username)
        {
            Quiz newQuiz = db.ListQuiz.Add(quiz);
            db.SaveChanges();


            return GenerateQuizResponseDTO(newQuiz, username);
        }
        public bool QuizCheck(string userId, string quizTitle)
        {
            return db.ListQuiz.Any(q => q.Title == quizTitle && q.OwnerId == userId);
        }

        public bool CheckCodeExist(string code)
        {
            return db.ListQuiz.Any(q => q.ShareCode == code);
        }

        public QuizResponseDTO GetGuizById(int quizId)
        {
            Quiz quizToShip = db.ListQuiz.Where(x => x.Id == quizId).FirstOrDefault();
            if (quizToShip == null)
                return null;
            return GenerateQuizResponseDTO(quizToShip, null);
        }
        private QuizResponseDTO GenerateQuizResponseDTO(Quiz quiz, string username)
        {
            return new QuizResponseDTO()
            {
                Id = quiz.Id,
                Author = username ?? "Nobody",
                Description = quiz.Description,
                IsPublic = quiz.IsPublic,
                ShareCode = quiz.ShareCode,
                Title = quiz.Title
            };
        }
    }
}
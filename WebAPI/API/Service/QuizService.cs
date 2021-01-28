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


            QuizResponseDTO response = new QuizResponseDTO()
            {
                Id = newQuiz.Id,
                Author = username ?? "Nobody",
                Description = newQuiz.Description,
                IsPublic = newQuiz.IsPublic,
                ShareCode = newQuiz.ShareCode,
                Title = newQuiz.Title
            };
            return response;
        }
        public bool QuizCheck(string userId, string quizTitle)
        {
            return db.ListQuiz.Any(q => q.Title == quizTitle && q.OwnerId == userId);
        }
    }
}
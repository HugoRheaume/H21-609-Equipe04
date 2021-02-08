using API.Models;
using API.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http.ModelBinding;

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

        public QuizResponseDTO GetQuizById(int quizId)
        {
            Quiz quizToShip = db.ListQuiz.FirstOrDefault(x => x.Id == quizId);
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

        public List<QuizResponseDTO> GetQuizFromUser(string userId, string username)
        {
            List<QuizResponseDTO> listQuiz =
                db.ListQuiz.Where(q => q.OwnerId == userId).Select(q => new QuizResponseDTO()
                {
                    Id = q.Id,
                    Author = username ?? "Nobody",
                    Title = q.Title,
                    IsPublic = q.IsPublic,
                    Description = q.Description,
                    ShareCode = q.ShareCode,
                    Date = q.Date,
                    NumberOfQuestions = q.ListQuestions.Count
                }).ToList();

            return listQuiz;
        }

        public bool DeleteQuiz(int quizId)
        {
            Quiz quizToDelete = db.ListQuiz.Find(quizId);
            if (quizToDelete == null) return false;

            List<Question> questionsToDelete = db.Question.Where(x => x.Quiz.Id == quizToDelete.Id).ToList();

            foreach (var item in questionsToDelete)
            {
                db.Question.Remove(item);
            }
            db.ListQuiz.Remove(quizToDelete);

            db.SaveChanges();
            return true;

        }
        public QuizResponseDTO GetQuizByCode(string pCode)
        {
                Quiz quiz;
                try
                {
                    quiz = db.ListQuiz.First(q => q.ShareCode == pCode);
                }
                catch (Exception)
                {
                    return null;
                }
                if (quiz != null)
                {

                    QuizResponseDTO response = new QuizResponseDTO()
                    {
                        Author = db.Users.Find(quiz.OwnerId)?.Name ?? "Nobody",
                        Id = quiz.Id,
                        IsPublic = quiz.IsPublic,
                        Description = quiz.Description,
                        ShareCode = quiz.ShareCode,
                        Title = quiz.Title,
                        Date = quiz.Date
                    };
                return response;
                }
                return null;
            
        }
        public QuizResponseDTO GetQuizByShareCode(string shareCode)
        {
            Quiz quizToShip = db.ListQuiz.Where(x => x.ShareCode == shareCode).FirstOrDefault();
            if(quizToShip == null)
            {
                return null;
            }
            return GenerateQuizResponseDTO(quizToShip, null);

        }
    }
}
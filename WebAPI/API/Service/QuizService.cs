using API.Models;
using API.Models.Question;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Xml.Linq;
using System.Web.Management;
using System.Web.Security;
using API.WebSocket;

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
                Title = quiz.Title,
                NumberOfQuestions = quiz.ListQuestions.Count
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
                    Date = quiz.Date,
                    NumberOfQuestions = quiz.ListQuestions.Count
                };
                return response;
            }
            return null;

        }
        public QuizResponseDTO GetQuizByShareCode(string shareCode)
        {
            Quiz quizToShip = db.ListQuiz.Where(x => x.ShareCode == shareCode).FirstOrDefault();
            if (quizToShip == null)
            {
                return null;
            }
            return GenerateQuizResponseDTO(quizToShip, null);

        }

        public int GetFinalScore(int quizId, CookieHeaderValue cookie)
        {
            string token = cookie["token"].Value;
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);

            int scoreTotal = 0;

            List<QuestionResult> list = db.QuestionResult.Where(q => q.User.Id == user.Id && q.Question.Quiz.Id == quizId).ToList();
            list.ForEach(q => scoreTotal += q.Score);

            return scoreTotal;
        }

        public void DeleteQuestionResults(int quizId, CookieHeaderValue cookie)
        {
            string token = cookie["token"].Value;
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);

            db.QuestionResult.RemoveRange(db.QuestionResult.Where(q => q.User.Id == user.Id && q.Question.Quiz.Id == quizId));
            db.SaveChanges();
        }

        /// <summary>
        /// Indique si le user passé en paramètre est le propriétaire du quiz
        /// </summary>
        /// <param name="userId">Id du user</param>
        /// <param name="quizId">Id du quiz</param>
        /// <returns>true si le user est le propriétaire du quiz et false si il ne l'est pas</returns>
        public bool IsUserOwnerOfQuiz(string userId, int quizId)
        {
            Quiz quiz = db.ListQuiz.Find(quizId);
            if (quiz != null)
                return quiz.OwnerId == userId;
            return false;
        }


        /// <summary>
        /// Retourne le type d'objet associé au sharecode donné
        /// </summary>
        /// <param name="sharecode">Sharecode du quiz/room</param>
        /// <returns>Quiz si c'est un quiz et Room si c'est une room</returns>
        public Type VerifyTypeOfShareCode(string sharecode)
        {
            Quiz quiz = db.ListQuiz.FirstOrDefault(q => q.ShareCode == sharecode);
            bool isRoom = RoomService.GetRooms.ContainsKey(sharecode);
            if (quiz != null)
                return typeof(Quiz);

            if (isRoom)
                return typeof(Room);

            return null;
        }



        public bool ModifyQuiz(QuizModifyDTO modifiedDTO)
        {
            Quiz quizToModify = db.ListQuiz.FirstOrDefault(q => q.Id == modifiedDTO.Id);
            if (quizToModify != null)
            {
                quizToModify.IsPublic = modifiedDTO.IsPublic;
                quizToModify.Title = modifiedDTO.Title;
                quizToModify.Description = modifiedDTO.Description;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<QuizResponseDTO> GetListPublicQuiz()
        {
            List<QuizResponseDTO> listPublicQuiz = db.ListQuiz.Where(q => q.IsPublic == true).Select(q => new QuizResponseDTO()
                {
                    Id = q.Id,
                    Author = db.Users.FirstOrDefault(u => q.OwnerId == u.Id).Name,
                    Title = q.Title,
                    IsPublic = q.IsPublic,
                    Description = q.Description,
                    ShareCode = q.ShareCode,
                    Date = q.Date,
                    NumberOfQuestions = q.ListQuestions.Count
                }).ToList();

            return listPublicQuiz;
        }
    }
}
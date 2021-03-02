using API.Models;
using API.Models.Question;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using API.WebSocket;
using Microsoft.Ajax.Utilities;
using API.Models.Quiz;

namespace API.Service
{
    public class QuizService : BaseService
    {
        public QuizService(ApplicationDbContext db) : base(db) { }

        public QuizService(){}

        /// <summary>
        /// Ajoute le quiz dans la database
        /// </summary>
        /// <param name="quiz">Quiz à ajouter</param>
        /// <returns>Le quiz qui viens d'être ajouté</returns>
        public virtual Quiz AddQuiz(Quiz quiz)
        {
            Quiz createdQuiz = db.ListQuiz.Add(quiz);
            db.SaveChanges();
            return createdQuiz;
        }

        /// <summary>
        /// Crée le quiz et retourne un QuizResponseDTO
        /// </summary>
        /// <param name="quiz">Le quiz à ajouter</param>
        /// <param name="username">Le username de l'utilisateur qui crée le quiz</param>
        /// <returns></returns>
        public virtual QuizResponseDTO CreateQuiz(Quiz quiz, string username)
        {
            if (quiz == null || username == null)
            {
                throw new NullReferenceException();
            }

            // Valide les attributs du Quiz
            ValidationContext context = new ValidationContext(quiz);
            List<ValidationResult> results = new List<ValidationResult>();
            if (Validator.TryValidateObject(quiz, context, results, true))
            {
                // Crée le quiz si il est valide

                Quiz newQuiz = AddQuiz(quiz);
                return GenerateQuizResponseDTO(newQuiz, username);
            }
            foreach (ValidationResult validation in results)
            {
                switch (validation.ErrorMessage)
                {
                    case "The title is too long":
                        throw new TitleTooLong();
                    case "The title is too short":
                        throw new TitleTooShort();
                    case "Description is too long":
                        throw new DescriptionIsTooLong();
                }
            }
            return null;
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

        /// <summary>
        /// Génère un QuizResponseDTO à partir d'un Quiz et du username qui l'a créer
        /// </summary>
        /// <param name="quiz">Le quiz à ajouter</param>
        /// <param name="username">Le username de l'utilisateur qui a créer le quiz</param>
        /// <returns>Un QuizResponseDTO à envoyer au client</returns>
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
                NumberOfQuestions = quiz.ListQuestions.Count,
                Date = quiz.Date
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

        /// <summary>
        /// Reçoit un quiz selon le code de partage reçu.
        /// </summary>
        /// <param name="shareCode">Le code de partage</param>
        /// <returns></returns>
        public QuizResponseDTO GetQuizByShareCode(string shareCode)
        {
            Quiz quizToShip = db.ListQuiz.FirstOrDefault(x => x.ShareCode == shareCode);
            if (quizToShip == null)
            {
                return null;
            }
            return GenerateQuizResponseDTO(quizToShip, null);

        }

        /// <summary>
        /// Trouve et renvoie tous les résultats dans la DB associé au Quiz et au User passé en paramètre.
        /// </summary>
        /// <param name="quizId">Id du quiz</param>
        /// <param name="cookie">Token du User</param>
        /// <returns>int</returns>
        public int GetFinalScore(int quizId, string token)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);

            int scoreTotal = 0;

            List<QuestionResult> list = db.QuestionResult.Where(q => q.User.Id == user.Id && q.Question.Quiz.Id == quizId).ToList();
            list.ForEach(q => scoreTotal += q.Score);

            return scoreTotal;
        }

        /// <summary>
        /// Supprime de la DB tous les résultats associé au Quiz et au User passé en paramètre.
        /// </summary>
        /// <param name="quizId">Id du quiz</param>
        /// <param name="cookie">Token du User</param>
        public void DeleteQuestionResults(int quizId, string token)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);

            db.QuestionResult.RemoveRange(db.QuestionResult.Where(q => q.User.Id == user.Id && q.Question.Quiz.Id == quizId));
            db.SaveChanges();
        }

        public void VerifyForTopScore(int quizId, string token, int score)
        {
            List<QuizTopScore> list = db.QuizTopScore.Where(q => q.QuizId == quizId).OrderByDescending(q => q.Score).Take(3).ToList();
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);
            
            if (list.Count < 3 || score > list[list.Count - 1].Score)
            {
                QuizTopScore topScore = new QuizTopScore
                {
                    QuizId = quizId,
                    Score = score,
                    UserName = user.Name
                };
                db.QuizTopScore.Add(topScore);

                if (list.Count == 3)
                    db.QuizTopScore.Remove(list[2]);

                db.SaveChanges();
            }
        }

        public List<QuizTopScore> GetTopScoresByQuiz(int quizId)
        {
            return db.QuizTopScore.Where(q => q.QuizId == quizId).OrderByDescending(q => q.Score).Take(3).ToList();
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
            if (modifiedDTO == null || modifiedDTO.Title == null || modifiedDTO.Description == null) return false;
            if (modifiedDTO.Description.Trim().Length < 1 || modifiedDTO.Description.Trim().Length > 1000) return false;
            if (modifiedDTO.Title.Trim().Length < 4 || modifiedDTO.Title.Trim().Length > 250) return false;

            Quiz quizToModify = getQuizByIdInDB(modifiedDTO.Id);
            if (quizToModify != null)
            {
                quizToModify.IsPublic = modifiedDTO.IsPublic;
                quizToModify.Title = modifiedDTO.Title;
                quizToModify.Description = modifiedDTO.Description;
                saveQuiz();
                return true;
            }
            return false;
        }

        public virtual Quiz getQuizByIdInDB(int Id)
        {
            return db.ListQuiz.FirstOrDefault(q => q.Id == Id);
        }
        public virtual bool saveQuiz()
        {
            db.SaveChanges();
            return true;
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

    public class DescriptionIsTooLong : Exception
    {
    }
    public class TitleTooShort : Exception
    {
    }

    public class TitleTooLong : Exception
    {
    }
}
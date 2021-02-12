using API.Models;
using API.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Net.Http.Headers;

namespace API.Service
{
    public class QuestionService : BaseService, IQuestionService
    {

        public QuestionService (ApplicationDbContext db) : base(db) { }

        public List<QuestionDTO> GetQuestions()
        {
            
            List<QuestionDTO> listToShip = new List<QuestionDTO>();

            List<Question> questionList = db.Question.Include(x => x.Quiz).ToList();
            foreach (var item in questionList)
            {
                listToShip.Add(GenerateQuestionDTO(item));
            }

            return listToShip;
        }
        public QuestionDTO GetQuestionById(int id)
        {
            Question question = db.Question.Include(q => q.Quiz).FirstOrDefault(x => x.Id == id);
            if (question != null)
                return GenerateQuestionDTO(question);

            return null;           
        }

        public List<QuestionDTO> GetQuestionsByShareCode(string shareCode)
        {
            Quiz quiz = db.ListQuiz.FirstOrDefault(x => x.ShareCode == shareCode);
            if (quiz == null)
                return null;

            List<Question> questionList = quiz.ListQuestions;

            return questionList.Select(item => GenerateQuestionDTO(item)).OrderBy(x => x.QuizIndex).ToList();
        }


        public List<QuestionDTO> GetQuestionByQuizId(int quizId)
        {
            Quiz quiz = db.ListQuiz.FirstOrDefault(x => x.Id == quizId);
            if (quiz == null)
                return null;
            List<Question> questionList = db.Question.Where(x => x.Quiz.Id == quizId).ToList();

            return questionList.Select(item => GenerateQuestionDTO(item)).OrderBy(x => x.QuizIndex).ToList();
        }

        public QuestionDTO GetNextQuestion(int quizId, int questionId)
        {
            List<QuestionDTO> list = GetQuestionByQuizId(quizId);
            Question question = db.Question.Find(questionId);
            int index = -1;
            if (questionId > -1)
                index = question.QuizIndex;
            if (list.Count <= index + 1)
                return new QuestionDTO() {
                    Id = 0,
                    QuizId = quizId,
                    Label = "",
                    TimeLimit = 0,
                    QuestionType = QuestionType.TrueFalse,
                    QuizIndex = -1,
                    NeedsAllAnswers = false,
                };
            return list[index + 1];
        }

        public QuestionDTO AddQuestion(QuestionCreateDTO question)
        {

            Quiz quiz = db.ListQuiz.FirstOrDefault(x => x.Id == question.QuizId);

            Question questionToCreate = new Question()
            {
                Quiz = quiz,
                Label = question.Label,
                TimeLimit = question.TimeLimit,
                QuestionType = question.QuestionType,
                QuestionTrueFalse = new List<QuestionTrueFalse>() { question.QuestionTrueFalse },
                QuestionMultipleChoice = question.QuestionMultipleChoice,
                NeedsAllAnswers = question.NeedsAllAnswers,
                QuestionAssociation = question.QuestionAssociation,
                Categories = question.Categories
            };

            Question q = db.Question.Add(questionToCreate);
            db.SaveChanges();
            return GenerateQuestionDTO(q);                       
        }

        
        private QuestionDTO GenerateQuestionDTO(Question q)
        {
            QuestionDTO question = new QuestionDTO()
            {
                Id = q.Id,
                QuizId = q.Quiz.Id,
                Label = q.Label,
                TimeLimit = q.TimeLimit,
                QuestionType = q.QuestionType,
                QuizIndex = q.QuizIndex,
                NeedsAllAnswers = q.NeedsAllAnswers,
                Categories = q.Categories

            };
            switch (q.QuestionType)
            {
                case QuestionType.TrueFalse:
                    question.QuestionTrueFalse = q.QuestionTrueFalse.FirstOrDefault();
                    break;
                case QuestionType.MultipleChoice:
                    question.QuestionMultipleChoice = q.QuestionMultipleChoice;
                    break;
                case QuestionType.Association:
                    question.QuestionAssociation = q.QuestionAssociation;
                    break;
                case QuestionType.Image:
                    break;
                default:
                    break;
            }

            return question;
        }

        public List<QuestionDTO> DeleteQuestion(int id)
        {
            Question question = db.Question.Where(x => x.Id == id).Include(x => x.Quiz).FirstOrDefault();
            if (question == null)
                return null;

            int quizId = question.Quiz.Id;

            db.Question.Remove(question);
            db.SaveChanges();
            return GetQuestionByQuizId(quizId);

        }
        public bool UpdateQuizIndex(List<QuestionDTO> questions )
        {
            foreach (var item in questions)
            {
                Question q = db.Question.Include(x => x.Quiz).FirstOrDefault(x => x.Id == item.Id);
                if (q == null)
                    continue;
                q.QuizIndex = item.QuizIndex;
            }
            db.SaveChanges();
            return true;

        }

        public bool StoreQuestionResult(QuestionResultDTO result, CookieHeaderValue cookie)
        {
            if (cookie == null) return false;
            string token = cookie["token"].Value;
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);

            db.QuestionResult.Add(new QuestionResult()
            {
                Question = db.Question.Find(result.QuestionId),
                User = user,
                Score = result.Score
            });

            db.SaveChanges();
            return true;
        }
    }
}
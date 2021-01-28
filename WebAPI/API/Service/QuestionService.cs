using API.Models;
using API.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace API.Service
{
    public class QuestionService : BaseService, IQuestionService
    {

        public QuestionService (ApplicationDbContext db) : base(db) { }

        public List<QuestionDTO> GetQuestions()
        {
            
            List<QuestionDTO> listToShip = new List<QuestionDTO>();

            List<Question> questionList = db.Question.ToList();
            foreach (var item in questionList)
            {
                listToShip.Add(GenerateQuestionDTO(item));
            }

            return listToShip;
        }
        public QuestionDTO GetQuestionById(int id)
        {
            Question question = db.Question.Where(x => x.Id == id).FirstOrDefault();
            if (question != null)
                return GenerateQuestionDTO(question);
            else
                return null;           
        }

        public List<Question> GetQuestionByQuizId(int quizId)
        {
            throw new NotImplementedException();
        }

        public QuestionDTO AddQuestion(Question question)
        {
            Question q = db.Question.Add(question);
            db.SaveChanges();
            return GenerateQuestionDTO(q);                       
        }

        private QuestionDTO GenerateQuestionDTO(Question q)
        {
            QuestionDTO question = new QuestionDTO()
            {
                Id = q.Id,
                Label = q.Label,
                TimeLimit = q.TimeLimit,
                QuestionType = q.QuestionType,

            };
            switch (q.QuestionType)
            {
                case QuestionType.TrueFalse:
                    question.QuestionTrueFalse = q.QuestionTrueFalse.First();
                    break;
                case QuestionType.MultipleChoice:
                    question.QuestionMultipleChoice = q.QuestionMultipleChoice;
                    break;
                case QuestionType.Association:
                    break;
                case QuestionType.Image:
                    break;
                default:
                    break;
            }

            return question;
        }
    }
}
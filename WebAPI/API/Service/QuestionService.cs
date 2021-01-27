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

            List<Question> questionList = db.Question.Include(x => x.QuestionTrueFalse).ToList();
            foreach (var item in questionList)
            {
                QuestionDTO question = new QuestionDTO()
                {
                    Id = item.Id,
                    Label = item.Label,
                    TimeLimit = item.TimeLimit,
                    QuestionType = item.QuestionType,
                   
                };
                switch (item.QuestionType)
                {
                    case QuestionType.TrueFalse:
                        question.QuestionTrueFalse = item.QuestionTrueFalse[0];
                        break;
                    case QuestionType.MultipleChoice:
                        question.QuestionMultipleChoice = item.QuestionMultipleChoice;
                        break;
                    case QuestionType.Association:
                        break;
                    case QuestionType.Image:
                        break;
                    default:
                        break;
                }

                listToShip.Add(question);



            }


            return listToShip;

        }
        public Question GetQuestionById(int id)
        {

            Question question = db.Question.Include(x => x.QuestionTrueFalse).Where(x => x.Id == id).First();
            return question;
        }


        public List<Question> GetQuestionByUser(int userId)
        {
            List<Question> questionList = db.Question.ToList();
            return questionList;
        }
        public void AddQuestion(Question question)
        {
            db.Question.Add(question);
            db.SaveChanges();
                       
        }        
    }
}
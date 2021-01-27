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

        public List<Question> GetQuestions()
        {
            List<Question> questionList = db.Question.Include(x => x.QuestionTrueFalse).ToList();
            return questionList;

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
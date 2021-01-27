using API.Models;
using API.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Service
{
    public class QuestionService : BaseService
    {

        public QuestionService (ApplicationDbContext db) : base(db) { }

        public List<Question> GetQuestions()
        {


            //QuestionTrueFalse t = new QuestionTrueFalse() { Id = 3, Answer = true, Statement = "Question test", TimeLimit = 60};

            //QuestionTrueFalse qtf = db.Question.First();
            List<Question> l = db.Question.ToList();
            

            return l;

        }
        public void AddQuestion(Question q)
        {
            Dictionary<string, bool> statements = new Dictionary<string, bool>();
            statements.Add("Choix 1", false);
            statements.Add("Choix 2", true);
            statements.Add("Choix 3", false);

            QuestionMultiple multiple = new QuestionMultiple() {MultipleStatement =  statements };
            db.Question.Add(multiple);
            db.SaveChanges();
        }
    }
}
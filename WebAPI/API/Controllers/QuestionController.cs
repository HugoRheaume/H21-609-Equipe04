using API.Models;
using API.Models.Question;
using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class QuestionController : ApiController
    {

        private QuestionService service = new QuestionService(new ApplicationDbContext());

        [HttpGet]
        public List<Question> Get()
        {

            return service.GetQuestions();
        }

        [HttpGet]
        public void Add()
        {
            QuestionTrueFalse q = new QuestionTrueFalse() {Answer = true, Statement = "Are you gay?", TimeLimit = 60 };
            service.AddQuestion(q);
            
        }
    }
}

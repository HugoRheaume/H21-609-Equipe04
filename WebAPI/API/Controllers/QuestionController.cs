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

        private IQuestionService service = new QuestionService(new ApplicationDbContext());

        [HttpGet]
        public List<QuestionDTO> Get()
        {

            return service.GetQuestions();
        }
        [HttpGet]        
        public Question Get(int id)
        {

            return service.GetQuestionById(id);
        }



        [HttpPost]
        public void Add(Question question)
        {
            service.AddQuestion(question);
        }
    }
}

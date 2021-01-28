using API.Models;
using API.Models.Question;
using API.Service;
using API.Validation;
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
        //Return List<QuestionDTO> 
        public IHttpActionResult Get()
        {
            return Ok(service.GetQuestions());            
        }
        [HttpGet]   
        //Return QuestionDTO
        public IHttpActionResult Get(int id)
        {
            return Ok(service.GetQuestionById(id));
        }

        [HttpPost]
        [QuestionValidation]
        public HttpResponseMessage Add(QuestionCreateDTO question)
        {
            Question questionToCreate = new Question() 
            { 
                Label = question.Label,
                TimeLimit = question.TimeLimit,
                QuestionType = question.QuestionType,
                QuestionTrueFalse = new List<QuestionTrueFalse>() { question.QuestionTrueFalse },
                QuestionMultipleChoice = question.QuestionMultipleChoice
            };

            service.AddQuestion(questionToCreate);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

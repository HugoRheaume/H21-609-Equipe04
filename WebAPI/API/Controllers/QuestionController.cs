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
        [HttpGet]
        [Route("api/question/getquizquestion/{quizid}")]
        public IHttpActionResult GetQuizQuestion(int quizId)
        {
            return Ok(service.GetQuestionByQuizId(quizId));
        }

        [HttpPost]
        [QuestionValidation]
        public IHttpActionResult Add(QuestionCreateDTO question)
        {
            return Ok(service.AddQuestion(question));
        }

        [HttpGet]
        [Route("api/question/delete/{questionId}")]
        public IHttpActionResult Delete(int questionId)
        {
            return Ok(service.DeleteQuestion(questionId));
        }
    }
}

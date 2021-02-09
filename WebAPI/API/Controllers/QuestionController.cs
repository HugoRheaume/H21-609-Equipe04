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
        [Route("api/Question/GetQuizQuestion/{quizid}")]
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
        [Route("api/Question/Delete/{questionId}")]
        public IHttpActionResult Delete(int questionId)
        {
            return Ok(service.DeleteQuestion(questionId));
        }


        [HttpPost]
        [Route("api/question/updatequizindex")]
        public IHttpActionResult UpdateQuizIndex(List<QuestionDTO> questions)
        {
            return Ok(service.UpdateQuizIndex(questions));
        }

        [HttpPost]
        public IHttpActionResult GetNextQuestion(QuestionResultDTO result)
        {
            if (result.QuestionId == -1)
                return Ok(service.GetNextQuestion(result.QuizId ,result.QuestionId));
            else if (service.StoreQuestionResult(result, Request.Headers.GetCookies("token").FirstOrDefault()))
                return Ok(service.GetNextQuestion(result.QuizId, result.QuestionId));
            else return BadRequest("Pas de Biscuit");
        }
    }
}

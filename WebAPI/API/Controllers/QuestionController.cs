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

        //public QuestionController(IQuestionService questionService)
        //{
        //    service = questionService;
        //}


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

        [HttpGet]
        [Route("api/Question/GetQuizQuestionsFromShareCode/{shareCode}")]
        public IHttpActionResult GetQuizQuestionsFromShareCode(string shareCode)
        {
            return Ok(service.GetQuestionsByShareCode(shareCode));
        }


        [TokenAuthorize]
        [HttpPost]
        [QuestionValidation]
        public IHttpActionResult Add(QuestionCreateDTO question)
        {
            AuthService authService = new AuthService(new ApplicationDbContext());
            QuizService quizService = new QuizService(new ApplicationDbContext());
            ApplicationUser user = authService.GetUserWithToken(Request);

            if (quizService.IsUserOwnerOfQuiz(user.Id, question.QuizId))
            {
                return Ok(service.AddQuestion(question));
            }

            return Unauthorized();
        }

        [TokenAuthorize]
        [HttpGet]
        [Route("api/Question/Delete/{questionId}")]
        public IHttpActionResult Delete(int questionId)
        {
            AuthService authService = new AuthService(new ApplicationDbContext());
            QuizService quizService = new QuizService(new ApplicationDbContext());
            ApplicationUser user = authService.GetUserWithToken(Request);

            int quizId = service.GetQuestionById(questionId).QuizId;

            if (quizService.IsUserOwnerOfQuiz(user.Id, quizId))
            {
                return Ok(service.DeleteQuestion(questionId));

            }

            return Unauthorized();
        }

        [TokenAuthorize]
        [HttpPost]
        [Route("api/question/updatequizindex")]
        public IHttpActionResult UpdateQuizIndex(List<QuestionDTO> questions)
        {
            
            return Ok(service.UpdateQuizIndex(questions));
        }

        [TokenAuthorize]
        [HttpPost]
        public IHttpActionResult GetNextQuestion(QuestionResultDTO result)
        {
            if (result.QuestionId == -1)
                return Ok(service.GetNextQuestion(result.QuizId ,result.QuestionId));
            else if (service.StoreQuestionResult(result, Request.Headers.GetCookies("token").FirstOrDefault()))
                return Ok(service.GetNextQuestion(result.QuizId, result.QuestionId));
            else return BadRequest("Pas de Biscuit");
        }


        [HttpPost]
        [ModelValidation]
        [TokenAuthorize]
        public IHttpActionResult ModifyQuestion(QuestionDTO modifiedDTO)
        {
            if (service.ModifyQuesiton(modifiedDTO)) return Ok("Question modifiée.");
            return BadRequest("Une erreur s'est produite.");
        }
    }
}

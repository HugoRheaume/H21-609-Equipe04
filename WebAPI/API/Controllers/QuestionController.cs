using API.Models;
using API.Models.Question;
using API.Service;
using API.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace API.Controllers
{
    public class QuestionController : ApiController
    {

        private IQuestionService service = new QuestionService(new ApplicationDbContext());
        private AuthService authService = new AuthService(new ApplicationDbContext());
        private QuizService quizService = new QuizService(new ApplicationDbContext());

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


        /// <summary>
        /// Ajouter une questions au quiz.
        /// </summary>
        /// <param name="question">La questions reçu par requête http</param>
        /// <returns></returns>
        [TokenAuthorize]
        [HttpPost]
        [QuestionValidation]
        public IHttpActionResult Add(QuestionCreateDTO question)
        {
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

        /// <summary>
        /// Si le QuestionId égale -1, donc qu'il n'y a pas de résultat puisqu'on veut la première question, retourne seulement celle-ci.
        /// Sinon, met le résultat dans la DB et retourne la prochaine question.
        /// </summary>
        /// <param name="result">Résultat de la question</param>
        /// <returns>QuestionDTO</returns>
        [TokenAuthorize]
        [HttpPost]
        public IHttpActionResult GetNextQuestion(QuestionResultDTO result)
        {
            CookieHeaderValue  cookie = Request.Headers.GetCookies("token").FirstOrDefault();
            if (cookie == null) return BadRequest("Pas de Biscuit");
            string token = cookie["token"].Value;

            if (result.QuestionId == -1)
            {
                QuizService s = new QuizService(new ApplicationDbContext());
                s.DeleteQuestionResults(result.QuizId, token);
            }
            else
                service.StoreQuestionResult(result, token);

            return Ok(service.GetNextQuestion(result.QuizId, result.QuestionId));
        }


        [HttpPost]
        [ModelValidation]
        [TokenAuthorize]
        public IHttpActionResult ModifyQuestion(QuestionDTO modifiedDTO)
        {
            AuthService authService = new AuthService(new ApplicationDbContext());
            QuizService quizService = new QuizService(new ApplicationDbContext());
            ApplicationUser user = authService.GetUserWithToken(Request);
            if (quizService.IsUserOwnerOfQuiz(user.Id, modifiedDTO.QuizId))
            {
                if (service.ModifyQuesiton(modifiedDTO)) return Ok("Question modifiée.");
                return BadRequest("Une erreur s'est produite.");
            }

            return Unauthorized();

        }
    }
}

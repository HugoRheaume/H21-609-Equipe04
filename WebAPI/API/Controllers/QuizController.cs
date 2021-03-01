using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using API.Models;
using API.Service;
using API.Validation;
using API.Models.Question;
using Microsoft.AspNet.Identity;
using System.Net.Http.Headers;
using API.WebSocket;
using API.Models.Quiz;

namespace API.Controllers
{
    public class QuizController : ApiController
    {
        private QuizService quizService;
        private static Random random = new Random();
        private AuthService authService;

        public QuizController()
        {
            quizService = new QuizService(new ApplicationDbContext());
            authService = new AuthService(new ApplicationDbContext());
        }

        [TokenAuthorize]
        [HttpPost]
        [ModelValidation]
        public IHttpActionResult Create(QuizRequestDTO quizRequestDto)
        {
            ApplicationUser user = authService.GetUserWithToken(Request);

            Quiz quiz = new Quiz
            {
                Title = quizRequestDto.Title,
                Description = quizRequestDto.Description,
                IsPublic = quizRequestDto.IsPublic,
                OwnerId = user.Id,
                ShareCode = Global.GenerateAlphanumeric(),
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };

            if (quizRequestDto.Confirm || !quizService.QuizCheck(user.Id, quizRequestDto.Title))
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created,
                    quizService.CreateQuiz(quiz, user.Name)));


            quizRequestDto.Confirm = true;
            // Envoie un message pour que l'utilisateur confirme
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                ReasonPhrase = "Quiz title already exist"
            };

            return ResponseMessage(message);
        }

        [TokenAuthorize]
        [HttpGet]
        public IHttpActionResult GetQuizFromUser()
        {
            ApplicationUser user = authService.GetUserWithToken(Request);

            return Ok(quizService.GetQuizFromUser(user.Id, user.Name));
        }

        [HttpGet]
        public IHttpActionResult DeleteQuiz(int id)
        {
            if (quizService.DeleteQuiz(id))
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [Route("api/Quiz/GetQuizById/{quizId}")]
        public IHttpActionResult GetQuizById(int quizId)
        {
            return Ok(quizService.GetQuizById(quizId));
        }

        [HttpGet]
        [TokenAuthorize]
        public IHttpActionResult GetQuizByCode([FromUri(Name = "code")] string pCode)
        {
            QuizResponseDTO response = quizService.GetQuizByShareCode(pCode);

            if (response == null)
            {
                return BadRequest("Aucun quiz n'a été trouvé");
            }
            return Ok(quizService.GetQuizByShareCode(pCode));
        }

        /// <summary>
        /// Prend le score de chaque question pour avoir le score final.
        /// Enlève de la DB le score pour les questions de ce Quiz de ce User.
        /// </summary>
        /// <returns>int</returns>
        /// <param name="quiz">Reçoit un QuizResponseDTO</param>
        [HttpPost]
        [TokenAuthorize]
        public IHttpActionResult GetFinalScore(QuizResponseDTO quiz)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("token").FirstOrDefault();
            if (cookie == null) return BadRequest("Pas de Biscuit");
            string token = cookie["token"].Value;

            int score = quizService.GetFinalScore(quiz.Id, token);

            quizService.VerifyForTopScore(quiz.Id, token, score);

            quizService.DeleteQuestionResults(quiz.Id, token);

            return Ok(score);
        }

        [HttpPost]
        public IHttpActionResult GetTopScores(QuizResponseDTO quiz)
        {
            return Ok(quizService.GetTopScoresByQuiz(quiz.Id));
        }


        [HttpGet]
        [Route("api/Quiz/GetObjectByShareCode/{sharecode}")]
        public IHttpActionResult GetObjectByShareCode(string sharecode)
        {
            Type objectType = quizService.VerifyTypeOfShareCode(sharecode);
            if (objectType == typeof(Quiz))
            {
                return Ok(quizService.GetQuizByShareCode(sharecode));
            }
            if (objectType == typeof(Room))
            {
                return Ok("Room.Join");
            }


            return BadRequest();
        }

        [HttpPost]
        [ModelValidation]
        [TokenAuthorize]
        public IHttpActionResult ModifyQuiz(QuizModifyDTO modifiedDTO)
        {
            AuthService authService = new AuthService(new ApplicationDbContext());
            QuizService quizService = new QuizService(new ApplicationDbContext());
            ApplicationUser user = authService.GetUserWithToken(Request);
            if (quizService.IsUserOwnerOfQuiz(user.Id, modifiedDTO.Id))
            {
                if (quizService.ModifyQuiz(modifiedDTO)) return Ok("Quiz modifié.");
                return BadRequest("Une erreur s'est produite.");
            }

            return Unauthorized();
        }

        [HttpGet]
        public IHttpActionResult GetPublicQuiz()
        {
            return Ok(quizService.GetListPublicQuiz());
        }
    }
}
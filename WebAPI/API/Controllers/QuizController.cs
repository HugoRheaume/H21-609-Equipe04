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

namespace API.Controllers
{
    public class QuizController : ApiController
    {
        private QuizService service = new QuizService(new ApplicationDbContext());
        private const string ALPHANUMERIC_CHARACTER_LIST = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();

        [TokenAuthorize]
        [HttpPost]
        [ModelValidation]
        public IHttpActionResult Create(QuizRequestDTO quizRequestDto)
        {

            AuthService authService = new AuthService(new ApplicationDbContext());

            ApplicationUser user = authService.GetUserWithToken(Request);
            // Enlever le "1" lorsque les user seront implémentés

            Quiz quiz = new Quiz
            {
                Title = quizRequestDto.Title,
                Description = quizRequestDto.Description,
                IsPublic = quizRequestDto.IsPublic,
                OwnerId = user.Id,
                ShareCode = GenerateAlphanumeric(),
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };

            if (quizRequestDto.Confirm || !service.QuizCheck(user.Id, quizRequestDto.Title))
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created,
                    service.CreateQuiz(quiz, user.Name)));


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
            AuthService authService = new AuthService(new ApplicationDbContext());

            ApplicationUser user = authService.GetUserWithToken(Request);

            return Ok(service.GetQuizFromUser(user.Id, user.Name));
        }

        [HttpGet]
        public IHttpActionResult DeleteQuiz(int id)
        {
            if (service.DeleteQuiz(id))
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [Route("api/Quiz/GetQuizById/{quizId}")]
        public IHttpActionResult GetQuizById(int quizId)
        {
            return Ok(service.GetQuizById(quizId));
        }

        [HttpGet]
        [TokenAuthorize]
        public IHttpActionResult GetQuizByCode([FromUri(Name = "code")] string pCode)
        {
            QuizResponseDTO response = service.GetQuizByCode(pCode);

            if (response == null)
            {
                return BadRequest("Aucun quiz n'a été trouvé");
            }
            return Ok(service.GetQuizByCode(pCode));
        }

        public string GenerateAlphanumeric()
        {
            StringBuilder code;
            do
            {
                code = new StringBuilder();
                char ch;
                for (int i = 0; i < 6; i++)
                {
                    ch = ALPHANUMERIC_CHARACTER_LIST[random.Next(0, ALPHANUMERIC_CHARACTER_LIST.Length)];
                    code.Append(ch);
                }
            } while (service.CheckCodeExist(code.ToString()));
            return code.ToString();
        }

        [HttpPost]
        [TokenAuthorize]
        public IHttpActionResult GetFinalScore(QuizResponseDTO quiz)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("token").FirstOrDefault();
            if (cookie == null) return BadRequest("Pas de Biscuit");

            int score = service.GetFinalScore(quiz.Id, cookie);

            service.DeleteQuestionResults(quiz.Id, cookie);

            return Ok(score);
        }

        [HttpPost]
        [ModelValidation]
        public IHttpActionResult ModifyQuiz(QuizModifyDTO modifiedDTO)
        {
            if (service.ModifyQuiz(modifiedDTO)) return Ok("Quiz modifié.");
            return BadRequest("Une erreur s'est produite.");
        }
    }
}
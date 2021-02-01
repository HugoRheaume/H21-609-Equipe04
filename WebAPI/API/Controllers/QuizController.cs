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

namespace API.Controllers
{
    public class QuizController : ApiController
    {
        private QuizService service = new QuizService(new ApplicationDbContext());
        private const string ALPHANUMERIC_CHARACTER_LIST = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();

        [HttpPost]
        [ModelValidation]
        public IHttpActionResult Create(QuizRequestDTO quizRequestDto)
        {
            // Enlever le "1" lorsque les user seront implémentés
            string userId = User.Identity.GetUserId() ?? "1";

            Quiz quiz = new Quiz()
            {
                Title = quizRequestDto.Title,
                Description = quizRequestDto.Description,
                IsPublic = quizRequestDto.IsPublic,
                OwnerId = userId,
                ShareCode = GenerateAlphanumeric(),
                Date = DateTime.Now,
                ListQuestions = new List<Question>()
            };

            if (quizRequestDto.Confirm || !service.QuizCheck(userId, quizRequestDto.Title))
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created,
                    service.CreateQuiz(quiz, User.Identity.Name)));


            quizRequestDto.Confirm = true;
            // Envoie un message pour que l'utilisateur confirme
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                ReasonPhrase = "Quiz title already exist"
            };

            return ResponseMessage(message);

        }
        
        [HttpGet]
        public IHttpActionResult GetQuizFromUser()
        {
            string userId = User.Identity.GetUserId() ?? "1";
            
            return Ok(service.GetQuizFromUser(userId, User.Identity.Name ?? "Nobody"));
        }

        [HttpGet]
        public IHttpActionResult DeleteQuiz(int id)
        {
            if (service.DeleteQuiz(id))
                return Ok();

            return BadRequest();
        }
        [HttpGet]
        [Route("api/quiz/getquizbyid/{quizId}")]

        public IHttpActionResult GetQuizById(int quizId)
        {
            return Ok(service.GetQuizById(quizId));
        }

        [HttpGet]
        public IHttpActionResult GetQuizByCode([FromUri(Name = "code")] string pCode)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Quiz quiz;
                try
                {
                    quiz = context.ListQuiz.First(q => q.ShareCode == pCode);
                }
                catch (Exception)
                {
                    return BadRequest("Aucun quiz n'a été trouvé.");
                }
                if (quiz != null)
                {
                
                    QuizResponseDTO response = new QuizResponseDTO()
                    {
                        Author = User.Identity.Name ?? "Nobody",
                        Id = quiz.Id,
                        IsPublic = quiz.IsPublic,
                        Description = quiz.Description,
                        ShareCode = quiz.ShareCode,
                        Title = quiz.Title
                    };
                    return Ok(response);
                }
                return null;
            }
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
    }
}
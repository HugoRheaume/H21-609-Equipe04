﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using API.Models;
using API.Service;
using API.Validation;
using Microsoft.AspNet.Identity;

namespace API.Controllers
{
    public class QuizController : ApiController
    {
        private QuizService service = new QuizService(new ApplicationDbContext());

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
                OwnerId = userId
            };

            if (!quizRequestDto.Confirm && service.QuizCheck(userId, quizRequestDto.Title))
            {
                quizRequestDto.Confirm = true;
                // Envoie un message pour que l'utilisateur confirme
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    ReasonPhrase = "Quiz title already exist"
                };

                return ResponseMessage(message);
            }
            

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created, service.CreateQuiz(quiz, User.Identity.Name)));
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
    }
}

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
using Microsoft.AspNet.Identity;

namespace API.Controllers
{
    public class QuizController : ApiController
    {
        [HttpPost]
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

            if (!ModelState.IsValid)
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, ModelState));

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (!quizRequestDto.Confirm && context.ListQuiz.Any(q => q.Title == quizRequestDto.Title && q.OwnerId == userId))
                {
                    quizRequestDto.Confirm = true;
                    // Envoie un message pour que l'utilisateur confirme
                    HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Accepted)
                    {
                        ReasonPhrase = "Quiz title already exist"
                    };

                    return ResponseMessage(message);
                }

                context.ListQuiz.Add(quiz);
                context.SaveChanges();
            }

            QuizResponseDTO response = new QuizResponseDTO()
            {
                Id = quiz.Id,
                Author = User.Identity.Name ?? "Nobody",
                Description = quiz.Description,
                IsPublic = quiz.IsPublic,
                ShareCode = quiz.ShareCode,
                Title = quiz.Title
            };

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created, response));
        }

        /*[HttpPost]
        public IHttpActionResult ConfirmCreate(QuizRequestDTO quizRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            string userId = User.Identity.GetUserId() ?? "1";
            Quiz quiz = new Quiz()
            {
                Title = quizRequestDto.Title,
                Description = quizRequestDto.Description,
                IsPublic = quizRequestDto.IsPublic,
                OwnerId = userId
            };
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.ListQuiz.Add(quiz);
                context.SaveChanges();
            }

            QuizResponseDTO response = new QuizResponseDTO()
            {
                Id = quiz.Id,
                Author = User.Identity.Name ?? "Nobody",
                Description = quiz.Description,
                IsPublic = quiz.IsPublic,
                ShareCode = quiz.ShareCode,
                Title = quiz.Title
            };

            return Ok(response);
        } */
    }
}
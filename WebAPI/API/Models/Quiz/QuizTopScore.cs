using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models.Quiz
{
    public class QuizTopScore
    {
        [Key]
        public int Id { get; set; }

        public int QuizId { get; set; }

        public int Score { get; set; }

        public string UserName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public class QuestionResult
    {
        [Key]
        public int Id { get; set; }

        public Question Question { get; set; }

        public ApplicationUser User { get; set; }

        public int Score { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public class QuestionTrueFalse
    {
        [Key]
        public int Id { get; set; }
        public bool Answer { get; set; }

        public int QuestionId { get; set; }
    }
}
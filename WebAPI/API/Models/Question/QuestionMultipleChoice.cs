using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public class QuestionMultipleChoice
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Statement { get; set; }
        public bool Answer { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public class QuestionTrueFalse : Question
    {
        public string Statement { get; set; }
        public bool Answer { get; set; }

    }
}
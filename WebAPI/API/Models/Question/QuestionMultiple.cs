using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public class QuestionMultiple : Question
    {


        public Dictionary<string, bool> MultipleStatement { get; set; }

    }
}
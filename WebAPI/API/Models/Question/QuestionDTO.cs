using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public class QuestionDTO
    {

        public int Id;
        public string Label;
        public QuestionType QuestionType;
        public int TimeLimit;
        public QuestionTrueFalse QuestionTrueFalse;
        public List<QuestionMultipleChoice> QuestionMultipleChoice;

    }
}
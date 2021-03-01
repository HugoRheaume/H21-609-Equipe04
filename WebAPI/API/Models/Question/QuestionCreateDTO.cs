using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public class QuestionCreateDTO
    {
        public string Label;
        public int QuizId;
        public QuestionType QuestionType;
        public int TimeLimit;
        public QuestionTrueFalse QuestionTrueFalse;
        public List<QuestionMultipleChoice> QuestionMultipleChoice;
        public List<QuestionAssociation> QuestionAssociation;
        public List<string> Categories;
        public bool NeedsAllAnswers;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public enum QuestionType { TrueFalse = 1, MultipleChoice = 2 , Association= 3}
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [MinLength(1, ErrorMessage = "You need to have a label"), MaxLength(250, ErrorMessage = "The label is too long")]
        public string Label { get; set; }

        public QuestionType QuestionType { get; set; }

        public int TimeLimit { get; set; }

        public bool NeedsAllAnswers { get; set; }

        [ForeignKey("QuestionId")]
        public virtual List<QuestionCategory> Categories { get; set; }

        [ForeignKey("QuestionId")]
        public virtual List<QuestionAssociation> QuestionAssociation { get; set; }

        [ForeignKey("QuestionId")]
        public virtual List<QuestionTrueFalse> QuestionTrueFalse { get; set; }

        [ForeignKey("QuestionId")]
        public virtual List<QuestionMultipleChoice> QuestionMultipleChoice { get; set; }

        public Quiz.Quiz Quiz { get; set; }

        public int QuizIndex { get; set; }

        public virtual List<QuestionResult> QuestionResults { get; set; }
    }
}
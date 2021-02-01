using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public enum QuestionType { TrueFalse = 1, MultipleChoice = 2 , Association= 3, Image = 4}
    public class Question
    {
        [Key]
        public int Id { get; set; }
        
        public string Label { get; set; }        

        public QuestionType QuestionType { get; set; }

        public int TimeLimit { get; set; }
        

        [ForeignKey("QuestionId")]
        public virtual List<QuestionTrueFalse> QuestionTrueFalse { get; set; }


        [ForeignKey("QuestionId")]
        public virtual List<QuestionMultipleChoice> QuestionMultipleChoice { get; set; }

        public Quiz Quiz { get; set; }
    }
}
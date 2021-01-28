using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models
{
    [Table("Quiz")]
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        public string OwnerId { get; set; }

        [MinLength(4, ErrorMessage = "The title is too short"), MaxLength(250, ErrorMessage = "The title is too long")]
        public string Title { get; set; }

        public bool IsPublic { get; set; }

        [MinLength(0, ErrorMessage = "You need to have a description"), MaxLength(1000, ErrorMessage = "Description is too long")]
        public string Description { get; set; }

        public string ShareCode { get; set; }

        //public List<Question.Question> ListQuestions { get; set; }
    }
}
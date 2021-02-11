using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class QuizModifyDTO
    {
        public int Id { get; set; }

        [MinLength(4, ErrorMessage = "Title is too short"), MaxLength(100, ErrorMessage = "Title is too long")]
        public string Title { get; set; }

        public bool IsPublic { get; set; }

        [MinLength(0, ErrorMessage = "You need to have a description"), MaxLength(1000, ErrorMessage = "Description is too long")]
        public string Description { get; set; }
    }
}
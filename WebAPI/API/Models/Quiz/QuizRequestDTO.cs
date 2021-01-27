using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace API.Models
{
    public class QuizRequestDTO
    {
        [MinLength(4, ErrorMessage = "Title can't be empty"), MaxLength(250, ErrorMessage = "Title is too long")]
        public string Title { get; set; }

        public bool IsPublic { get; set; }

        [MinLength(0, ErrorMessage = "You need to have a description"), MaxLength(1000, ErrorMessage = "Description is too long")]
        public string Description { get; set; }
    }
}
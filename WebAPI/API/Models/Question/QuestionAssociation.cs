using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Question
{
    public class QuestionAssociation
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public string Statement { get; set; }

        public int CategoryIndex { get; set; }
    }
}
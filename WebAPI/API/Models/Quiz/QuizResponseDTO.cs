using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class QuizResponseDTO
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public bool IsPublic { get; set; }

        public string Description { get; set; }

        public string ShareCode { get; set; }
    }
}
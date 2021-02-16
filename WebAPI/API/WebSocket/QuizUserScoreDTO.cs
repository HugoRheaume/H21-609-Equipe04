using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.WebSocket
{
    public class QuizUserScoreDTO
    {
        public UserDTO user { get; set; }
        public int score { get; set; }
    }
}
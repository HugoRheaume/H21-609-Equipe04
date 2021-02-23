using API.Models;
using API.Models.Question;
using API.Service;
using API.WebSocket.Command.QuizCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.WebSocket
{
    public class RoomQuizState
    {
        private QuestionService questionService = new QuestionService(new ApplicationDbContext());
        public int defaultTimeLimit { get; set; }
        private int questionIndex { get; set; }

        private Dictionary<string, int> scores = new Dictionary<string, int>();

        public bool IsAcceptingAnswer { get; set; }

        private Quiz quiz { get; set; }
        public Quiz GetQuiz { get { return quiz; } }
        private List<QuestionDTO> questions { get; set; }

        private int m_nbGoodAnswer = 0;
        public int GetNbGoodAnswer
        {
            get { return m_nbGoodAnswer; }
        }
        public RoomQuizState(Quiz quiz)
        {
            this.questionIndex = 0;
            this.quiz = quiz;
            IsAcceptingAnswer = false;
            questions =  questionService.GetQuestionByQuizId(this.quiz.Id);
        }
        public QuestionDTO NextQuestion(int questionIndex)
        {
            IsAcceptingAnswer = true;
            m_nbGoodAnswer = 0;
            this.questionIndex = questionIndex;
            foreach (var item in questions)
            {
                if (item.QuizIndex == this.questionIndex)
                    return item;
            }
            return null;
        }
        public void UpdateScores(string token , int score)
        {
            if (IsAcceptingAnswer == false)
                return;
            if (!scores.ContainsKey(token))
                scores.Add(token, 0);
            if (score > 0)
                m_nbGoodAnswer++;

            scores[token] += score;
        }
        public List<QuizUserScoreDTO> GetScores()
        {
            List<QuizUserScoreDTO> users = new List<QuizUserScoreDTO>();
            foreach (var item in this.scores)
            {
                QuizUserScoreDTO u = new QuizUserScoreDTO();
                u.user = new UserDTO(new ApplicationDbContext().Users.FirstOrDefault(x => x.Token == item.Key));
                if (u.user == null)
                    break;
                u.score = item.Value;
                users.Add(u);
            }
            return users;
        }
        
    }
}
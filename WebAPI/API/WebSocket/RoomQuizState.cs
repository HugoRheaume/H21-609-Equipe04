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
        
        private int questionIndex { get; set; }

        public bool IsAcceptingAnswer { get; set; }

        private Quiz quiz { get; set; }
        public Quiz GetQuiz { get { return quiz; } }
        private List<QuestionDTO> questions { get; set; }

        private int m_nbGoodAnswer = 0;
        public int GetNbGoodAnswer
        {
            get { return m_nbGoodAnswer; }
        }
        private List<Client> users { get; set; }
        private readonly Client owner;
        public RoomQuizState(Quiz quiz, ref List<Client> users, ref Client owner)
        {
            this.users = users;
            this.questionIndex = 0;
            this.quiz = quiz;
            this.owner = owner;
            IsAcceptingAnswer = false;
            questions =  questionService.GetQuestionByQuizId(this.quiz.Id);

            foreach (var item in users)
            {
                item.CurrentScore = 0;
            }
        }
        public QuestionDTO NextQuestion(int questionIndex)
        {
            ResetIsAnswer();
            IsAcceptingAnswer = true;
            this.m_nbGoodAnswer = 0;
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
            
            foreach (var item in this.users)
            {
                if(item.connectedUser.Token == token)
                {
                    if (score > 0)
                        this.m_nbGoodAnswer++;
                    item.CurrentScore += score;
                    item.IsAnswer = true;
                }
            }
            if (CheckAllAnswer())
            {
                ResetIsAnswer();
                new QuizForceSkipCommand().Run(owner);
            }

        }
        public List<QuizUserScoreDTO> GetScores()
        {
            List<QuizUserScoreDTO> userScores = new List<QuizUserScoreDTO>();
            foreach (var item in this.users)
            {
                QuizUserScoreDTO u = new QuizUserScoreDTO();
                u.user = new UserDTO(new ApplicationDbContext().Users.FirstOrDefault(x => x.Token == item.connectedUser.Token));
                if (u.user == null)
                    break;
                u.score = item.CurrentScore;
                userScores.Add(u);
            }
            userScores.OrderBy( x => x.score);
            return userScores;
        }

        private bool CheckAllAnswer()
        {
            bool allAnswer = true;
            foreach (var item in this.users)
            {
                if (!item.IsAnswer)
                    allAnswer = false;
            }
            return allAnswer;

        }
        private void ResetIsAnswer()
        {
            foreach (var item in this.users)
            {
                item.IsAnswer = false;
            }
        }
        
    }
}
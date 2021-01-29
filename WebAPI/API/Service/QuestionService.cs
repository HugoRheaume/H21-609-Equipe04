using API.Models;
using API.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace API.Service
{
    public class QuestionService : BaseService, IQuestionService
    {

        public QuestionService (ApplicationDbContext db) : base(db) { }

        public List<QuestionDTO> GetQuestions()
        {
            
            List<QuestionDTO> listToShip = new List<QuestionDTO>();

            List<Question> questionList = db.Question.Include(x => x.Quiz).ToList();
            foreach (var item in questionList)
            {
                listToShip.Add(GenerateQuestionDTO(item));
            }

            return listToShip;
        }
        public QuestionDTO GetQuestionById(int id)
        {
            Question question = db.Question.Where(x => x.Id == id).FirstOrDefault();
            if (question != null)
                return GenerateQuestionDTO(question);
            else
                return null;           
        }

        public List<QuestionDTO> GetQuestionByQuizId(int quizId)
        {
            List<QuestionDTO> listToShip = new List<QuestionDTO>();
            Quiz quiz = db.ListQuiz.Where(x => x.Id == quizId).FirstOrDefault();
            if (quiz == null)
                return null;
            List<Question> questionList = db.Question.Where(x => x.Quiz.Id == quizId).ToList();
            foreach (var item in questionList)
            {
                listToShip.Add(GenerateQuestionDTO(item));
            }
            return listToShip;



        }

        public QuestionDTO AddQuestion(QuestionCreateDTO question)
        {

            Quiz quiz = db.ListQuiz.Where(x => x.Id == question.QuizId).FirstOrDefault();



            Question questionToCreate = new Question()
            {
                Quiz = quiz,
                Label = question.Label,
                TimeLimit = question.TimeLimit,
                QuestionType = question.QuestionType,
                QuestionTrueFalse = new List<QuestionTrueFalse>() { question.QuestionTrueFalse },
                QuestionMultipleChoice = question.QuestionMultipleChoice
            };



            Question q = db.Question.Add(questionToCreate);
            db.SaveChanges();
            return GenerateQuestionDTO(q);                       
        }

        
        private QuestionDTO GenerateQuestionDTO(Question q)
        {
            QuestionDTO question = new QuestionDTO()
            {
                Id = q.Id,
                QuizId = q.Quiz.Id,
                Label = q.Label,
                TimeLimit = q.TimeLimit,
                QuestionType = q.QuestionType,

            };
            switch (q.QuestionType)
            {
                case QuestionType.TrueFalse:
                    question.QuestionTrueFalse = q.QuestionTrueFalse.First();
                    break;
                case QuestionType.MultipleChoice:
                    question.QuestionMultipleChoice = q.QuestionMultipleChoice;
                    break;
                case QuestionType.Association:
                    break;
                case QuestionType.Image:
                    break;
                default:
                    break;
            }

            return question;
        }

        public List<QuestionDTO> DeleteQuestion(int id)
        {
            Question question = db.Question.Where(x => x.Id == id).Include(x => x.Quiz).FirstOrDefault();
            if (question == null)
                return null;

            int quizId = question.Quiz.Id;

            db.Question.Remove(question);
            db.SaveChanges();
            return GetQuestionByQuizId(quizId);

        }
    }
}
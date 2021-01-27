using API.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Service
{
    public interface IQuestionService
    {
        List<QuestionDTO> GetQuestions();
        Question GetQuestionById(int id);
        List<Question> GetQuestionByUser(int userId);


        void AddQuestion(Question q);

    }
}
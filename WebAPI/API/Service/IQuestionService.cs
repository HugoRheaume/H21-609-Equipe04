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
        QuestionDTO GetQuestionById(int id);
        List<Question> GetQuestionByQuizId(int quizId);


        QuestionDTO AddQuestion(Question q);

    }
}
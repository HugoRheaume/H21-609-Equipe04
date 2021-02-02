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
        List<QuestionDTO> GetQuestionByQuizId(int quizId);


        QuestionDTO AddQuestion(QuestionCreateDTO q);

        List<QuestionDTO> DeleteQuestion(int id);
        bool UpdateQuizIndex(List<QuestionDTO> questions);

    }
}
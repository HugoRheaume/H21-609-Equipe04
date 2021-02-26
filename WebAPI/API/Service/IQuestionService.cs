using API.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace API.Service
{
    public interface IQuestionService
    {
        List<QuestionDTO> GetQuestions();
        QuestionDTO GetQuestionById(int id);
        List<QuestionDTO> GetQuestionByQuizId(int quizId);

        List<QuestionDTO> GetQuestionsByShareCode(string shareCode);
        QuestionDTO GetNextQuestion(int quizId, int questionId);

        QuestionDTO AddQuestion(QuestionCreateDTO q);

        List<QuestionDTO> DeleteQuestion(int id);
        bool UpdateQuizIndex(List<QuestionDTO> questions);
        void StoreQuestionResult(QuestionResultDTO result, string token);
        bool ModifyQuesiton(QuestionDTO modifiedDTO);
    }
}
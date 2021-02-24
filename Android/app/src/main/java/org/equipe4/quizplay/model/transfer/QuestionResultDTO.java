package org.equipe4.quizplay.model.transfer;

public class QuestionResultDTO {
    public int questionId;
    public int quizId;
    public int score;

    public QuestionResultDTO(int pQuestionId, int pQuizId, int pScore){
        questionId = pQuestionId;
        quizId = pQuizId;
        score = pScore;
    }
}

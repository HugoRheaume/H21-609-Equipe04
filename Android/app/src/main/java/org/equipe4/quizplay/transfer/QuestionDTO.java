package org.equipe4.quizplay.transfer;

import java.io.Serializable;
import java.util.List;

public class QuestionDTO implements Serializable {

    public int id;
    public int quizId;
    public String label;
    public int questionType;
    public int timeLimit;
    public QuestionTrueFalse questionTrueFalse;
    public List<QuestionMultipleChoice> questionMultipleChoice;
    public boolean needsAllAnswers;
    public int quizIndex;
}
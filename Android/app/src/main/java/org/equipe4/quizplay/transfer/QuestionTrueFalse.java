package org.equipe4.quizplay.transfer;

import java.io.Serializable;

public class QuestionTrueFalse implements Serializable {
    public int id;
    public boolean answer;
    public int questionId;
}

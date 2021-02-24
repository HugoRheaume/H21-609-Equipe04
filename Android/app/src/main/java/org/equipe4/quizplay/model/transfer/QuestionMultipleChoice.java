package org.equipe4.quizplay.model.transfer;

import java.io.Serializable;

public class QuestionMultipleChoice implements Serializable {
    public int id;
    public String statement;
    public boolean answer;
    public int questionId;
}

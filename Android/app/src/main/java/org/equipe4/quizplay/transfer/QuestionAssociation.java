package org.equipe4.quizplay.transfer;

import java.io.Serializable;

public class QuestionAssociation implements Serializable {
    public int id;
    public String statement;
    public int categoryIndex;
    public int questionId;
}

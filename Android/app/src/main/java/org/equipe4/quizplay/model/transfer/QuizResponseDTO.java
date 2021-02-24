package org.equipe4.quizplay.model.transfer;

import java.io.Serializable;

public class QuizResponseDTO implements Serializable {
    public int id;

    public String author;

    public String title;

    public boolean isPublic;

    public String description;

    public String shareCode;

    public int numberOfQuestions;
}

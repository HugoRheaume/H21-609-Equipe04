package org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation;

import org.equipe4.quizplay.model.webSocket.webSocketCommand.BaseCommand;

public class AnswerQuizCommand extends BaseCommand {
    public int score;

    public AnswerQuizCommand(String token, int score) {
        this.commandName = "Quiz.Answer";
        this.token = token;
        this.score = score;
    }
}

package org.equipe4.quizplay.model.webSocket.webSocketCommand.resultCommand;

import org.equipe4.quizplay.model.transfer.QuestionDTO;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.BaseCommand;

public class NextQuizResponse extends BaseCommand {
    public QuestionDTO question;
}

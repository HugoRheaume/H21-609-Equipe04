package org.equipe4.quizplay.model.webSocket.webSocketCommand.resultCommand;

import org.equipe4.quizplay.model.transfer.QuizResponseDTO;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.BaseCommand;

public class JoinRoomResponse extends BaseCommand {
    public QuizResponseDTO quiz;

}

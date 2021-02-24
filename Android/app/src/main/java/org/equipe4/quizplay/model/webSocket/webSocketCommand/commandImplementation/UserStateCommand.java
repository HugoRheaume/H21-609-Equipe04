package org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation;

import org.equipe4.quizplay.model.transfer.UserDTO;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.BaseCommand;

import java.util.ArrayList;
import java.util.List;

public class UserStateCommand extends BaseCommand {
    public List<UserDTO> users = new ArrayList<>();
}

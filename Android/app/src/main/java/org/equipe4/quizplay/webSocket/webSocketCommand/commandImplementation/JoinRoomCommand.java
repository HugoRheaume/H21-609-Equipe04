package org.equipe4.quizplay.webSocket.webSocketCommand.commandImplementation;

import org.equipe4.quizplay.webSocket.webSocketCommand.BaseCommand;

import java.io.Serializable;

public class JoinRoomCommand extends BaseCommand implements Serializable {
    private final String sharecode;

    public JoinRoomCommand(String token, String sharecode) {
        this.commandName = "Room.Join";
        this.token = token;
        this.sharecode = sharecode;
    }
}

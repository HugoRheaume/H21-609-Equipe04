package org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation;

import org.equipe4.quizplay.model.webSocket.webSocketCommand.BaseCommand;

import java.io.Serializable;

public class LeaveRoomCommand extends BaseCommand implements Serializable {
    private final String sharecode;

    public LeaveRoomCommand(String token, String sharecode) {
        this.commandName = "Room.Leave";
        this.token = token;
        this.sharecode = sharecode;
    }

}

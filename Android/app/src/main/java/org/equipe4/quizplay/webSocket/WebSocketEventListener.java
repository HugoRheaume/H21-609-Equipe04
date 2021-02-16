package org.equipe4.quizplay.webSocket;

import org.equipe4.quizplay.transfer.QuizResponseDTO;
import org.equipe4.quizplay.transfer.UserDTO;

import java.util.List;

public interface WebSocketEventListener {
        void OnUserState(List<UserDTO> listUsers);
        // or void onEvent(); as per your need

        void OnRoomJoin(QuizResponseDTO quiz);

        void OnRoomDeleted();
}


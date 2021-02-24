package org.equipe4.quizplay.model.webSocket;

import android.content.Intent;

import org.equipe4.quizplay.ListQuizActivity;
import org.equipe4.quizplay.model.transfer.QuizResponseDTO;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.resultCommand.NextQuizResponse;

public interface WebSocketEventListener {

        // On "Room.Join" message
        default void onRoomJoin(QuizResponseDTO quiz) {}

        // On "Log.Message" code 203 -> Room is deleted
        default void onRoomDeleted() { }

        //region Question response activity

        // On "Quiz.Next"
        default void onNextQuestion(NextQuizResponse nextQuizResponse) {}

        // On "Quiz.QuestionResult"
        default void onQuestionResult() {}

        //endregion
}


package org.equipe4.quizplay.activityLiveQuiz;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import org.equipe4.quizplay.ListQuizActivity;
import org.equipe4.quizplay.R;
import org.equipe4.quizplay.databinding.ActivityLiveTrueFalseBinding;
import org.equipe4.quizplay.model.transfer.QuestionDTO;
import org.equipe4.quizplay.model.transfer.QuizResponseDTO;
import org.equipe4.quizplay.model.util.Global;
import org.equipe4.quizplay.model.util.SharedPrefUtil;
import org.equipe4.quizplay.model.webSocket.WSClient;
import org.equipe4.quizplay.model.webSocket.WebSocketEventListener;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation.AnswerQuizCommand;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.resultCommand.NextQuizResponse;

import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.util.concurrent.ExecutionException;

public class LiveTrueFalseActivity extends AppCompatActivity {

    ActivityLiveTrueFalseBinding binding;
    WSClient client;

    QuestionDTO question;
    Button selectedButton;
    QuizResponseDTO currentQuiz;
    boolean isGoodAnswer;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityLiveTrueFalseBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        setWebSocketEvents();
        question = (QuestionDTO)getIntent().getSerializableExtra("question");

        currentQuiz = (QuizResponseDTO) getIntent().getSerializableExtra("currentQuiz");
        binding.progressQuiz.setMax(currentQuiz.numberOfQuestions);
        Log.i("WebSocket", currentQuiz.numberOfQuestions + "");
        binding.progressQuiz.setProgress(question.quizIndex + 1);
        binding.quizTitle.setText("" + currentQuiz.title);
    }

    private void sendAnswer() {

        // TODO Envoyer la réponse
        int score = 0;
        if (isGoodAnswer)
            score = 1;

        String token = SharedPrefUtil.getTokenFromCookie(getApplicationContext(), Global.getCurrentUri());
        AnswerQuizCommand answerQuizCommand = new AnswerQuizCommand(token, score);
        client.sendAnswer(answerQuizCommand);
    }

    public void verifyAnswer(View v) {
        isGoodAnswer = false;
        if (binding.btnTrue.getId() == v.getId()) {
            if (question.questionTrueFalse.answer) {
                isGoodAnswer = true;
            }
            selectedButton = binding.btnTrue;
            binding.btnTrue.setBackground(ContextCompat.getDrawable(getApplicationContext(), R.drawable.button_border_selected));
        }
        else if (binding.btnFalse.getId() == v.getId()) {
            if (!question.questionTrueFalse.answer) {
                isGoodAnswer = true;
            }
            selectedButton = binding.btnFalse;
            binding.btnFalse.setBackground(ContextCompat.getDrawable(getApplicationContext(), R.drawable.button_border_selected));
        }

        binding.btnFalse.setClickable(false);
        binding.btnTrue.setClickable(false);

        sendAnswer();
    }

    private void validateButtonState() {
        if (selectedButton != null){
            selectedButton.setBackground(ContextCompat.getDrawable(getApplicationContext(), R.drawable.button_border_wrong));
        }

        if (isGoodAnswer) {
            selectedButton.setBackground(ContextCompat.getDrawable(getApplicationContext(), R.drawable.button_border_good));
        }
        binding.btnTrue.setClickable(false);
        binding.btnFalse.setClickable(false);
    }

    private void setWebSocketEvents() {
        try {
            client = new WSClient();

            client.setEventListener(new WebSocketEventListener() {
                @Override
                public void onNextQuestion(NextQuizResponse nextQuizResponse) {
                    Intent i = new Intent(getApplicationContext(), Global.getQuestionTypeLive(nextQuizResponse.question.questionType));
                    i.putExtra("question", nextQuizResponse.question);
                    i.putExtra("currentQuiz", currentQuiz);
                    startActivity(i);
                }

                @Override
                public void onQuestionResult() {
                    validateButtonState();
                }

                @Override
                public void onRoomDeleted() {
                    Intent i = new Intent(getApplicationContext(), ListQuizActivity.class);
                    i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                    startActivity(i);
                }
            });
        } catch (KeyManagementException | NoSuchAlgorithmException | ExecutionException | InterruptedException e) {
            e.printStackTrace();
        }
    }
}
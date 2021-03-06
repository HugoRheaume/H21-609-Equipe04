package org.equipe4.quizplay.activityLiveQuiz;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.ToggleButton;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import org.equipe4.quizplay.ListQuizActivity;
import org.equipe4.quizplay.R;
import org.equipe4.quizplay.databinding.ActivityLiveMultipleChoiceBinding;
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
    QuizResponseDTO currentQuiz;
    boolean isGoodAnswer = false;
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

        setButtonEvents();
    }

    private void sendAnswer() {
        int score = 0;
        if (isGoodAnswer)
            score = 1;

        String token = SharedPrefUtil.getTokenFromCookie(getApplicationContext(), Global.getCurrentUri());
        AnswerQuizCommand answerQuizCommand = new AnswerQuizCommand(token, score);
        client.sendAnswer(answerQuizCommand);
    }

    public void verifyAnswer() {
        binding.btnFalse.setClickable(false);
        binding.btnTrue.setClickable(false);
        sendAnswer();
    }

    private void updateButtonState() {
        binding.btnTrue.setClickable(false);
        binding.btnFalse.setClickable(false);
        binding.result.setVisibility(View.VISIBLE);
        if (isGoodAnswer) {
            binding.result.setText(R.string.rightAnswer);
            binding.result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.good));
        }
        else {
            binding.result.setText(R.string.wrongAnswer);
            binding.result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.wrong));
        }
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
                    updateButtonState();
                    binding.result.setVisibility(View.VISIBLE);
                    if (isGoodAnswer) {
                        binding.result.setText(R.string.rightAnswer);
                        binding.result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.good));
                    }
                    else {
                        binding.result.setText(R.string.wrongAnswer);
                        binding.result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.wrong));
                    }
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

    private void setButtonEvents() {
        binding.btnFalse.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked){
                    if (!question.questionTrueFalse.answer) {
                        isGoodAnswer = true;
                    }
                    verifyAnswer();
                    updateButtonState();
                    }
            }
        });
        binding.btnTrue.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    if (question.questionTrueFalse.answer) {
                        isGoodAnswer = true;
                    }
                    verifyAnswer();
                    updateButtonState();
                }
            }
        });
    }
}
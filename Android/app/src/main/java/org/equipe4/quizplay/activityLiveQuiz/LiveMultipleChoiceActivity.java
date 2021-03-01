package org.equipe4.quizplay.activityLiveQuiz;

import android.content.Intent;
import android.icu.text.UnicodeSetSpanner;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.ToggleButton;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import org.equipe4.quizplay.ListQuizActivity;
import org.equipe4.quizplay.R;
import org.equipe4.quizplay.databinding.ActivityLiveMultipleChoiceBinding;
import org.equipe4.quizplay.model.transfer.QuestionDTO;
import org.equipe4.quizplay.model.transfer.QuestionMultipleChoice;
import org.equipe4.quizplay.model.transfer.QuizResponseDTO;
import org.equipe4.quizplay.model.util.Global;
import org.equipe4.quizplay.model.util.SharedPrefUtil;
import org.equipe4.quizplay.model.webSocket.WSClient;
import org.equipe4.quizplay.model.webSocket.WebSocketEventListener;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation.AnswerQuizCommand;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.resultCommand.NextQuizResponse;

import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ExecutionException;

public class LiveMultipleChoiceActivity extends AppCompatActivity {

    ActivityLiveMultipleChoiceBinding binding;
    WSClient client;
    QuizResponseDTO currentQuiz;
    QuestionDTO question;
    HashMap<Integer, ToggleButton> mapToggleButton;
    HashMap<ToggleButton, QuestionMultipleChoice> mapSelectedAnswers;
    boolean isGoodAnswer = false;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityLiveMultipleChoiceBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        setWebSocketEvents();
        question = (QuestionDTO)getIntent().getSerializableExtra("question");

        mapToggleButton = new HashMap<>();
        mapSelectedAnswers = new HashMap<>();
        mapToggleButton.put(0, binding.btnChoix1);
        mapToggleButton.put(1, binding.btnChoix2);
        mapToggleButton.put(2, binding.btnChoix3);
        mapToggleButton.put(3, binding.btnChoix4);
        mapToggleButton.put(4, binding.btnChoix5);
        mapToggleButton.put(5, binding.btnChoix6);
        mapToggleButton.put(6, binding.btnChoix7);
        mapToggleButton.put(7, binding.btnChoix8);

        for (Map.Entry<Integer, ToggleButton> entry: mapToggleButton.entrySet()) {
            if(entry.getKey() >= question.questionMultipleChoice.size()){
                entry.getValue().setVisibility(View.INVISIBLE);
            }
        }

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

    private void verifyAnswer() {
        boolean allAnswer = true;
        boolean oneAnswer = false;
        for(int i = 0; i < question.questionMultipleChoice.size(); i++) {
            QuestionMultipleChoice q = question.questionMultipleChoice.get(i);
            ToggleButton toggleButton = mapToggleButton.get(i);
            if (question.needsAllAnswers) {
                if (q.answer && !toggleButton.isChecked()) {
                    allAnswer = false;
                    break;
                }
            } else {
                if (q.answer && toggleButton.isChecked())
                    oneAnswer = true;
            }
            if (!q.answer && toggleButton.isChecked()) {
                allAnswer = false;
                oneAnswer = false;
                break;
            }
        }
        if (question.needsAllAnswers)
            isGoodAnswer = allAnswer;
        else
            isGoodAnswer = oneAnswer;

        binding.result.setVisibility(View.VISIBLE);
        if (isGoodAnswer) {
            binding.result.setText(R.string.rightAnswer);
            binding.result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.good));
        }
        else {
            binding.result.setText(R.string.wrongAnswer);
            binding.result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.wrong));
        }
        sendAnswer();
    }

    private void updateButtonState() {
        binding.btnSubmit.setClickable(false);
        for (ToggleButton btn :
                mapToggleButton.values()) {
            btn.setClickable(false);
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
        for (ToggleButton btn : mapToggleButton.values()) {
            btn.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                @Override
                public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                    if (isChecked) {
                        int questionIndex = 0;

                        for (Map.Entry<Integer, ToggleButton> entry : mapToggleButton.entrySet()) {
                            if (entry.getValue() == (ToggleButton) buttonView) {
                                questionIndex = entry.getKey();
                            }
                        }

                        mapSelectedAnswers.put((ToggleButton) buttonView ,question.questionMultipleChoice.get(questionIndex));
                    }
                    else {
                        mapSelectedAnswers.remove((ToggleButton) buttonView);
                    }
                }
            });
        }
        binding.btnSubmit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                verifyAnswer();
                updateButtonState();
            }
        });
    }
}
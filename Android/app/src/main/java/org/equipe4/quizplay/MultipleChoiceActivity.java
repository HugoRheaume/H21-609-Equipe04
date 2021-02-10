package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.content.Intent;
import android.content.res.Resources;
import android.graphics.PorterDuff;
import android.os.Bundle;
import android.os.CountDownTimer;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.ProgressBar;
import android.widget.RadioButton;
import android.widget.TextView;

import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.QuestionDTO;
import org.equipe4.quizplay.transfer.QuestionMultipleChoice;
import org.equipe4.quizplay.transfer.QuestionResultDTO;
import org.equipe4.quizplay.transfer.QuizResponseDTO;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MultipleChoiceActivity extends AppCompatActivity {

    QuestionDTO question;
    QuizResponseDTO quiz;
    CountDownTimer timer;
    QPService service = RetrofitUtil.get();
    QuestionDTO nextQuestion;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_multiple_choice);

        question = (QuestionDTO)getIntent().getSerializableExtra("question");
        quiz = (QuizResponseDTO)getIntent().getSerializableExtra("quiz");

        TextView questionLabel = findViewById(R.id.questionLabel);
        questionLabel.setText(question.label);

        TextView quizTitle = findViewById(R.id.quizTitle);
        quizTitle.setText(quiz.title);

        ProgressBar progressQuiz = findViewById(R.id.progressQuiz);
        progressQuiz.setMax(quiz.numberOfQuestions);
        progressQuiz.setProgress(question.quizIndex + 1);

        initRecycler();

        if (question.timeLimit > 0) {
            findViewById(R.id.timeLimit).setVisibility(View.VISIBLE);
            startTimer();
        }
    }

    private void startTimer() {
        Resources res = getResources();

        ProgressBar progressTime = findViewById(R.id.progressTime);
        progressTime.setMax(question.timeLimit);
        progressTime.setProgress(question.timeLimit);

        TextView tvTime = findViewById(R.id.tvTime);
        tvTime.setText(question.timeLimit + "");

        timer = new CountDownTimer(question.timeLimit * 1000, 1000) {
            @Override
            public void onTick(long millisUntilFinished) {
                tvTime.setText(res.getQuantityString(R.plurals.secondsRemaining, (int)millisUntilFinished / 1000, (int)millisUntilFinished / 1000));
                progressTime.setProgress((int)millisUntilFinished / 1000);
            }

            @Override
            public void onFinish() {
                tvTime.setText(R.string.timesup);
                answer();
            }
        };
        timer.start();
    }

    public void answer() {
        if (timer != null)
            timer.cancel();
        boolean rightAnswer = false;

        List<CheckBox> checkBoxes = getCheckboxList();

        boolean allAnswer = true;
        boolean oneAnswer = false;
        for(int i = 0; i < question.questionMultipleChoice.size(); i++) {
            QuestionMultipleChoice q = question.questionMultipleChoice.get(i);
            CheckBox c = checkBoxes.get(i);
            if (question.needsAllAnswers) {
                if (q.answer && !c.isChecked()) {
                    allAnswer = false;
                    break;
                }
            }
            else {
                if (q.answer && c.isChecked())
                    oneAnswer = true;
            }
            if (!q.answer && c.isChecked()) {
                allAnswer = false;
                oneAnswer = false;
                break;
            }
        }

        if (question.needsAllAnswers)
            rightAnswer = allAnswer;
        else
            rightAnswer = oneAnswer;


        for(int i = 0; i < question.questionMultipleChoice.size(); i++) {
            QuestionMultipleChoice q = question.questionMultipleChoice.get(i);
            CheckBox c = checkBoxes.get(i);

            if (q.answer) {
                c.setBackground(ContextCompat.getDrawable(getApplicationContext(), R.drawable.right_answer));
            }

            if (!q.answer && c.isChecked()) {
                c.getButtonDrawable().setColorFilter(ContextCompat.getColor(getApplicationContext(), R.color.wrong), PorterDuff.Mode.SRC_IN);
            }

            c.setClickable(false);
        }

        Button button = findViewById(R.id.nextQuestion);
        button.setText(R.string.nextQuestion);

        TextView result = findViewById(R.id.result);
        result.setVisibility(View.VISIBLE);
        if (rightAnswer) {
            result.setText(R.string.rightAnswer);
            result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.good));
        } else {
            result.setText(R.string.wrongAnswer);
            result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.wrong));
        }

        service.getNextQuestion(new QuestionResultDTO(question.id, quiz.id, rightAnswer ? 1 : 0)).enqueue(new Callback<QuestionDTO>() {
            @Override
            public void onResponse(Call<QuestionDTO> call, Response<QuestionDTO> response) {
                if(response.isSuccessful()) {
                    nextQuestion = response.body();
                    if (nextQuestion.quizIndex == -1)
                        button.setText(R.string.finishQuiz);
                    Log.i("Response", response.body().toString());
                } else {
                    Log.i("RETROFIT", response.message());
                }
            }

            @Override
            public void onFailure(Call<QuestionDTO> call, Throwable t) {
                Log.e("RETROFIT", t.getMessage());
            }
        });
    }

    public void nextQuestion(View v) {
        if (nextQuestion == null) {
            answer();
            return;
        }

        Intent i;
        if (nextQuestion.quizIndex == -1)
            i = new Intent(getApplicationContext(), ResultActivity.class);
        else {
            switch (nextQuestion.questionType) {
                case 1:
                    i = new Intent(getApplicationContext(), TrueFalseActivity.class);
                    break;
                case 2:
                    i = new Intent(getApplicationContext(), MultipleChoiceActivity.class);
                    break;
                default:
                    return;
            }
            i.putExtra("question", nextQuestion);
        }

        i.putExtra("quiz", quiz);
        startActivity(i);
        finish();
    }

    private void initRecycler() {
        RecyclerView choiceRecyclerView = findViewById(R.id.choiceRecyclerView);

        choiceRecyclerView.setHasFixedSize(true);

        RecyclerView.LayoutManager memeLayoutManager = new LinearLayoutManager(this);
        choiceRecyclerView.setLayoutManager(memeLayoutManager);

        RecyclerView.Adapter memeAdapter = new ChoiceAdapter(question.questionMultipleChoice);
        choiceRecyclerView.setAdapter(memeAdapter);
    }

    private List<CheckBox> getCheckboxList()
    {
        List<CheckBox> list = new ArrayList<>();
        int numberOfChoices = question.questionMultipleChoice.size();

        list.add(findViewById(R.id.checkboxChoice1));
        list.add(findViewById(R.id.checkboxChoice2));
        if (numberOfChoices >= 3)
            list.add(findViewById(R.id.checkboxChoice3));
        if (numberOfChoices >= 4)
            list.add(findViewById(R.id.checkboxChoice4));
        if (numberOfChoices >= 5)
            list.add(findViewById(R.id.checkboxChoice5));
        if (numberOfChoices >= 6)
            list.add(findViewById(R.id.checkboxChoice6));
        if (numberOfChoices >= 7)
            list.add(findViewById(R.id.checkboxChoice7));
        if (numberOfChoices >= 8)
            list.add(findViewById(R.id.checkboxChoice8));

        return list;
    }
}
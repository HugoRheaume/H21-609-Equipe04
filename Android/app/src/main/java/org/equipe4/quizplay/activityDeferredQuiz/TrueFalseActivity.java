package org.equipe4.quizplay.activityDeferredQuiz;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import android.content.Intent;
import android.content.res.Resources;
import android.graphics.PorterDuff;
import android.os.Bundle;
import android.os.CountDownTimer;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.RadioButton;
import android.widget.TextView;
import android.widget.Toast;

import org.equipe4.quizplay.R;
import org.equipe4.quizplay.model.http.QPService;
import org.equipe4.quizplay.model.http.RetrofitUtil;
import org.equipe4.quizplay.model.transfer.QuestionDTO;
import org.equipe4.quizplay.model.transfer.QuestionResultDTO;
import org.equipe4.quizplay.model.transfer.QuizResponseDTO;
import org.equipe4.quizplay.model.util.Global;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class TrueFalseActivity extends AppCompatActivity {

    QuestionDTO question;
    QuizResponseDTO quiz;
    CountDownTimer timer;
    QPService service = RetrofitUtil.get();
    QuestionDTO nextQuestion;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_true_false);
        question = (QuestionDTO)getIntent().getSerializableExtra("question");
        quiz = (QuizResponseDTO)getIntent().getSerializableExtra("quiz");

        TextView questionLabel = findViewById(R.id.questionLabel);
        questionLabel.setText(question.label);

        TextView quizTitle = findViewById(R.id.quizTitle);
        quizTitle.setText(quiz.title);

        ProgressBar progressQuiz = findViewById(R.id.progressQuiz);
        progressQuiz.setMax(quiz.numberOfQuestions);
        progressQuiz.setProgress(question.quizIndex + 1);

        if (question.timeLimit > 0) {
            findViewById(R.id.timeLimit).setVisibility(View.VISIBLE);
            startTimer();
        }
        else {
            findViewById(R.id.timeLimit).setLayoutParams(new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WRAP_CONTENT, 0));
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
                answer(null);
            }
        };
        timer.start();
    }

    public void answer(View v) {
        //Stop le timer
        if (timer != null)
            timer.cancel();

        boolean rightAnswer = false;

        if (v != null) {
            boolean checked = ((RadioButton) v).isChecked();

            switch (v.getId()) {
                case R.id.radioTrue:
                    if (checked && question.questionTrueFalse.answer)
                        rightAnswer = true;
                    break;
                case R.id.radioFalse:
                    if (checked && !question.questionTrueFalse.answer)
                        rightAnswer = true;
                    break;
            }
            //Change la couleur du RadioButton coch?? en rouge si ce n'est pas la bonne r??ponse
            if (!rightAnswer)
                ((RadioButton)v).getButtonDrawable().setColorFilter(ContextCompat.getColor(getApplicationContext(), R.color.wrong), PorterDuff.Mode.SRC_IN);

        }

        //D??sactive les RadioButton
        findViewById(R.id.radioTrue).setClickable(false);
        findViewById(R.id.radioFalse).setClickable(false);

        //Encercle la bonne r??ponse
        if (question.questionTrueFalse.answer) {
            RadioButton radioTrue = findViewById(R.id.radioTrue);
            radioTrue.setBackground(ContextCompat.getDrawable(getApplicationContext(), R.drawable.right_answer));
        } else {
            RadioButton radioFalse = findViewById(R.id.radioFalse);
            radioFalse.setBackground(ContextCompat.getDrawable(getApplicationContext(), R.drawable.right_answer));
        }

        //Change le texte du bouton
        Button button = findViewById(R.id.nextQuestion);
        button.setText(R.string.nextQuestion);

        //Affiche "Bonne" ou "Mauvaise r??ponse" de la bonne couleur
        TextView result = findViewById(R.id.result);
        result.setVisibility(View.VISIBLE);
        if (rightAnswer) {
            result.setText(R.string.rightAnswer);
            result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.good));
        } else {
            result.setText(R.string.wrongAnswer);
            result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.wrong));
        }

        SendResultAndGetNextQ(rightAnswer);
    }

    private void SendResultAndGetNextQ(boolean rightAnswer) {
        service.getNextQuestion(new QuestionResultDTO(question.id, quiz.id, rightAnswer ? 1 : 0)).enqueue(new Callback<QuestionDTO>() {
            @Override
            public void onResponse(Call<QuestionDTO> call, Response<QuestionDTO> response) {
                if(response.isSuccessful()) {
                    nextQuestion = response.body();

                    //Si c'est la derni??re question du quiz, change le texte du bouton pour dire: "Terminer le Quiz"
                    if (nextQuestion.quizIndex == -1)
                        ((Button)findViewById(R.id.nextQuestion)).setText(R.string.finishQuiz);

                    Log.i("Response", response.body().toString());
                } else {
                    Log.i("RETROFIT", response.message());
                }
            }

            @Override
            public void onFailure(Call<QuestionDTO> call, Throwable t) {
                Log.e("RETROFIT", t.getMessage());
                Toast.makeText(TrueFalseActivity.this, getString(R.string.toastNoAcess), Toast.LENGTH_SHORT).show();
            }
        });
    }

    public void nextQuestion(View v) {
        if (nextQuestion == null) {
            answer(null);
            return;
        }

        Intent i;
        if (nextQuestion.quizIndex == -1)
            i = new Intent(getApplicationContext(), ResultActivity.class);
        else {
            i = new Intent(getApplicationContext(), Global.getQuestionTypeDeferred(nextQuestion.questionType));
            i.putExtra("question", nextQuestion);
        }

        i.putExtra("quiz", quiz);
        startActivity(i);
        finish();
    }
}
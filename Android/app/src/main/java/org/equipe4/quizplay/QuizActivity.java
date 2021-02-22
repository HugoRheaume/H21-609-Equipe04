package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.TextView;

import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.QuestionDTO;
import org.equipe4.quizplay.transfer.QuestionResultDTO;
import org.equipe4.quizplay.transfer.QuizResponseDTO;
import org.w3c.dom.Text;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class QuizActivity extends AppCompatActivity {


    QPService service = RetrofitUtil.get();
    QuizResponseDTO quiz;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_quiz);

        quiz = (QuizResponseDTO)getIntent().getSerializableExtra("quiz");

        TextView quizTitle = findViewById(R.id.quizTitle);
        quizTitle.setText(quiz.title);
        TextView quizDesc = findViewById(R.id.quizDesc);
        quizDesc.setText(quiz.description);

    }

    public void startQuiz(View v) {
        service.getNextQuestion(new QuestionResultDTO(-1, quiz.id,0)).enqueue(new Callback<QuestionDTO>() {
            @Override
            public void onResponse(Call<QuestionDTO> call, Response<QuestionDTO> response) {
                if(response.isSuccessful()) {
                    QuestionDTO question = response.body();
                    Log.i("Response", response.body().toString());

                    Intent i;
                    switch (question.questionType) {
                        case 1:
                            i = new Intent(getApplicationContext(), TrueFalseActivity.class);
                            break;
                        case 2:
                            i = new Intent(getApplicationContext(), MultipleChoiceActivity.class);
                            break;
                        case 3:
                            i = new Intent(getApplicationContext(), AssociationActivity.class);
                            break;
                        default:
                            return;
                    }
                    i.putExtra("question", question);
                    i.putExtra("quiz", quiz);
                    startActivity(i);
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
}
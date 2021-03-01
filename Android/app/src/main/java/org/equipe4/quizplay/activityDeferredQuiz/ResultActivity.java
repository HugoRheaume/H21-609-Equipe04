package org.equipe4.quizplay.activityDeferredQuiz;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.content.res.Resources;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import org.equipe4.quizplay.LandingActivity;
import org.equipe4.quizplay.ListQuizActivity;
import org.equipe4.quizplay.R;
import org.equipe4.quizplay.model.http.QPService;
import org.equipe4.quizplay.model.http.RetrofitUtil;
import org.equipe4.quizplay.model.transfer.QuizResponseDTO;
import org.equipe4.quizplay.model.transfer.QuizTopScore;
import org.equipe4.quizplay.model.transfer.UserDTO;
import org.equipe4.quizplay.model.util.SharedPrefUtil;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ResultActivity extends AppCompatActivity {

    QPService service = RetrofitUtil.get();
    QuizResponseDTO quiz;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_result);

        quiz = (QuizResponseDTO)getIntent().getSerializableExtra("quiz");

        TextView tvScore = findViewById(R.id.score);
        Resources res = getResources();

        int score = -1;
        service.getFinalScore(quiz).enqueue(new Callback<Integer>() {
            @Override
            public void onResponse(Call<Integer> call, Response<Integer> response) {
                if (response.isSuccessful()) {
                    int score = response.body();
                    tvScore.setText(res.getQuantityString(R.plurals.points, score, score));
                    loadScoreboard();
                } else {
                    Log.i("RETROFIT", response.message());
                }
            }

            @Override
            public void onFailure(Call<Integer> call, Throwable t) {
                Log.e("RETROFIT", t.getMessage());
                Toast.makeText(ResultActivity.this, getString(R.string.toastNoAcess), Toast.LENGTH_SHORT).show();
            }
        });

        loadScoreboard();
    }

    private void loadScoreboard() {
        service.getTopScores(quiz).enqueue(new Callback<List<QuizTopScore>>() {
            @Override
            public void onResponse(Call<List<QuizTopScore>> call, Response<List<QuizTopScore>> response) {
                if (response.isSuccessful()) {
                    Resources res = getResources();
                    List<QuizTopScore> list = response.body();

                    for (int i = 0; i < list.size(); i++) {
                        QuizTopScore topScore = list.get(i);

                        if (i == 0) {
                            ((TextView)findViewById(R.id.place1Name)).setText(topScore.userName);
                            ((TextView)findViewById(R.id.place1Score)).setText(res.getQuantityString(R.plurals.pts, topScore.score, topScore.score));
                        }
                        else if (i == 1) {
                            ((TextView)findViewById(R.id.place2Name)).setText(topScore.userName);
                            ((TextView)findViewById(R.id.place2Score)).setText(res.getQuantityString(R.plurals.pts, topScore.score, topScore.score));
                        }
                        else if (i == 2) {
                            ((TextView)findViewById(R.id.place3Name)).setText(topScore.userName);
                            ((TextView)findViewById(R.id.place3Score)).setText(res.getQuantityString(R.plurals.pts, topScore.score, topScore.score));
                        }
                    }
                }
                else {
                    Log.i("RETROFIT", response.message());
                }
            }

            @Override
            public void onFailure(Call<List<QuizTopScore>> call, Throwable t) {
                Log.e("RETROFIT", t.getMessage());
                Toast.makeText(ResultActivity.this, getString(R.string.toastNoAcess), Toast.LENGTH_SHORT).show();
            }
        });
    }

    public void back(View v) {
        Intent i = new Intent(getApplicationContext(), ListQuizActivity.class);
        i.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivity(i);
    }
}
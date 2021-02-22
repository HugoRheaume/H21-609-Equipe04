package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.content.res.Resources;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.QuizResponseDTO;
import org.equipe4.quizplay.transfer.UserDTO;
import org.equipe4.quizplay.util.SharedPrefUtil;

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
                } else {
                    Log.i("RETROFIT", response.message());
                }
            }

            @Override
            public void onFailure(Call<Integer> call, Throwable t) {
                Log.e("RETROFIT", t.getMessage());
            }
        });
    }

    public void back(View v) {
        Intent i = new Intent(getApplicationContext(), ListQuizActivity.class);
        i.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivity(i);
    }
}
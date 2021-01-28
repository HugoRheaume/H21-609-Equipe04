package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.HelloWorldObj;
import org.equipe4.quizplay.transfer.QuizResponseDTO;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity {

    QPService service = new RetrofitUtil().service;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void JoinQuiz(View v){
        EditText editTextCode = findViewById(R.id.editTextCode);

        if (editTextCode.getText().toString().equals("")){
            Toast.makeText(this, R.string.toastEnterCode, Toast.LENGTH_SHORT).show();
        }
        else {
            service.getQuizByCode(editTextCode.getText().toString().toUpperCase()).enqueue(new Callback<QuizResponseDTO>() {
                @Override
                public void onResponse(Call<QuizResponseDTO> call, Response<QuizResponseDTO> response) {
                    if (response.isSuccessful()){
                        Intent intent = new Intent(getApplicationContext(), PseudoActivity.class);
                        startActivity(intent);
                    }
                    else {
                        Toast.makeText(MainActivity.this, R.string.toastNoQuizFound, Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<QuizResponseDTO> call, Throwable t) {
                    Log.e("RETROFIT", t.getMessage());
                }
            });
        }
    }



//    public void helloWorld(View v) {
//        QPService service = new RetrofitUtil().service;
//        final HelloWorldObj[] helloWorldObj = new HelloWorldObj[1];
//
//        startLoading();
//        service.helloWorld().enqueue(new Callback<HelloWorldObj>() {
//            @Override
//            public void onResponse(Call<HelloWorldObj> call, Response<HelloWorldObj> response) {
//                if (response.isSuccessful()) {
//                    helloWorldObj[0] = response.body();
//
//                    TextView text = findViewById(R.id.helloWorldTxt);
//                    text.setText(helloWorldObj[0].text);
//
//                    ImageView img = findViewById(R.id.helloWorldImg);
//                    Picasso.get().load(helloWorldObj[0].image).into(img);
//
//                    stopLoading();
//                }
//                else {
//                    Log.i("RETROFIT", response.code()+"");
//                    Toast.makeText(getApplicationContext(), "not stonks", Toast.LENGTH_SHORT).show();
//
//                    stopLoading();
//                }
//            }
//
//            @Override
//            public void onFailure(Call<HelloWorldObj> call, Throwable t) {
//                Log.i("RETROFIT", t.getMessage());
//                Toast.makeText(getApplicationContext(), "Connection problems???", Toast.LENGTH_SHORT).show();
//
//                stopLoading();
//            }
//        });
//    }
//
//    private void startLoading() {
//        findViewById(R.id.progress).setVisibility(View.VISIBLE);
//        findViewById(R.id.helloWorldBtn).setClickable(false);
//    }
//
//    private void stopLoading() {
//        findViewById(R.id.progress).setVisibility(View.GONE);
//        findViewById(R.id.helloWorldBtn).setClickable(true);
//    }
}
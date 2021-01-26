package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.HelloWorldObj;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void helloWorld(View v) {
        QPService service = new RetrofitUtil().service;
        final HelloWorldObj[] helloWorldObj = new HelloWorldObj[1];

        startLoading();
        service.helloWorld().enqueue(new Callback<HelloWorldObj>() {
            @Override
            public void onResponse(Call<HelloWorldObj> call, Response<HelloWorldObj> response) {
                if (response.isSuccessful()) {
                    helloWorldObj[0] = response.body();

                    TextView text = findViewById(R.id.helloWorldTxt);
                    text.setText(helloWorldObj[0].text);

                    ImageView img = findViewById(R.id.helloWorldImg);
                    Picasso.get().load(helloWorldObj[0].image).into(img);

                    stopLoading();
                }
                else {
                    Log.i("RETROFIT", response.code()+"");
                    Toast.makeText(getApplicationContext(), "not stonks", Toast.LENGTH_SHORT).show();

                    stopLoading();
                }
            }

            @Override
            public void onFailure(Call<HelloWorldObj> call, Throwable t) {
                Log.i("RETROFIT", t.getMessage());
                Toast.makeText(getApplicationContext(), "Connection problems???", Toast.LENGTH_SHORT).show();

                stopLoading();
            }
        });
    }

    private void startLoading() {
        findViewById(R.id.progress).setVisibility(View.VISIBLE);
        findViewById(R.id.helloWorldBtn).setClickable(false);
    }

    private void stopLoading() {
        findViewById(R.id.progress).setVisibility(View.GONE);
        findViewById(R.id.helloWorldBtn).setClickable(true);
    }
}
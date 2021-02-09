package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

public class ResultActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_result);
    }

    public void back(View v) {
        Intent i = new Intent(getApplicationContext(), MainActivity.class);
        startActivity(i);
    }
}
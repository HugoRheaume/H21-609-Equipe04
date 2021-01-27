package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.widget.TextView;

public class WaitingRoomActivity extends AppCompatActivity {


    static String pseudo;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_waiting_room);

        Intent intent = getIntent();
        pseudo = intent.getStringExtra("playerPseudo");


        TextView textViewPseudo = findViewById(R.id.textViewPseudo);
        textViewPseudo.setText(pseudo);
    }
}
package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

public class PseudoActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_pseudo);
    }

    public void JoinWaitingRoom(View v){
        EditText editTextPseudo = findViewById(R.id.editTextPseudo);
        if (editTextPseudo.getText().toString().equals("")){
            Toast.makeText(this, R.string.toastEnterName, Toast.LENGTH_SHORT).show();
        }
        else {
            Intent intent = new Intent(this, WaitingRoomActivity.class);
            intent.putExtra("playerPseudo", editTextPseudo.getText().toString());
            startActivity(intent);
        }
    }
}
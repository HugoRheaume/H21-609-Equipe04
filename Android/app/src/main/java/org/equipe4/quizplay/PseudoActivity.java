package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import org.equipe4.quizplay.databinding.ActivityPseudoBinding;

public class PseudoActivity extends AppCompatActivity {

    private ActivityPseudoBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityPseudoBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
    }

    public void JoinWaitingRoom(View v){
        EditText editTextPseudo = findViewById(R.id.editTextPseudo);
        if (editTextPseudo.getText().toString().equals("")){
            Toast.makeText(this, R.string.toastEnterName, Toast.LENGTH_SHORT).show();
        }
        else {
            Intent intent = new Intent(this, QuizActivity.class);
            intent.putExtra("quiz", getIntent().getSerializableExtra("quiz"));
            startActivity(intent);
        }
    }
}
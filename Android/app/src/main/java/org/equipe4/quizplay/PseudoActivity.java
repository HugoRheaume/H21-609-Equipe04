package org.equipe4.quizplay;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import org.equipe4.quizplay.databinding.ActivityPseudoBinding;
import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.UserDTO;
import org.equipe4.quizplay.util.SharedPrefUtil;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PseudoActivity extends AppCompatActivity {

    private ActivityPseudoBinding binding;
    QPService service;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityPseudoBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        service = RetrofitUtil.get();
    }

    public void CreateUser(View v) {
        EditText editTextPseudo = binding.editTextPseudo;
        String username = editTextPseudo.getText().toString().trim();

        if (username.equals("")){
            Toast.makeText(this, R.string.toastEnterName, Toast.LENGTH_SHORT).show();
        }
        else {
            service.generateAnonymousUser("\"" + username + "\"").enqueue(new Callback<UserDTO>() {
                @Override
                public void onResponse(Call<UserDTO> call, Response<UserDTO> response) {
                    if (response.isSuccessful()) {
                        SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
                        response.body().isAnonymous = true;
                        sharedPrefUtil.setCurrentUser(response.body());

                        Intent intent = new Intent(getApplicationContext(), ListQuizActivity.class);
                        startActivity(intent);
                        finish();
                    }
                }

                @Override
                public void onFailure(Call<UserDTO> call, Throwable t) {
                    Toast.makeText(PseudoActivity.this, getString(R.string.toastNoAcess), Toast.LENGTH_SHORT).show();
                }
            });



        }
    }
}
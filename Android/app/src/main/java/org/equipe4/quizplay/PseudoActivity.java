package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import org.equipe4.quizplay.databinding.ActivityPseudoBinding;
import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.UserDTO;
import org.equipe4.quizplay.util.SharedPrefUtil;
import org.equipe4.quizplay.webSocket.webSocketCommand.commandImplementation.JoinRoomCommand;

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

    public void JoinWaitingRoom(View v) {
        EditText editTextPseudo = binding.editTextPseudo;
        String username = editTextPseudo.getText().toString();

        if (username.equals("")){
            Toast.makeText(this, R.string.toastEnterName, Toast.LENGTH_SHORT).show();
        }
        else {
            service.generateAnonymousUser("\"" + username + "\"").enqueue(new Callback<UserDTO>() {
                @Override
                public void onResponse(Call<UserDTO> call, Response<UserDTO> response) {
                    if (response.isSuccessful()) {
                        if (getIntent().getBooleanExtra("isLive", false)) {
                            // EN DIRECT
                            String token = SharedPrefUtil.getTokenFromCookie(getApplicationContext(), response.raw().request().url().host());

                            SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
                            response.body().isAnonymous = true;
                            sharedPrefUtil.setCurrentUser(response.body());

                            Intent intent = new Intent(getApplicationContext(), WaitingRoomActivity.class);
                            intent.putExtra("shareCode", getIntent().getStringExtra("shareCode"));
                            intent.putExtra("token", token);
                            startActivity(intent);
                        }
                        else {
                            // EN DIFFÉRÉ
                            SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
                            response.body().isAnonymous = true;
                            sharedPrefUtil.setCurrentUser(response.body());
                            Intent intent = new Intent(getApplicationContext(), QuizActivity.class);
                            intent.putExtra("quiz", getIntent().getSerializableExtra("quiz"));
                            startActivity(intent);
                        }

                    }
                }

                @Override
                public void onFailure(Call<UserDTO> call, Throwable t) {
                    Toast.makeText(PseudoActivity.this, "SOMETHING WENT WRONG MY GUY", Toast.LENGTH_SHORT).show();
                }
            });



        }
    }
}
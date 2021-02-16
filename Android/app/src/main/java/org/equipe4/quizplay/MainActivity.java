package org.equipe4.quizplay;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.franmontiel.persistentcookiejar.persistence.SerializableCookie;
import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.firebase.auth.FirebaseAuth;
import com.google.gson.Gson;
import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.databinding.ActivityMainBinding;
import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.QuizResponseDTO;
import org.equipe4.quizplay.transfer.UserDTO;
import org.equipe4.quizplay.util.SharedPrefUtil;
import org.equipe4.quizplay.webSocket.webSocketCommand.commandImplementation.JoinRoomCommand;

import okhttp3.Cookie;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity {

    private ActivityMainBinding binding;
    private FirebaseAuth mAuth;
    private QPService service = RetrofitUtil.get();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
        UserDTO user = sharedPrefUtil.getCurrentUser();

        binding.name.setText(user.name);
        Picasso.get().load(user.picture).into(binding.profilePic);

        binding.btnLogout.setOnClickListener(v -> {

            GoogleSignInClient client;
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                    .requestIdToken(getString(R.string.client_id))      // TODO Très important, mauvaise valeur = marche pas
                    .requestEmail()
                    .build();
            client = GoogleSignIn.getClient(this, gso);
            client.signOut();

            mAuth.signOut();

            service.logout().enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                    Intent i = new Intent(getApplicationContext(), LandingActivity.class);

                    sharedPrefUtil.clearCurrentUser();

                    i.putExtra("loggedOut", true);
                    startActivity(i);
                    finish();
                }

                @Override
                public void onFailure(Call<String> call, Throwable t) {

                    Log.e("RETROFIT", t.getMessage());
                    Toast.makeText(MainActivity.this, "An error occured", Toast.LENGTH_SHORT).show();
                }
            });

        });
    }

    public void JoinQuiz(View v){
        EditText editTextCode = binding.editTextCode;

        String shareCode = editTextCode.getText().toString().toUpperCase();
        if (shareCode.equals("")){
            Toast.makeText(this, R.string.toastEnterCode, Toast.LENGTH_SHORT).show();
        }
        else {
            service.GetObjectByShareCode(shareCode).enqueue(new Callback<Object>() {
                @Override
                public void onResponse(Call<Object> call, Response<Object> response) {
                    if (response.isSuccessful()){

                        if (response.body().getClass().getName().equals(String.class.getName())) {
                            Log.i("TEST", "LA RÉPONSE EST UN STRING");

                            String token = SharedPrefUtil.getTokenFromCookie(getApplicationContext(), response.raw().request().url().host());

                            Intent intent = new Intent(getApplicationContext(), WaitingRoomActivity.class);
                            intent.putExtra("shareCode", shareCode);
                            intent.putExtra("token", token);
                            startActivity(intent);
                        }
                        else {
                            Gson gson = new Gson();
                            String json = gson.toJson(response.body());
                            QuizResponseDTO quiz = gson.fromJson(json, QuizResponseDTO.class);


                            Intent intent = new Intent(getApplicationContext(), QuizActivity.class);
                            intent.putExtra("quiz", quiz);
                            startActivity(intent);
                        }

                    }
                    else {
                        Toast.makeText(MainActivity.this, R.string.toastNoQuizFound, Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<Object> call, Throwable t) {
                    Log.e("RETROFIT", t.getMessage());
                }
            });
        }
    }

    @Override
    public void onBackPressed() {
        moveTaskToBack(false);
    }

    @Override
    protected void onStart() {
        super.onStart();
        // Firebase - Vérifier si déjà connecté
        mAuth = FirebaseAuth.getInstance();
    }
}
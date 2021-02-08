package org.equipe4.quizplay;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.databinding.ActivityMainBinding;
import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.QuizResponseDTO;
import org.equipe4.quizplay.transfer.UserDTO;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity {

    private ActivityMainBinding binding;
    private FirebaseAuth mAuth;
    private FirebaseUser mUser;
    private QPService service = RetrofitUtil.get();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        SharedPreferences sharedPreferences = this.getSharedPreferences("connectedUser", MODE_PRIVATE);
        String username = sharedPreferences.getString("username", "");
        String picture = sharedPreferences.getString("picture", "");

        binding.name.setText(username);
        Picasso.get().load(picture).into(binding.profilePic);

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
                    SharedPreferences.Editor editor = sharedPreferences.edit();
                    editor.apply();

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

        //Bouton join quiz
        binding.buttonJoin.setOnClickListener(v -> {
            EditText editTextCode = binding.editTextCode;

            if (editTextCode.getText().toString().equals("")){
                Toast.makeText(this, R.string.toastEnterCode, Toast.LENGTH_SHORT).show();
            }
            else {
                service.getQuizByCode(editTextCode.getText().toString().toUpperCase()).enqueue(new Callback<QuizResponseDTO>() {
                    @Override
                    public void onResponse(Call<QuizResponseDTO> call, Response<QuizResponseDTO> response) {
                        if (response.isSuccessful()){
                            Intent intent = new Intent(getApplicationContext(), WaitingRoomActivity.class);
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
        });
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
        mUser = mAuth.getCurrentUser();
//        if (mUser == null) {
//            Intent i = new Intent(getApplicationContext(), LandingActivity.class);
//            startActivity(i);
//            finish();
//        }
//        else {
//            mUser.getIdToken(true)
//                .addOnCompleteListener(task -> {
//                    if (task.isSuccessful()) {
//
//                        // TODO Envoie le token Firebase à notre serveur .NET
//                        String idToken = task.getResult().getToken();
//                        //Toast.makeText(getApplicationContext(), "token to server " + idToken, Toast.LENGTH_SHORT).show();
//
//                        service.login("\"" + idToken + "\"").enqueue(new Callback<UserDTO>() {
//                            @Override
//                            public void onResponse(Call<UserDTO> call, Response<UserDTO> response) {
//                                Log.i("REQUEST","Ok " + response.body());
//
//                            }
//
//                            @Override
//                            public void onFailure(Call<UserDTO> call, Throwable t) {
//                                Toast.makeText(MainActivity.this, "Ko " + t.getMessage(), Toast.LENGTH_SHORT).show();
//                            }
//                        });
//                    } else {
//                        // Handle error -> task.getException();
//                    }
//                });
//        }
    }
}
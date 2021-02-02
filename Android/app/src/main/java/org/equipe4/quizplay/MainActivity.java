package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.databinding.ActivityMainBinding;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.UserDTO;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity {

    private ActivityMainBinding binding;
    private FirebaseAuth mAuth;
    private FirebaseUser mUser;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        // TODO Envoyer un token Firebase pour vérification serveur avec le compte sur lequel on est connecté.

        binding.btnServerRequest.setOnClickListener(view -> {
            FirebaseUser mUser = mAuth.getCurrentUser();
            if (mUser != null) {
                mUser.getIdToken(true)
                        .addOnCompleteListener(task -> {
                                    if (task.isSuccessful()) {

                                        // TODO Envoie le token Firebase à notre serveur .NET
                                        String idToken = task.getResult().getToken();
                                        //Toast.makeText(getApplicationContext(), "token to server " + idToken, Toast.LENGTH_SHORT).show();
                                        new RetrofitUtil().service.Login(idToken).enqueue(new Callback<UserDTO>() {
                                            @Override
                                            public void onResponse(Call<UserDTO> call, Response<UserDTO> response) {
                                                Log.i("REQUEST","Ok " + response.body());
                                                UserDTO user = response.body();

                                                TextView name = findViewById(R.id.name);
                                                name.setText(user.name);
                                                ImageView profilePic = findViewById(R.id.profilePic);
                                                Picasso.get().load(user.picture).into(profilePic);
                                            }

                                            @Override
                                            public void onFailure(Call<UserDTO> call, Throwable t) {
                                                Toast.makeText(MainActivity.this, "Ko " + t.getMessage(), Toast.LENGTH_SHORT).show();
                                            }
                                        });
                                    } else {
                                        // Handle error -> task.getException();
                                    }
                                }
                        );
            }

        });


    }


    @Override
    protected void onStart() {
        super.onStart();
        // Firebase - Vérifier si déjà connecté
        mAuth = FirebaseAuth.getInstance();
        mUser = mAuth.getCurrentUser();
        if (mUser == null) {
            Toast.makeText(this, "Not logged firebase", Toast.LENGTH_SHORT).show();
        }

        if (mUser != null) {
            binding.TextViewUser.setText(mUser.getDisplayName());
        }
    }
}
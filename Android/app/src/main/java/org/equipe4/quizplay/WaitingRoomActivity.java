package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.databinding.ActivityMainBinding;
import org.equipe4.quizplay.databinding.ActivityWaitingRoomBinding;
import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.UserDTO;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class WaitingRoomActivity extends AppCompatActivity {


    static String pseudo;
    private ActivityWaitingRoomBinding binding;
    private FirebaseAuth mAuth;
    private FirebaseUser mUser;
    private QPService service = RetrofitUtil.get();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityWaitingRoomBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        Intent intent = getIntent();
        pseudo = intent.getStringExtra("playerPseudo");


        TextView textViewPseudo = findViewById(R.id.textViewPseudo);
        textViewPseudo.setText(pseudo);
    }

    @Override
    protected void onStart() {
        super.onStart();
        // Firebase - Vérifier si déjà connecté
        mAuth = FirebaseAuth.getInstance();
        mUser = mAuth.getCurrentUser();
        if (mUser != null) {
            mUser.getIdToken(true)
                    .addOnCompleteListener(task -> {
                        if (task.isSuccessful()) {

                            // TODO Envoie le token Firebase à notre serveur .NET
                            String idToken = task.getResult().getToken();
                            //Toast.makeText(getApplicationContext(), "token to server " + idToken, Toast.LENGTH_SHORT).show();

                            service.login("\"" + idToken + "\"").enqueue(new Callback<UserDTO>() {
                                @Override
                                public void onResponse(Call<UserDTO> call, Response<UserDTO> response) {
                                    Log.i("REQUEST","Ok " + response.body());
                                    UserDTO user = response.body();
                                    TextView name = binding.textViewPseudo;
                                    name.setText(user.name);
                                }

                                @Override
                                public void onFailure(Call<UserDTO> call, Throwable t) {
                                    Toast.makeText(WaitingRoomActivity.this, "Ko " + t.getMessage(), Toast.LENGTH_SHORT).show();
                                }
                            });
                        } else {
                            // Handle error -> task.getException();
                        }
                    });
        }

    }
}
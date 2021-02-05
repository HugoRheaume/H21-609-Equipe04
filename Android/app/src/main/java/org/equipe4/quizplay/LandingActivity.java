package org.equipe4.quizplay;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.android.gms.common.api.ApiException;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.AuthCredential;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.auth.GoogleAuthProvider;

import org.equipe4.quizplay.databinding.ActivityLandingBinding;
import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.QuizResponseDTO;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LandingActivity extends AppCompatActivity {

    QPService service = RetrofitUtil.get();

    private static final int GOOGLE_SIGN_IN_CODE = 123;

    private FirebaseAuth mAuth;

    private ActivityLandingBinding binding;

    private GoogleSignInClient mGoogleSignInClient;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityLandingBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        binding.signInButton.setOnClickListener(v -> {
            startSignIn();
        });

    }

    private void startSignIn() {
        GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestIdToken(getString(R.string.client_id))      // TODO Très important, mauvaise valeur = marche pas
                .requestEmail()
                .build();
        mGoogleSignInClient = GoogleSignIn.getClient(this, gso);

        // Ce intent part l'application Google qui va gérer l'authentification
        Intent signInIntent = mGoogleSignInClient.getSignInIntent();
        startActivityForResult(signInIntent, GOOGLE_SIGN_IN_CODE);
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        // Result returned from launching the Intent from GoogleSignInClient.getSignInIntent(...);
        if (requestCode == GOOGLE_SIGN_IN_CODE) {
            Task<GoogleSignInAccount> task = GoogleSignIn.getSignedInAccountFromIntent(data);
            handleSignInResult(task);
        }

        //Si plusieurs activités, d'autres IF avec codes différents

    }



    private void handleSignInResult(Task<GoogleSignInAccount> completedTask) {
        try {
            // On a le compte google et toutes les informations
            GoogleSignInAccount account = completedTask.getResult(ApiException.class);
            firebaseAuthWithGoogle(account.getIdToken());
        } catch (ApiException e) {
            // The ApiException status code indicates the detailed failure reason.
            // Please refer to the GoogleSignInStatusCodes class reference for more information.
            e.printStackTrace();

        }
    }

    private void firebaseAuthWithGoogle(String idToken) {
        AuthCredential credential = GoogleAuthProvider.getCredential(idToken, null);
        // TODO C'est ici qu'on envoie la requête à Firebase avec le token google
        mAuth.signInWithCredential(credential)
                .addOnCompleteListener(this, task -> {
                    if (task.isSuccessful()) {
                        // On est connecté
                        FirebaseUser user = mAuth.getCurrentUser();
                        Intent i = new Intent(getApplicationContext(), MainActivity.class);
                        startActivity(i);
                        finish();
                    } else {
                        Toast.makeText(LandingActivity.this, "Authentication Failed.", Toast.LENGTH_SHORT).show();
                    }
                });
    }

    @Override
    protected void onStart() {
        super.onStart();
        // Firebase - Vérifier si déjà connecté
        mAuth = FirebaseAuth.getInstance();
        FirebaseUser currentUser = mAuth.getCurrentUser();
    }

    public void JoinQuiz(View v){
        EditText editTextCode = binding.editTextCode;

        if (editTextCode.getText().toString().equals("")){
            Toast.makeText(this, R.string.toastEnterCode, Toast.LENGTH_SHORT).show();
        }
        else {
            service.getQuizByCode(editTextCode.getText().toString().toUpperCase()).enqueue(new Callback<QuizResponseDTO>() {
                @Override
                public void onResponse(Call<QuizResponseDTO> call, Response<QuizResponseDTO> response) {
                    if (response.isSuccessful()){
                        Intent intent = new Intent(getApplicationContext(), PseudoActivity.class);
                        startActivity(intent);
                        finish();
                    }
                    else {
                        Toast.makeText(LandingActivity.this, R.string.toastNoQuizFound, Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<QuizResponseDTO> call, Throwable t) {
                    Log.e("RETROFIT", t.getMessage());
                }
            });
        }
    }
}
package org.equipe4.quizplay;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.franmontiel.persistentcookiejar.PersistentCookieJar;
import com.franmontiel.persistentcookiejar.cache.SetCookieCache;
import com.franmontiel.persistentcookiejar.persistence.SharedPrefsCookiePersistor;
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
import org.equipe4.quizplay.model.http.QPService;
import org.equipe4.quizplay.model.http.RetrofitUtil;
import org.equipe4.quizplay.model.transfer.UserDTO;
import org.equipe4.quizplay.model.util.Global;
import org.equipe4.quizplay.model.util.SharedPrefUtil;
import org.equipe4.quizplay.model.webSocket.WSClient;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LandingActivity extends AppCompatActivity {

    QPService service;

    private static final int GOOGLE_SIGN_IN_CODE = 123;

    private FirebaseAuth mAuth;

    private ActivityLandingBinding binding;

    private GoogleSignInClient mGoogleSignInClient;

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        binding = ActivityLandingBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        manageExtras();

        WSClient.disconnect();

        //region Button events

        //Login Google
        binding.signInButton.setOnClickListener(v -> {
            startSignIn();
        });

        //Login Guest
        binding.btnGuestLogin.setOnClickListener(v -> {
            Intent intent = new Intent(this, PseudoActivity.class);
            startActivity(intent);
        });

        //endregion
    }

    private void manageExtras() {
        String toastMessage = getIntent().getStringExtra("message");
        if (toastMessage != null) {
            Toast.makeText(this, toastMessage, Toast.LENGTH_SHORT).show();
        }

        if (getIntent().getBooleanExtra("expired", false)) {
            Global.logout(getApplicationContext());
        }
    }


    @Override
    protected void onStart() {
        super.onStart();
        if (RetrofitUtil.cookieJar == null)
            RetrofitUtil.cookieJar = new PersistentCookieJar(new SetCookieCache(), new SharedPrefsCookiePersistor(this.getApplicationContext()));
        service = RetrofitUtil.get();
        // Firebase - Vérifier si déjà connecté
        verifyConnected();
    }

    private void verifyConnected() {
        mAuth = FirebaseAuth.getInstance();
        FirebaseUser currentUser = mAuth.getCurrentUser();
        if (currentUser != null) {
            Intent i = new Intent(getApplicationContext(), ListQuizActivity.class);
            startActivity(i);
            finish();
        } else {
            SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(getApplicationContext());

            if (sharedPrefUtil.getCurrentUser().isAnonymous)
                Global.logout(getApplicationContext());

            sharedPrefUtil.clearCurrentUser();
        }
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        // Result returned from launching the Intent from GoogleSignInClient.getSignInIntent(...);
        if (requestCode == GOOGLE_SIGN_IN_CODE) {
            Task<GoogleSignInAccount> task = GoogleSignIn.getSignedInAccountFromIntent(data);
            handleSignInResult(task);
        }
    }

    //region SignIn with Google Méthode
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

    // endregion

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
                    mAuth.getCurrentUser().getIdToken(true).addOnCompleteListener(taskToken -> {
                        if (taskToken.isSuccessful()) {

                            String firebaseToken = taskToken.getResult().getToken();

                            service.login("\"" + firebaseToken + "\"").enqueue(new Callback<UserDTO>() {
                                @Override
                                public void onResponse(Call<UserDTO> call, Response<UserDTO> response) {
                                    UserDTO user = response.body();

                                    Intent i = new Intent(getApplicationContext(), ListQuizActivity.class);

                                    SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
                                    sharedPrefUtil.setCurrentUser(user);

                                    //i.putExtra("name", (Serializable) user);
                                    //i.putExtra("picture", user.picture);

                                    startActivity(i);
                                    finish();
                                }

                                @Override
                                public void onFailure(Call<UserDTO> call, Throwable t) {
                                    Toast.makeText(LandingActivity.this, "Erreur " + t.getMessage(), Toast.LENGTH_SHORT).show();
                                }
                            });

                        }
                    });

                } else {
                    Toast.makeText(LandingActivity.this, "Authentication Failed.", Toast.LENGTH_SHORT).show();
                }
            });
    }
}
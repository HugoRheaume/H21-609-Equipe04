package org.equipe4.quizplay.util;

import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.firebase.auth.FirebaseAuth;

import org.equipe4.quizplay.LandingActivity;
import org.equipe4.quizplay.R;
import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class Global {

    static SharedPrefUtil sharedPrefUtil;
    static QPService service = RetrofitUtil.get();

    static public void logout(Context context){
        FirebaseAuth auth = FirebaseAuth.getInstance();
        sharedPrefUtil = new SharedPrefUtil(context);
        GoogleSignInClient client;
        GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestIdToken(context.getString(R.string.client_id))      // TODO Très important, mauvaise valeur = marche pas
                .requestEmail()
                .build();
        client = GoogleSignIn.getClient(context, gso);
        client.signOut();

        auth.signOut();

        if (sharedPrefUtil.getCurrentUser().isAnonymous)
            logoutAnonymous(context);
        else
            logoutConnected(context);
    }

    static private void logoutConnected(Context context){
        service.logout().enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String> call, Response<String> response) {
                Intent i = new Intent(context, LandingActivity.class);

                sharedPrefUtil.clearCurrentUser();

                i.putExtra("loggedOut", true);
                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                context.startActivity(i);
            }

            @Override
            public void onFailure(Call<String> call, Throwable t) {

                Log.e("RETROFIT", t.getMessage());
                Toast.makeText(context, "An error occured", Toast.LENGTH_SHORT).show();
            }
        });
    }

    static private void logoutAnonymous(Context context){
        service.logoutAnonymous().enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                if (response.isSuccessful()) {
                    Intent i = new Intent(context, LandingActivity.class);

                    sharedPrefUtil.clearCurrentUser();

                    i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                    i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                    context.startActivity(i);
                }
            }
            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {
                Toast.makeText(context, "Un erreur est survenu", Toast.LENGTH_SHORT).show();
            }
        });
    }

}

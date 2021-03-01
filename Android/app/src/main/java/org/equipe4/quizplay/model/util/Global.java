package org.equipe4.quizplay.model.util;

import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.firebase.auth.FirebaseAuth;

import org.equipe4.quizplay.activityDeferredQuiz.AssociationActivity;
import org.equipe4.quizplay.LandingActivity;
import org.equipe4.quizplay.R;
import org.equipe4.quizplay.activityDeferredQuiz.MultipleChoiceActivity;
import org.equipe4.quizplay.activityDeferredQuiz.TrueFalseActivity;
import org.equipe4.quizplay.activityLiveQuiz.LiveMultipleChoiceActivity;
import org.equipe4.quizplay.activityLiveQuiz.LiveTrueFalseActivity;
import org.equipe4.quizplay.model.http.RetrofitUtil;

import java.net.URI;
import java.net.URISyntaxException;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class Global {


    public static final String IP_PROD = "https://api.e4.projet.college-em.info/api/";
    public static final String IP_MARCO = "https://192.168.0.136:45455/api/";
    public static final String IP_XAV = "https://192.168.0.172:45455/api/";
    public static final String IP_NIC = "https://192.168.2.28:45455/api/";
    public static final String IP_HUGO = "https://192.168.0.131:45455/api/";
    public static final String IP_RENAUD = "https://192.168.0.107:45455/api/";

    public static final String SERVER_PROD = "wss://api.e4.projet.college-em.info/websocket/connect";
    public static final String SERVER_MARCO = "wss://192.168.0.136:45455/websocket/connect";
    public static final String SERVER_XAV = "wss://192.168.0.172:45455/websocket/connect";
    public static final String SERVER_HUGO = "wss://192.168.0.131:45455/websocket/connect";
    public static final String SERVER_RENAUD = "wss://192.168.0.107:45455/websocket/connect";


    public static String getCurrentUri() {

        URI uri = null;
        try {
            uri = new URI(RetrofitUtil.SERVER_URL);
        } catch (URISyntaxException e) {
            e.printStackTrace();
        }

        return uri.getHost();
    }

    static public void logout(Context context){
        FirebaseAuth auth = FirebaseAuth.getInstance();
        org.equipe4.quizplay.model.util.SharedPrefUtil sharedPrefUtil = new org.equipe4.quizplay.model.util.SharedPrefUtil(context);
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
        RetrofitUtil.get().logout().enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String> call, Response<String> response) {
                Intent i = new Intent(context, LandingActivity.class);
                SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(context);
                sharedPrefUtil.clearCurrentUser();

                i.putExtra("loggedOut", true);
                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                context.startActivity(i);
            }

            @Override
            public void onFailure(Call<String> call, Throwable t) {

                Log.e("RETROFIT", t.getMessage());
                Toast.makeText(context, context.getString(R.string.toastNoAcess), Toast.LENGTH_SHORT).show();
            }
        });
    }

    static private void logoutAnonymous(Context context){
        RetrofitUtil.get().logoutAnonymous().enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                if (response.isSuccessful()) {
                    Intent i = new Intent(context, LandingActivity.class);
                    SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(context);
                    sharedPrefUtil.clearCurrentUser();

                    i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                    i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                    context.startActivity(i);
                }
            }
            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {
                Toast.makeText(context, context.getString(R.string.toastNoAcess), Toast.LENGTH_SHORT).show();
            }
        });
    }

    public static Class<? extends AppCompatActivity> getQuestionTypeDeferred(int questionType) {
        switch (questionType) {
            case 1:
                return TrueFalseActivity.class;
            case 2:
                return MultipleChoiceActivity.class;
            case 3:
                return AssociationActivity.class;
            default:
                break;
        }
        return null;
    }


    public static Class<? extends AppCompatActivity> getQuestionTypeLive(int questionType) {
        switch (questionType) {
            case 1:
                return LiveTrueFalseActivity.class;
            case 2:
                return LiveMultipleChoiceActivity.class;
            default:
                break;
        }
        return null;
    }

}

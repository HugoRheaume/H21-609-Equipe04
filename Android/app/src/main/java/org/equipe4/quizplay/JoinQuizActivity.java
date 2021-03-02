package org.equipe4.quizplay;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.drawerlayout.widget.DrawerLayout;

import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.material.navigation.NavigationView;
import com.google.firebase.auth.FirebaseAuth;
import com.google.gson.Gson;
import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.activityDeferredQuiz.QuizActivity;
import org.equipe4.quizplay.activityLiveQuiz.WaitingRoomActivity;
import org.equipe4.quizplay.databinding.ActivityJoinQuizBinding;
import org.equipe4.quizplay.model.http.QPService;
import org.equipe4.quizplay.model.http.RetrofitUtil;
import org.equipe4.quizplay.model.transfer.QuizResponseDTO;
import org.equipe4.quizplay.model.transfer.UserDTO;
import org.equipe4.quizplay.model.util.Global;
import org.equipe4.quizplay.model.util.SharedPrefUtil;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class JoinQuizActivity extends AppCompatActivity {

    private ActivityJoinQuizBinding binding;
    QPService service;
    SharedPrefUtil sharedPrefUtil;
    UserDTO user;
    ActionBarDrawerToggle toggle;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityJoinQuizBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        runOnUiThread(() -> {
            service = RetrofitUtil.get();
        });
        sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
        user = sharedPrefUtil.getCurrentUser();

        configureDrawer();
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

                            if (quiz.numberOfQuestions == 0) {
                                Toast.makeText(JoinQuizActivity.this, getString(R.string.toastNoQuestionsInQuiz), Toast.LENGTH_SHORT).show();
                            }
                            else {
                                Intent intent = new Intent(getApplicationContext(), QuizActivity.class);
                                intent.putExtra("quiz", quiz);
                                startActivity(intent);
                            }
                        }
                    }
                    else {
                        Toast.makeText(getApplicationContext(), R.string.toastNoQuizFound, Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<Object> call, Throwable t) {
                    Log.e("RETROFIT", t.getMessage());
                    Toast.makeText(JoinQuizActivity.this, getString(R.string.toastNoAcess), Toast.LENGTH_SHORT).show();
                }
            });
        }
    }

    private void configureDrawer(){
        //Toolbar
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        NavigationView nav_view = (NavigationView) findViewById(R.id.nav_view);
        final DrawerLayout drawer_layout = (DrawerLayout) findViewById(R.id.drawer_layout);

        View layout = nav_view.getHeaderView(0);
        View txtViewHeader = layout.findViewById(R.id.header);

        ImageView profilePic = txtViewHeader.findViewById(R.id.profilePicDrawer);
        Picasso.get().load(user.picture).into(profilePic);

        TextView HeaderTitle = txtViewHeader.findViewById(R.id.txtViewCurrentUser);
        HeaderTitle.setText(user.name);

        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        nav_view.setNavigationItemSelectedListener(new NavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem item) {

                switch(item.getItemId()){
                    case R.id.navigation_item_list_quiz:
                        startActivity(new Intent(getApplicationContext(), ListQuizActivity.class));
                        break;

                    case R.id.navigation_item_join:
                        break;

                    case R.id.navigation_item_logout:
                        Global.logout(getApplicationContext());
                        break;
                }

                drawer_layout.closeDrawers();
                return false;
            }
        });

        toggle = new ActionBarDrawerToggle(this, drawer_layout, R.string.open_drawer, R.string.close_drawer);
        drawer_layout.addDrawerListener(toggle);
        toggle.syncState();
    }

    //Drawer Override Methode
    @Override
    public boolean onOptionsItemSelected(@NonNull MenuItem item) {
        if (toggle.onOptionsItemSelected(item)){
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onPostCreate(@Nullable Bundle savedInstanceState) {
        super.onPostCreate(savedInstanceState);
        toggle.syncState();
    }

    @Override
    public void onConfigurationChanged(@NonNull Configuration newConfig) {
        toggle.onConfigurationChanged(newConfig);
        super.onConfigurationChanged(newConfig);
    }
}
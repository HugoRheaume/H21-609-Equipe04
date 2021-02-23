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
import android.widget.ImageView;
import android.widget.TextView;

import com.google.android.material.navigation.NavigationView;
import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.QuestionDTO;
import org.equipe4.quizplay.transfer.QuestionResultDTO;
import org.equipe4.quizplay.transfer.QuizResponseDTO;
import org.equipe4.quizplay.transfer.UserDTO;
import org.equipe4.quizplay.util.Global;
import org.equipe4.quizplay.util.SharedPrefUtil;
import org.w3c.dom.Text;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class QuizActivity extends AppCompatActivity {


    QPService service = RetrofitUtil.get();
    QuizResponseDTO quiz;
    SharedPrefUtil sharedPrefUtil;
    UserDTO user;
    ActionBarDrawerToggle toggle;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_quiz);

        sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
        user = sharedPrefUtil.getCurrentUser();

        quiz = (QuizResponseDTO)getIntent().getSerializableExtra("quiz");

        TextView quizTitle = findViewById(R.id.quizTitle);
        quizTitle.setText(quiz.title);
        TextView quizDesc = findViewById(R.id.quizDesc);
        quizDesc.setText(quiz.description);

        configureDrawer();
    }

    public void startQuiz(View v) {
        service.getNextQuestion(new QuestionResultDTO(-1, quiz.id,0)).enqueue(new Callback<QuestionDTO>() {
            @Override
            public void onResponse(Call<QuestionDTO> call, Response<QuestionDTO> response) {
                if(response.isSuccessful()) {
                    QuestionDTO question = response.body();
                    Log.i("Response", response.body().toString());

                    Intent i;
                    switch (question.questionType) {
                        case 1:
                            i = new Intent(getApplicationContext(), TrueFalseActivity.class);
                            break;
                        case 2:
                            i = new Intent(getApplicationContext(), MultipleChoiceActivity.class);
                            break;
                        case 3:
                            i = new Intent(getApplicationContext(), AssociationActivity.class);
                            break;
                        default:
                            return;
                    }
                    i.putExtra("question", question);
                    i.putExtra("quiz", quiz);
                    startActivity(i);
                } else {
                    Log.i("RETROFIT", response.message());
                }
            }

            @Override
            public void onFailure(Call<QuestionDTO> call, Throwable t) {
                Log.e("RETROFIT", t.getMessage());
            }
        });
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
                        startActivity(new Intent(getApplicationContext(), JoinQuizActivity.class));
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
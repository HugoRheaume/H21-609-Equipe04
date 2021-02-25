package org.equipe4.quizplay.activityLiveQuiz;

import android.content.Intent;
import android.content.res.Configuration;
import android.content.res.Resources;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.drawerlayout.widget.DrawerLayout;

import com.google.android.material.navigation.NavigationView;
import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.JoinQuizActivity;
import org.equipe4.quizplay.LandingActivity;
import org.equipe4.quizplay.ListQuizActivity;
import org.equipe4.quizplay.R;
import org.equipe4.quizplay.databinding.ActivityWaitingRoomBinding;
import org.equipe4.quizplay.model.http.QPService;
import org.equipe4.quizplay.model.http.RetrofitUtil;
import org.equipe4.quizplay.model.transfer.QuizResponseDTO;
import org.equipe4.quizplay.model.transfer.UserDTO;
import org.equipe4.quizplay.model.util.Global;
import org.equipe4.quizplay.model.util.SharedPrefUtil;
import org.equipe4.quizplay.model.webSocket.WSClient;
import org.equipe4.quizplay.model.webSocket.WebSocketEventListener;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation.JoinRoomCommand;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation.LeaveRoomCommand;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.resultCommand.NextQuizResponse;

import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.util.concurrent.ExecutionException;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class WaitingRoomActivity extends AppCompatActivity {

    ActivityWaitingRoomBinding binding;
    WSClient client;
    QuizResponseDTO currentQuiz;
    ActionBarDrawerToggle toggle;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        binding = ActivityWaitingRoomBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        try {
            client = new WSClient();
        } catch (KeyManagementException | NoSuchAlgorithmException | ExecutionException | InterruptedException e) {
            e.printStackTrace();
        }

        SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(getApplicationContext());

        if(!sharedPrefUtil.getCurrentUser().isAnonymous) {
            binding.btnChangeUsername.setVisibility(View.GONE);
        }

        binding.txtWelcome.setText(getString(R.string.welcome,sharedPrefUtil.getCurrentUser().name));

        if (savedInstanceState != null) {
            currentQuiz = (QuizResponseDTO)savedInstanceState.getSerializable("currentQuiz");
            setUiInfo();
        }

        setWebSocketEvents();
        configureDrawer();
        // Événement click pour changer de pseudo
        binding.btnChangeUsername.setOnClickListener(v -> {
            leaveRoom();
            finish();
        });
    }

    // Set websocket event listeners
    private void setWebSocketEvents() {
        JoinRoomCommand joinRoomCommand = new JoinRoomCommand(getIntent().getStringExtra("token"), getIntent().getStringExtra("shareCode"));
        client.setEventListener(new WebSocketEventListener() {
            @Override
            public void onRoomJoin(QuizResponseDTO quiz) {
                currentQuiz = quiz;
                setUiInfo();
            }

            @Override
            public void onRoomDeleted() {
                leaveRoom();
                Intent i = new Intent(getApplicationContext(), ListQuizActivity.class);
                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                i.putExtra("message", getString(R.string.toastRoomDeleted));
                startActivity(i);
            }

            @Override
            public void onNextQuestion(NextQuizResponse nextQuizResponse) {
                Intent i = new Intent(getApplicationContext(), Global.getQuestionTypeLive(nextQuizResponse.question.questionType));
                i.putExtra("question", nextQuizResponse.question);
                i.putExtra("currentQuiz", currentQuiz);
                startActivity(i);
            }
        });
        client.joinRoom(joinRoomCommand);
    }

    // Quitte la Room et supprime l'utilisateur si il est anonyme
    private void leaveRoom() {
        LeaveRoomCommand leaveRoomCommand = new LeaveRoomCommand(getIntent().getStringExtra("token"), getIntent().getStringExtra("shareCode"));
        client.leaveRoom(leaveRoomCommand);
    }

    // region Configure drawer

    private void configureDrawer(){
        //Toolbar
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        NavigationView nav_view = (NavigationView) findViewById(R.id.nav_view);
        final DrawerLayout drawer_layout = (DrawerLayout) findViewById(R.id.drawer_layout);

        View layout = nav_view.getHeaderView(0);
        View txtViewHeader = layout.findViewById(R.id.header);

        UserDTO user = new SharedPrefUtil(getApplicationContext()).getCurrentUser();
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

    // endregion

    private void setUiInfo() {
        Resources res = getResources();
        binding.txtQuizName.setText(currentQuiz.title);
        binding.txtDescription.setText(currentQuiz.description);
        binding.txtNbQuestion.setText(res.getQuantityString(R.plurals.number_of_questions, currentQuiz.numberOfQuestions, currentQuiz.numberOfQuestions));
    }


    @Override
    public void onBackPressed() {
        leaveRoom();
        Intent i = new Intent(getApplicationContext(), ListQuizActivity.class);
        i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivity(i);
        super.onBackPressed();
    }

    @Override
    protected void onSaveInstanceState(@NonNull Bundle outState) {
        super.onSaveInstanceState(outState);

        outState.putSerializable("currentQuiz", currentQuiz);
    }
}
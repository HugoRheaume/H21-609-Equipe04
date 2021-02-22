package org.equipe4.quizplay;

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

import org.equipe4.quizplay.databinding.ActivityWaitingRoomBinding;
import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.QuizResponseDTO;
import org.equipe4.quizplay.transfer.UserDTO;
import org.equipe4.quizplay.util.Global;
import org.equipe4.quizplay.util.SharedPrefUtil;
import org.equipe4.quizplay.webSocket.WSClient;
import org.equipe4.quizplay.webSocket.WebSocketEventListener;
import org.equipe4.quizplay.webSocket.webSocketCommand.commandImplementation.JoinRoomCommand;
import org.equipe4.quizplay.webSocket.webSocketCommand.commandImplementation.LeaveRoomCommand;

import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.util.List;
import java.util.concurrent.ExecutionException;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class WaitingRoomActivity extends AppCompatActivity {

    ActivityWaitingRoomBinding binding;
    WSClient client;
    QPService service = RetrofitUtil.get();
    SharedPrefUtil sharedPrefUtil;
    UserDTO user;
    ActionBarDrawerToggle toggle;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityWaitingRoomBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
        user = sharedPrefUtil.getCurrentUser();

        if(!sharedPrefUtil.getCurrentUser().isAnonymous) {
            binding.btnChangeUsername.setVisibility(View.GONE);
        }

        binding.txtWelcome.setText(getString(R.string.welcome,sharedPrefUtil.getCurrentUser().name));
        try {
            JoinRoomCommand joinRoomCommand = new JoinRoomCommand(getIntent().getStringExtra("token"), getIntent().getStringExtra("shareCode"));
            client = new WSClient();

            client.setEventListener(new WebSocketEventListener() {
                @Override
                public void OnUserState(List<UserDTO> listUsers) {
                    Resources res = getResources();
                    binding.txtUserCount.setText(res.getQuantityString(R.plurals.participants, listUsers.size(), listUsers.size()));
                }

                @Override
                public void OnRoomJoin(QuizResponseDTO quiz) {
                    Resources res = getResources();
                    binding.txtQuizName.setText(quiz.title);
                    binding.txtDescription.setText(quiz.description);
                    binding.txtNbQuestion.setText(res.getQuantityString(R.plurals.number_of_questions, quiz.numberOfQuestions, quiz.numberOfQuestions));
                }

                @Override
                public void OnRoomDeleted() {
                    leaveRoom();
                    Intent i = new Intent(getApplicationContext(), LandingActivity.class);
                    i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                    i.putExtra("message", "La salle a été supprimée");
                    startActivity(i);
                }
            });

            client.joinRoom(joinRoomCommand);
        } catch (NoSuchAlgorithmException | KeyManagementException | InterruptedException | ExecutionException e) {
            e.printStackTrace();
        }

        // Événement click pour changer de pseudo
        binding.btnChangeUsername.setOnClickListener(v -> {
            leaveRoom();
            finish();
        });

        configureDrawer();
    }

    // Quitte la Room et supprime l'utilisateur si il est anonyme
    private void leaveRoom() {
        LeaveRoomCommand leaveRoomCommand = new LeaveRoomCommand(getIntent().getStringExtra("token"), getIntent().getStringExtra("shareCode"));

        SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(getApplicationContext());

        UserDTO user =  sharedPrefUtil.getCurrentUser();

        client.leaveRoom(leaveRoomCommand);
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
                        leaveRoom();
                        startActivity(new Intent(getApplicationContext(), ListQuizActivity.class));
                        break;

                    case R.id.navigation_item_join:
                        leaveRoom();
                        startActivity(new Intent(getApplicationContext(), JoinQuizActivity.class));
                        break;

                    case R.id.navigation_item_logout:
                        leaveRoom();
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

    @Override
    public void onBackPressed() {
        leaveRoom();
        Intent i = new Intent(getApplicationContext(), ListQuizActivity.class);
        i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivity(i);
        super.onBackPressed();
    }
}
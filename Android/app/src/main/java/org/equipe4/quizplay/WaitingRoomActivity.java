package org.equipe4.quizplay;

import android.content.Intent;
import android.content.res.Resources;
import android.os.Bundle;
import android.view.View;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import org.equipe4.quizplay.databinding.ActivityWaitingRoomBinding;
import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.QuizResponseDTO;
import org.equipe4.quizplay.transfer.UserDTO;
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
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityWaitingRoomBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());
        SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(getApplicationContext());

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
    }

    // Quitte la Room et supprime l'utilisateur si il est anonyme
    private void leaveRoom() {
        LeaveRoomCommand leaveRoomCommand = new LeaveRoomCommand(getIntent().getStringExtra("token"), getIntent().getStringExtra("shareCode"));

        SharedPrefUtil sharedPrefUtil = new SharedPrefUtil(getApplicationContext());

        UserDTO user =  sharedPrefUtil.getCurrentUser();

        client.leaveRoom(leaveRoomCommand);

        // Si l'utilisateur est anonyme, supprime l'utilisateur de la database
        if (user.isAnonymous){
            service.logoutAnonymous().enqueue(new Callback<Boolean>() {
                @Override
                public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                    if (response.isSuccessful()) {
                        if (response.body()) {
                            sharedPrefUtil.clearCurrentUser();
                        }
                    }
                }
                @Override
                public void onFailure(Call<Boolean> call, Throwable t) {
                    Toast.makeText(WaitingRoomActivity.this, "Un erreur est survenu", Toast.LENGTH_SHORT).show();
                }
            });
        }
    }

    @Override
    public void onBackPressed() {
        leaveRoom();
        Intent i = new Intent(getApplicationContext(), LandingActivity.class);
        i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivity(i);
        super.onBackPressed();
    }
}
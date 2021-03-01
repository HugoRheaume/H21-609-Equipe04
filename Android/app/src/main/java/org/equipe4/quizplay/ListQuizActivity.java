package org.equipe4.quizplay;

import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.franmontiel.persistentcookiejar.PersistentCookieJar;
import com.franmontiel.persistentcookiejar.cache.SetCookieCache;
import com.franmontiel.persistentcookiejar.persistence.SharedPrefsCookiePersistor;
import com.google.android.material.navigation.NavigationView;
import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.activityDeferredQuiz.QuizActivity;
import org.equipe4.quizplay.model.http.QPService;
import org.equipe4.quizplay.model.http.RetrofitUtil;
import org.equipe4.quizplay.model.service.AppKilledService;
import org.equipe4.quizplay.model.transfer.QuizResponseDTO;
import org.equipe4.quizplay.model.transfer.UserDTO;
import org.equipe4.quizplay.model.util.Global;
import org.equipe4.quizplay.model.util.SharedPrefUtil;
import org.equipe4.quizplay.model.webSocket.WSClient;

import java.util.Collections;
import java.util.Comparator;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ListQuizActivity extends AppCompatActivity implements AdapterView.OnItemSelectedListener {

    SharedPrefUtil sharedPrefUtil;
    UserDTO user;
    ActionBarDrawerToggle toggle;
    List<QuizResponseDTO> listQuiz;
    QPService service;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_list_quiz);

        service = RetrofitUtil.get();

        startService(new Intent(getApplicationContext(), AppKilledService.class));

        manageExtras();

        WSClient.disconnect();

        sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
        user = sharedPrefUtil.getCurrentUser();

        configureDrawer();

        Spinner dropdown = findViewById(R.id.dropdownSort);
        String[] items = new String[]{getString(R.string.quizTitle), getString(R.string.questionNumber), getString(R.string.author), getString(R.string.dateCreation)};
        ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_dropdown_item, items);
        dropdown.setAdapter(adapter);
        dropdown.setOnItemSelectedListener(this);

        service.getPublicQuiz().enqueue(new Callback<List<QuizResponseDTO>>() {
            @Override
            public void onResponse(Call<List<QuizResponseDTO>> call, Response<List<QuizResponseDTO>> response) {
                if (response.isSuccessful()) {
                    listQuiz = response.body();

                    sortListQuiz(getString(R.string.quizTitle));

                    initRecycler();
                }
                else {
                    Log.i("RETROFIT", response.message());
                }
            }

            @Override
            public void onFailure(Call<List<QuizResponseDTO>> call, Throwable t) {
                Toast.makeText(getApplicationContext(), getString(R.string.toastNoAcess), Toast.LENGTH_SHORT).show();
                Log.i("RETROFIT", t.getMessage());
            }
        });
    }

    private void sortListQuiz(String sort) {
        if (listQuiz == null) return;
        Collections.sort(listQuiz, (Comparator<QuizResponseDTO>) (o1, o2) -> {
            if (sort.equals(getString(R.string.quizTitle)))
                return o1.title.toLowerCase().compareTo(o2.title.toLowerCase());
            if (sort.equals(getString(R.string.questionNumber)))
                return o2.numberOfQuestions - o1.numberOfQuestions;
            if (sort.equals(getString(R.string.author)))
                return o1.author.toLowerCase().compareTo(o2.author.toLowerCase());
            if (sort.equals(getString(R.string.dateCreation)))
                return o2.date.compareTo(o1.date);
            return 0;
        });
    }

    @Override
    protected void onStart() {
        super.onStart();

        if (RetrofitUtil.cookieJar == null)
            RetrofitUtil.cookieJar = new PersistentCookieJar(new SetCookieCache(), new SharedPrefsCookiePersistor(this.getApplicationContext()));
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

    private void initRecycler() {
        RecyclerView quizRecyclerView = findViewById(R.id.quizRecyclerView);

        quizRecyclerView.setHasFixedSize(true);

        RecyclerView.LayoutManager quizLayoutManager = new LinearLayoutManager(this);
        quizRecyclerView.setLayoutManager(quizLayoutManager);

        RecyclerView.Adapter quizAdapter = new QuizAdapter(listQuiz, getApplicationContext());
        quizRecyclerView.setAdapter(quizAdapter);
    }

    public void startQuiz(View v) {
        QuizResponseDTO quiz = null;

        for(QuizResponseDTO q : listQuiz) {
            if (q.id == (int)v.getTag())
                quiz = q;
        }

        if (quiz == null) {
            Toast.makeText(getApplicationContext(), getString(R.string.toastErrorAccessingQuiz), Toast.LENGTH_SHORT).show();
            return;
        }

        Intent intent = new Intent(getApplicationContext(), QuizActivity.class);
        intent.putExtra("quiz", quiz);
        startActivity(intent);
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

    @Override
    public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
        String item = (String)parent.getItemAtPosition(position);
        if (listQuiz != null) {
            sortListQuiz(item);
            initRecycler();
        }
    }

    @Override
    public void onNothingSelected(AdapterView<?> parent) {

    }

    // endregion
}
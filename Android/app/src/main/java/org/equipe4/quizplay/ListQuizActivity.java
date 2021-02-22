package org.equipe4.quizplay;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.drawerlayout.widget.DrawerLayout;

import android.app.ActivityManager;
import android.content.ComponentName;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.material.navigation.NavigationView;
import com.squareup.picasso.Picasso;

import org.equipe4.quizplay.service.AppKilledService;
import org.equipe4.quizplay.transfer.UserDTO;
import org.equipe4.quizplay.util.Global;
import org.equipe4.quizplay.util.SharedPrefUtil;

public class ListQuizActivity extends AppCompatActivity {

    SharedPrefUtil sharedPrefUtil;
    UserDTO user;
    ActionBarDrawerToggle toggle;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_list_quiz);

        startService(new Intent(getApplicationContext(), AppKilledService.class));

        sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
        user = sharedPrefUtil.getCurrentUser();

        configureDrawer();
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
        HeaderTitle.setText(sharedPrefUtil.getCurrentUser().name);

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

        toggle = new ActionBarDrawerToggle(this, drawer_layout, R.string.open_drawer, R.string.close_drawer){
            @Override
            public void onDrawerClosed(View drawerView) {
                super.onDrawerClosed(drawerView);
            }

            @Override
            public void onDrawerOpened(View drawerView) {
                super.onDrawerOpened(drawerView);
            }
        };
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
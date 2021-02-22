package org.equipe4.quizplay.service;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;

import androidx.annotation.Nullable;

import org.equipe4.quizplay.transfer.UserDTO;
import org.equipe4.quizplay.util.Global;
import org.equipe4.quizplay.util.SharedPrefUtil;

public class AppKilledService extends Service {

    SharedPrefUtil sharedPrefUtil;
    UserDTO user;

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public void onTaskRemoved(Intent rootIntent) {
        super.onTaskRemoved(rootIntent);

        sharedPrefUtil = new SharedPrefUtil(getApplicationContext());
        user = sharedPrefUtil.getCurrentUser();

        if (user.isAnonymous)
            Global.logout(getApplicationContext());

        stopSelf();
    }
}

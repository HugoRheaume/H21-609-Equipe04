package org.equipe4.quizplay.util;

import android.content.Context;
import android.content.SharedPreferences;

import com.franmontiel.persistentcookiejar.persistence.SerializableCookie;

import org.equipe4.quizplay.transfer.UserDTO;

import okhttp3.Cookie;

import static android.content.Context.MODE_PRIVATE;

public class SharedPrefUtil {

    private final SharedPreferences sharedPreferences;

    public SharedPrefUtil(Context context) {
        sharedPreferences = context.getSharedPreferences("connectedUser", MODE_PRIVATE);
    }

    public UserDTO getCurrentUser(){
        UserDTO user = new UserDTO();
        user.name = sharedPreferences.getString("username", "");
        user.picture = sharedPreferences.getString("picture", "");
        user.email = sharedPreferences.getString("email", "");
        user.isAnonymous = sharedPreferences.getBoolean("isAnonymous", false);
        return user;
    }

    public void setCurrentUser(UserDTO user) {
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.putString("username", user.name);
        editor.putString("picture", user.picture);
        editor.putString("email", user.email);
        editor.putBoolean("isAnonymous", user.isAnonymous);
        editor.apply();
    }

    public void clearCurrentUser() {
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.clear();
        editor.apply();
    }


    public static String getTokenFromCookie(Context context, String hostName) {
        SerializableCookie cookieSer = new SerializableCookie();
        SharedPreferences sharedPreferences = context.getSharedPreferences("CookiePersistence", MODE_PRIVATE);
        Cookie cookie = cookieSer.decode(sharedPreferences.getString("https://" + hostName + "/|token", ""));
        return cookie.value();
    }
}

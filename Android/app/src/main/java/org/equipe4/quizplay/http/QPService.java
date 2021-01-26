package org.equipe4.quizplay.http;

import org.equipe4.quizplay.transfer.HelloWorldObj;

import retrofit2.Call;
import retrofit2.http.GET;

public interface QPService {
    @GET("HelloWorld")
    Call<HelloWorldObj> helloWorld();
}

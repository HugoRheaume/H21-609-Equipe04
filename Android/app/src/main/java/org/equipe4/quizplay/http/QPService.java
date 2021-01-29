package org.equipe4.quizplay.http;

import org.equipe4.quizplay.transfer.HelloWorldObj;
import org.equipe4.quizplay.transfer.QuizResponseDTO;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface QPService {
    @GET("HelloWorld")
    Call<HelloWorldObj> helloWorld();

    @GET("Quiz/GetQuizByCode")
    Call<QuizResponseDTO> getQuizByCode(@Query("code") String code);
}

package org.equipe4.quizplay.http;

import org.equipe4.quizplay.transfer.QuizResponseDTO;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface QPService {

    @GET("Quiz/GetQuizByCode")
    Call<QuizResponseDTO> getQuizByCode(@Query("code") String code);

    @POST("Login/Token")
    Call<String> Login(@Query("token") String token);
}

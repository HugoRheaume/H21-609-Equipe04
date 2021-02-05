package org.equipe4.quizplay.http;

import org.equipe4.quizplay.transfer.QuizResponseDTO;
import org.equipe4.quizplay.transfer.UserDTO;

import retrofit2.Call;
import retrofit2.Response;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.Headers;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface QPService {

    @GET("Quiz/GetQuizByCode")
    Call<QuizResponseDTO> getQuizByCode(@Query("code") String code);

    @POST("Auth/Login")
    @Headers("Content-Type: application/json")
    Call<UserDTO> login(@Body String token);

    @GET("Auth/Logout")
    Call<String> logout();
}

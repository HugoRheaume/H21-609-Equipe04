package org.equipe4.quizplay.model.http;

import org.equipe4.quizplay.model.transfer.QuestionDTO;
import org.equipe4.quizplay.model.transfer.QuestionResultDTO;
import org.equipe4.quizplay.model.transfer.QuizResponseDTO;
import org.equipe4.quizplay.model.transfer.UserDTO;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Headers;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface QPService {

    @GET("Quiz/GetObjectByShareCode/{code}")
    Call<Object> GetObjectByShareCode(@Path("code") String code);

    @POST("Question/GetNextQuestion")
    Call<QuestionDTO> getNextQuestion(@Body QuestionResultDTO result);

    @POST("Auth/GenerateAnonymousUser")
    @Headers("Content-Type: application/json")
    Call<UserDTO> generateAnonymousUser(@Body String username);

    @POST("Auth/Login")
    @Headers("Content-Type: application/json")
    Call<UserDTO> login(@Body String token);

    @GET("Auth/Logout")
    Call<String> logout();

    @GET("Auth/LogoutAnonymous")
    Call<Boolean> logoutAnonymous();

    @POST("Quiz/GetFinalScore")
    Call<Integer> getFinalScore(@Body QuizResponseDTO quiz);

    @GET("Quiz/GetPublicQuiz")
    Call<List<QuizResponseDTO>> getPublicQuiz();
}

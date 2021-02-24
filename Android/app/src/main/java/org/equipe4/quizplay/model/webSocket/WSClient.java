package org.equipe4.quizplay.model.webSocket;

import android.os.AsyncTask;
import android.util.Log;

import com.google.gson.Gson;
import com.neovisionaries.ws.client.WebSocket;
import com.neovisionaries.ws.client.WebSocketAdapter;
import com.neovisionaries.ws.client.WebSocketException;
import com.neovisionaries.ws.client.WebSocketExtension;
import com.neovisionaries.ws.client.WebSocketFactory;

import org.equipe4.quizplay.model.util.Global;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.BaseCommand;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation.AnswerQuizCommand;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation.JoinRoomCommand;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation.LeaveRoomCommand;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.commandImplementation.LogMessageCommand;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.resultCommand.JoinRoomResponse;
import org.equipe4.quizplay.model.webSocket.webSocketCommand.resultCommand.NextQuizResponse;

import java.io.IOException;
import java.io.Serializable;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.util.concurrent.ExecutionException;

import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSocketFactory;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;


public class WSClient implements Serializable {


    private static final String SERVER = Global.SERVER_PROD;

    private static WebSocket socket;
    public static String roomShareCode;
    private static WebSocketEventListener events;
    private static Gson gson;
    public WSClient() throws KeyManagementException, NoSuchAlgorithmException, ExecutionException, InterruptedException {
        if (socket == null) {
            socket = getWSClient().get();
        }
        gson = new Gson();
        events = null;
    }

    public static AsyncTask<String, Integer, WebSocket> getWSClient() throws NoSuchAlgorithmException, KeyManagementException {

        final TrustManager[] trustAllCerts = new TrustManager[]{
                new X509TrustManager() {
                    @Override
                    public void checkClientTrusted(java.security.cert.X509Certificate[] chain, String authType) {}

                    @Override
                    public void checkServerTrusted(java.security.cert.X509Certificate[] chain, String authType) {}

                    @Override
                    public java.security.cert.X509Certificate[] getAcceptedIssuers() {
                        return new java.security.cert.X509Certificate[]{};
                    }
                }
        };
        // Install the all-trusting trust manager
        final SSLContext sslContext = SSLContext.getInstance("SSL");
        sslContext.init(null, trustAllCerts, new java.security.SecureRandom());
        // Create an ssl socket factory with our all-trusting manager
        final SSLSocketFactory sslSocketFactory = sslContext.getSocketFactory();


        return new AsyncTask<String, Integer, WebSocket>() {
            @Override
            protected WebSocket doInBackground(String... strings) {
                String serverURL = strings[0];
                try {
                    return new WebSocketFactory()
                            .setSSLSocketFactory(sslSocketFactory)
                            .createSocket(serverURL)
                            .addListener(new WebSocketAdapter() {
                                // A text message arrived from the server.
                                @Override
                                public void onTextMessage(WebSocket websocket, String message) {
                                    socketListener(message);
                                }
                            }).connect();
                } catch (IOException | WebSocketException e) {
                    e.printStackTrace();
                }
                return null;
            }
        }.execute(SERVER);
    }

    public static void socketListener(String message) {
        Gson gson = new Gson();
        BaseCommand command = gson.fromJson(message, BaseCommand.class);

        Log.i("WebSocket-RECEIVE", message);


        // Handle message received
        handleMessage(command.commandName, message);
    }

    public static void disconnect() {
        if (socket != null) {
            Log.i("WebSocket", "Disconnected");
            socket.disconnect();
            socket = null;
        }
    }

    //region Room command

    private static void handleMessage(String commandName, String message) {
        switch (commandName) {
            case "Room.Join":
                JoinRoomResponse response = gson.fromJson(message, JoinRoomResponse.class);
                events.onRoomJoin(response.quiz);
                break;
            case "Log.Message":
                LogMessageCommand log = gson.fromJson(message, LogMessageCommand.class);
                // Log message
                logMessageActions(log.messageType);
                break;
            case "Quiz.Next":
                NextQuizResponse quizResponse = gson.fromJson(message, NextQuizResponse.class);
                events.onNextQuestion(quizResponse);
                break;
            case "Quiz.QuestionResult":
                events.onQuestionResult();
                break;
            default:
                break;

        }
    }

    private static void logMessageActions(String code) {
        switch (code){
            case "203":
                socket.disconnect();
                events.onRoomDeleted();
                break;
        }
    }

    //endregion

    //region Commandes WS

    public void joinRoom(JoinRoomCommand joinRoomCommandAction) {
        Log.i("WebSocket-SEND", gson.toJson(joinRoomCommandAction));
        socket.sendText(gson.toJson(joinRoomCommandAction));
    }

    public void leaveRoom(LeaveRoomCommand leaveRoomCommand) {
        Log.i("WebSocket-SEND", gson.toJson(leaveRoomCommand));
        socket.sendText(gson.toJson(leaveRoomCommand));
        disconnect();
    }

    public void sendAnswer(AnswerQuizCommand answerQuizCommand) {
        Log.i("WebSocket-SEND", gson.toJson(answerQuizCommand));
        socket.sendText(gson.toJson(answerQuizCommand));
    }

    //endregion

    public void setEventListener(WebSocketEventListener listener) {
        events = listener;
    }

}

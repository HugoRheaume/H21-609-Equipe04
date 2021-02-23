package org.equipe4.quizplay.webSocket;

import android.os.AsyncTask;
import android.util.Log;

import com.google.gson.Gson;
import com.neovisionaries.ws.client.WebSocket;
import com.neovisionaries.ws.client.WebSocketAdapter;
import com.neovisionaries.ws.client.WebSocketException;
import com.neovisionaries.ws.client.WebSocketExtension;
import com.neovisionaries.ws.client.WebSocketFactory;

import org.equipe4.quizplay.webSocket.webSocketCommand.BaseCommand;
import org.equipe4.quizplay.webSocket.webSocketCommand.commandImplementation.JoinRoomCommand;
import org.equipe4.quizplay.webSocket.webSocketCommand.commandImplementation.LeaveRoomCommand;
import org.equipe4.quizplay.webSocket.webSocketCommand.commandImplementation.LogMessageCommand;
import org.equipe4.quizplay.webSocket.webSocketCommand.commandImplementation.UserStateCommand;
import org.equipe4.quizplay.webSocket.webSocketCommand.resultCommand.JoinRoomResponse;

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

    private static final String SERVER_PROD = "wss://api.e4.projet.college-em.info/websocket/connect";
    private static final String SERVER_MARCO = "wss://192.168.0.136:45455/websocket/connect";
    private static final String SERVER_XAV = "wss://192.168.0.172:45455/websocket/connect";
    private static final String SERVER_HUGO = "wss://192.168.0.131:45455/websocket/connect";


    private static final String SERVER = SERVER_MARCO;

    private static WebSocket socket;

    private static WebSocketEventListener events;

    public WSClient() throws KeyManagementException, NoSuchAlgorithmException, ExecutionException, InterruptedException {
        socket = getWSClient().get();
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
                            })
                            .addExtension(WebSocketExtension.PERMESSAGE_DEFLATE)
                            .connect();
                } catch (WebSocketException | IOException e) {
                    e.printStackTrace();
                }
                return null;
            }
        }.execute(SERVER);
    }


    public static void socketListener(String message) {
        Gson gson = new Gson();
        BaseCommand command = gson.fromJson(message, BaseCommand.class);


        switch (command.commandName) {
            case "Room.UserState":
                UserStateCommand stateCommand = gson.fromJson(message, UserStateCommand.class);
                events.OnUserState(stateCommand.users);
                break;
            case "Room.Join":
                JoinRoomResponse response = gson.fromJson(message, JoinRoomResponse.class);
                events.OnRoomJoin(response.quiz);
                break;
            case "Log.Message":
                LogMessageCommand log = gson.fromJson(message, LogMessageCommand.class);
                switch (log.messageType){
                    case "203":
                        socket.disconnect();
                        events.OnRoomDeleted();
                        break;
                }
                break;
            default:
                break;

        }
        Log.i("WebSocket-RECEIVE", message);
    }



    //region Commandes WS

    public void joinRoom(JoinRoomCommand joinRoomCommandAction) {
        Gson gson = new Gson();
        Log.i("WebSocket-SEND", gson.toJson(joinRoomCommandAction));
        socket.sendText(gson.toJson(joinRoomCommandAction));
    }

    public void leaveRoom(LeaveRoomCommand leaveRoomCommand) {
        Gson gson = new Gson();
        Log.i("WebSocket-SEND", gson.toJson(leaveRoomCommand));
        socket.sendText(gson.toJson(leaveRoomCommand));
        socket.disconnect();
    }

    //endregion

    public void setEventListener(WebSocketEventListener listener) {
        events = listener;
    }

}

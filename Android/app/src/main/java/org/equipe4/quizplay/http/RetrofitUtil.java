package org.equipe4.quizplay.http;

import java.util.ArrayList;
import java.util.List;

import javax.net.ssl.HostnameVerifier;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.SSLSocketFactory;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

import okhttp3.Cookie;
import okhttp3.CookieJar;
import okhttp3.HttpUrl;
import okhttp3.OkHttpClient;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import retrofit2.converter.scalars.ScalarsConverterFactory;

public class RetrofitUtil {

    private static final String IP_PROD = "https://api.e4.projet.college-em.info/api/";
    private static final String IP_MARCO = "https://192.168.0.136:45455/api/";
    private static final String IP_XAV = "https://192.168.0.172:45455/api/";

    public static QPService get() {
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(IP_XAV)
                .client(getClient())
                .addConverterFactory(ScalarsConverterFactory.create())
                .addConverterFactory(GsonConverterFactory.create())
                .build();
        return retrofit.create(QPService.class);
    }

    public static class MyCookieJar implements CookieJar {

        private static List<Cookie> cookies;

        @Override
        public void saveFromResponse(HttpUrl url, List<Cookie> cookies) {
            MyCookieJar.cookies =  cookies;
        }

        @Override
        public List<Cookie> loadForRequest(HttpUrl url) {
            List<Cookie> res = new ArrayList<>();
            if (cookies != null){
                for(Cookie c : cookies){
                    if (c.expiresAt() > System.currentTimeMillis()) res.add(c);
                }
            }
            return res;
        }
    }


    public static OkHttpClient getClient() {
        try {
            OkHttpClient.Builder builder = new OkHttpClient.Builder();

            CookieJar cookieJar = new MyCookieJar();
            builder = builder.cookieJar(cookieJar);

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
            // configure the builder to accept all SSL certificates
            builder = builder.sslSocketFactory(sslSocketFactory);
            // configure the builder to accept all hostnames includint localhost
            builder = builder.hostnameVerifier(new HostnameVerifier() {
                @Override
                public boolean verify(String hostname, SSLSession session) {
                    return true;
                }
            });
            // Adds logging capability to see http exchanges on Android Monitor
            HttpLoggingInterceptor interceptor = new HttpLoggingInterceptor();
            interceptor.setLevel(HttpLoggingInterceptor.Level.BODY);
            builder = builder.addInterceptor(interceptor);
            return builder.build();
        }catch(Exception e){
            e.printStackTrace();
            return null;
        }
    }
}

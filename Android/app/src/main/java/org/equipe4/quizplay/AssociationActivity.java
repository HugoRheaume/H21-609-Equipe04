package org.equipe4.quizplay;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import android.content.ClipData;
import android.content.ClipDescription;
import android.content.Intent;
import android.content.res.Resources;
import android.graphics.PorterDuff;
import android.os.Bundle;
import android.os.CountDownTimer;
import android.util.AttributeSet;
import android.util.Log;
import android.view.DragEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.RadioButton;
import android.widget.TextView;
import android.widget.Toast;

import org.equipe4.quizplay.http.QPService;
import org.equipe4.quizplay.http.RetrofitUtil;
import org.equipe4.quizplay.transfer.QuestionAssociation;
import org.equipe4.quizplay.transfer.QuestionDTO;
import org.equipe4.quizplay.transfer.QuestionResultDTO;
import org.equipe4.quizplay.transfer.QuizResponseDTO;

import java.util.ArrayList;
import java.util.Dictionary;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class AssociationActivity extends AppCompatActivity {

    QuestionDTO question;
    QuizResponseDTO quiz;
    CountDownTimer timer;
    QPService service = RetrofitUtil.get();
    QuestionDTO nextQuestion;
    List<TextView> tvList;
    List<LinearLayout> dragListeners;
    List<LinearLayout> categories;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_association);

        question = (QuestionDTO)getIntent().getSerializableExtra("question");
        quiz = (QuizResponseDTO)getIntent().getSerializableExtra("quiz");

        TextView questionLabel = findViewById(R.id.questionLabel);
        questionLabel.setText(question.label);

        TextView quizTitle = findViewById(R.id.quizTitle);
        quizTitle.setText(quiz.title);

        tvList = new ArrayList<>();
        dragListeners = new ArrayList<>();
        dragListeners.add((LinearLayout)findViewById(R.id.zoneAsso1));
        categories = new ArrayList<>();
        categories.add((LinearLayout)findViewById(R.id.zoneCategory1));
        categories.add((LinearLayout)findViewById(R.id.zoneCategory2));

        TextView asso1 = findViewById(R.id.asso1);
        asso1.setText("1. " + question.questionAssociation.get(0).statement);
        asso1.setTag("out");
        tvList.add(asso1);
        if (question.questionAssociation.size() >= 2) {
            TextView asso2 = findViewById(R.id.asso2);
            asso2.setText("2. " + question.questionAssociation.get(1).statement);
            asso2.setTag("out");
            asso2.setVisibility(View.VISIBLE);
            tvList.add(asso2);
        }
        if (question.questionAssociation.size() >= 3) {
            TextView asso3 = findViewById(R.id.asso3);
            asso3.setText("3. " + question.questionAssociation.get(2).statement);
            asso3.setTag("out");
            asso3.setVisibility(View.VISIBLE);
            tvList.add(asso3);
        }
        if (question.questionAssociation.size() >= 4) {
            TextView asso4 = findViewById(R.id.asso4);
            asso4.setText("4. " + question.questionAssociation.get(3).statement);
            asso4.setTag("out");
            dragListeners.add((LinearLayout)findViewById(R.id.zoneAsso2));
            findViewById(R.id.zoneAsso2).setVisibility(View.VISIBLE);
            tvList.add(asso4);
        }
        if (question.questionAssociation.size() >= 5) {
            TextView asso5 = findViewById(R.id.asso5);
            asso5.setText("5. " + question.questionAssociation.get(4).statement);
            asso5.setTag("out");
            asso5.setVisibility(View.VISIBLE);
            tvList.add(asso5);
        }

        TextView category1 = findViewById(R.id.category1);
        category1.setText(question.categories.get(0));
        TextView category2 = findViewById(R.id.category2);
        category2.setText(question.categories.get(1));
        if (question.categories.size() == 3) {
            TextView category3 = findViewById(R.id.category3);
            category3.setText(question.categories.get(2));
            findViewById(R.id.zoneCategory3).setVisibility(View.VISIBLE);
            categories.add((LinearLayout)findViewById(R.id.zoneCategory3));
        }

        ProgressBar progressQuiz = findViewById(R.id.progressQuiz);
        progressQuiz.setMax(quiz.numberOfQuestions);
        progressQuiz.setProgress(question.quizIndex + 1);

        if (question.timeLimit > 0) {
            findViewById(R.id.timeLimit).setVisibility(View.VISIBLE);
            startTimer();
        }

        for(TextView tv : tvList) {
            tv.setOnLongClickListener(v -> {
                ClipData.Item item = new ClipData.Item((String)v.getTag());
                ClipData dragData = new ClipData((String)v.getTag(), new String[] { ClipDescription.MIMETYPE_TEXT_PLAIN }, item);
                View.DragShadowBuilder shadowBuilder = new View.DragShadowBuilder(v);
                v.setVisibility(View.INVISIBLE);
                return v.startDrag(dragData, shadowBuilder, v, 0);
            });
        }

        for(LinearLayout ll : dragListeners) {
            DragEventListener dragListener = new DragEventListener();
            ll.setOnDragListener(dragListener);
        }
        for(LinearLayout ll : categories) {
            DragEventListener dragListener = new DragEventListener();
            ll.setOnDragListener(dragListener);
        }

    }

    private void startTimer() {
        Resources res = getResources();

        ProgressBar progressTime = findViewById(R.id.progressTime);
        progressTime.setMax(question.timeLimit);
        progressTime.setProgress(question.timeLimit);

        TextView tvTime = findViewById(R.id.tvTime);
        tvTime.setText(question.timeLimit + "");

        timer = new CountDownTimer(question.timeLimit * 1000, 1000) {
            @Override
            public void onTick(long millisUntilFinished) {
                tvTime.setText(res.getQuantityString(R.plurals.secondsRemaining, (int)millisUntilFinished / 1000, (int)millisUntilFinished / 1000));
                progressTime.setProgress((int)millisUntilFinished / 1000);
            }

            @Override
            public void onFinish() {
                tvTime.setText(R.string.timesup);
                answer();
            }
        };
        timer.start();
    }

    public void answer() {
        if (timer != null)
            timer.cancel();
        boolean rightAnswer = true;
        boolean[][] goodAnswers = new boolean[question.categories.size()][question.questionAssociation.size()];

        for (int i = 0; i <tvList.size(); i++) {
            TextView tv = tvList.get(i);
            QuestionAssociation asso = question.questionAssociation.get(i);

            if(!tv.getTag().equals(asso.categoryIndex + "")) {
                rightAnswer = false;
                tv.getBackground().setColorFilter(ContextCompat.getColor(getApplicationContext(), R.color.wrong), PorterDuff.Mode.SRC_IN);
                goodAnswers[asso.categoryIndex][i] = true;
            }
            else
                tv.getBackground().setColorFilter(ContextCompat.getColor(getApplicationContext(), R.color.good), PorterDuff.Mode.SRC_IN);

            tv.setClickable(false);
        }

        String text1 = "";
        String text2 = "";
        String text3 = "";
        for(int i = 0; i < goodAnswers.length; i++) {
            boolean[] a = goodAnswers[i];
            for(int j = 0; j < a.length; j++) {
                boolean b = a[j];
                if (b) {
                    if (i == 0)
                        text1 += ((j + 1) + ", ");
                    if (i == 1)
                        text2 += ((j + 1) + ", ");
                    if (i == 2)
                        text3 += ((j + 1) + ", ");
                }
            }
        }


        if (!text1.equals("")) {
            text1 = text1.substring(0, text1.length() - 2);
            TextView tv = findViewById(R.id.goodAnswerCategory1);
            tv.setText(text1);
            tv.setVisibility(View.VISIBLE);
        }
        if (!text2.equals("")) {
            text2 = text2.substring(0, text2.length() - 2);
            TextView tv = findViewById(R.id.goodAnswerCategory2);
            tv.setText(text2);
            tv.setVisibility(View.VISIBLE);
        }
        if (!text3.equals("")) {
            text3 = text3.substring(0, text3.length() - 2);
            TextView tv = findViewById(R.id.goodAnswerCategory3);
            tv.setText(text3);
            tv.setVisibility(View.VISIBLE);
        }

        Button button = findViewById(R.id.nextQuestion);
        button.setText(R.string.nextQuestion);

        TextView result = findViewById(R.id.result);
        result.setVisibility(View.VISIBLE);
        if (rightAnswer) {
            result.setText(R.string.rightAnswer);
            result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.good));
        } else {
            result.setText(R.string.wrongAnswer);
            result.setTextColor(ContextCompat.getColor(getApplicationContext(), R.color.wrong));
        }

        service.getNextQuestion(new QuestionResultDTO(question.id, quiz.id, rightAnswer ? 1 : 0)).enqueue(new Callback<QuestionDTO>() {
            @Override
            public void onResponse(Call<QuestionDTO> call, Response<QuestionDTO> response) {
                if(response.isSuccessful()) {
                    nextQuestion = response.body();
                    if (nextQuestion.quizIndex == -1)
                        button.setText(R.string.finishQuiz);
                    Log.i("Response", response.body().toString());
                } else {
                    Log.i("RETROFIT", response.message());
                }
            }

            @Override
            public void onFailure(Call<QuestionDTO> call, Throwable t) {
                Log.e("RETROFIT", t.getMessage());
            }
        });
    }

    public void nextQuestion(View v) {
        if (nextQuestion == null) {
            answer();
            return;
        }

        Intent i;
        if (nextQuestion.quizIndex == -1)
            i = new Intent(getApplicationContext(), ResultActivity.class);
        else {
            switch (nextQuestion.questionType) {
                case 1:
                    i = new Intent(getApplicationContext(), TrueFalseActivity.class);
                    break;
                case 2:
                    i = new Intent(getApplicationContext(), MultipleChoiceActivity.class);
                    break;
                case 3:
                    i = new Intent(getApplicationContext(), AssociationActivity.class);
                    break;
                default:
                    return;
            }
            i.putExtra("question", nextQuestion);
        }

        i.putExtra("quiz", quiz);
        startActivity(i);
        finish();
    }

    protected class DragEventListener implements View.OnDragListener {

        @Override
        public boolean onDrag(View v, DragEvent event) {
            final int action = event.getAction();

            switch (action) {
                case DragEvent.ACTION_DRAG_STARTED:
                    if (event.getClipDescription().hasMimeType(ClipDescription.MIMETYPE_TEXT_PLAIN)) {
                        v.invalidate();
                        return true;
                    }
                    return false;

                case DragEvent.ACTION_DRAG_ENTERED:
                    v.invalidate();
                    return true;

                case DragEvent.ACTION_DRAG_LOCATION:
                    return true;

                case DragEvent.ACTION_DRAG_EXITED:
                    v.invalidate();
                    return true;

                case DragEvent.ACTION_DROP:
                    if (dragListeners.contains((LinearLayout)v)) return false;

                    ClipData.Item item = event.getClipData().getItemAt(0);
                    String dragData = item.getText().toString();

                    v.invalidate();

                    View view = (View)event.getLocalState();
                    ViewGroup viewGroup = (ViewGroup)view.getParent();

                    viewGroup.removeView(view);

                    if (dragListeners.contains((LinearLayout)viewGroup)) {
                        if (viewGroup.getChildCount() == 1 || ((question.questionAssociation.size() < 3) && (viewGroup.getChildCount() == 1 + (3 - question.questionAssociation.size())))) {
                            viewGroup.setVisibility(View.GONE);
                        } else {
                            View placeholder = viewGroup.getChildAt(viewGroup.getChildCount() - 1);
                            ((LinearLayout.LayoutParams) placeholder.getLayoutParams()).weight += 1;
                        }
                    }

                    LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT, 0);
                    params.setMargins(0, 5, 0, 5);
                    view.setLayoutParams(params);


                    LinearLayout destination = (LinearLayout)v;
                    if (v.getId() == findViewById(R.id.zoneCategory1).getId())
                        view.setTag("0");
                    if (v.getId() == findViewById(R.id.zoneCategory2).getId())
                        view.setTag("1");
                    if (v.getId() == findViewById(R.id.zoneCategory3).getId())
                        view.setTag("2");
                    destination.addView(view);

                    return true;

                case DragEvent.ACTION_DRAG_ENDED:
                    v.invalidate();
                    ((View)event.getLocalState()).setVisibility(View.VISIBLE);

                    if (event.getResult())
                        Log.i("DragDrop", "It do be working");
                    else
                        Log.i("DragDrop", "It ain't working");

                    return true;
                default:
                    Log.e("DragDrop","Unknown action type received by OnDragListener.");
                    break;
            }
            return false;
        }
    }
}
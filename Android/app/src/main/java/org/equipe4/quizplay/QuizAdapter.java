package org.equipe4.quizplay;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.LinearLayout;
import android.widget.TextView;

import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.RecyclerView;

import org.equipe4.quizplay.model.transfer.QuestionMultipleChoice;
import org.equipe4.quizplay.model.transfer.QuizResponseDTO;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.List;
import java.util.Locale;

public class QuizAdapter extends RecyclerView.Adapter<QuizAdapter.QuizViewHolder> {
    public List<QuizResponseDTO> list;
    public Context context;

    // Provide a reference to the views for each data item
    // Complex data items may need more than one view per item, and
    // you provide access to all the views for a data item in a view holder
    public static class QuizViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout layoutItem;
        public LinearLayout quizItem;
        public TextView tvQuizTitle;
        public TextView tvQuizNb;
        public TextView tvQuizDate;
        public TextView tvQuizAuthor;
        public QuizViewHolder(LinearLayout l) {
            super(l);
            layoutItem = l;
            quizItem = l.findViewById(R.id.quizItem);
            tvQuizTitle = l.findViewById(R.id.tvQuizTitle);
            tvQuizNb = l.findViewById(R.id.tvQuizNb);
            tvQuizDate = l.findViewById(R.id.tvQuizDate);
            tvQuizAuthor = l.findViewById(R.id.tvQuizAuthor);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public QuizAdapter(List<QuizResponseDTO> listQuiz, Context c) {
        list = listQuiz;
        context = c;
    }

    // Create new views (invoked by the layout manager)
    @Override
    public QuizAdapter.QuizViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        LinearLayout v = (LinearLayout) LayoutInflater.from(parent.getContext())
                .inflate(R.layout.quiz_item, parent, false);
        QuizViewHolder vh = new QuizViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(QuizViewHolder holder, int position) {
        // - get element from your dataset at this position
        // - replace the contents of the view with that element
        if (position % 2 == 0)
            holder.layoutItem.setBackgroundColor(ContextCompat.getColor(context, R.color.lightList));
        holder.quizItem.setTag(list.get(position).id);
        holder.tvQuizTitle.setText(list.get(position).title);
        holder.tvQuizNb.setText(list.get(position).numberOfQuestions + "");

        DateFormat df = new SimpleDateFormat("yyyy-MM-dd", Locale.CANADA_FRENCH);
        holder.tvQuizDate.setText(df.format(list.get(position).date));
        holder.tvQuizAuthor.setText(list.get(position).author);
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return list.size();
    }
}


package org.equipe4.quizplay;

import android.view.LayoutInflater;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.LinearLayout;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import org.equipe4.quizplay.transfer.QuestionMultipleChoice;

import java.util.List;

public class ChoiceAdapter extends RecyclerView.Adapter<ChoiceAdapter.ChoiceViewHolder> {
    public List<QuestionMultipleChoice> list;

    // Provide a reference to the views for each data item
    // Complex data items may need more than one view per item, and
    // you provide access to all the views for a data item in a view holder
    public static class ChoiceViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView tvChoice;
        public CheckBox checkboxChoice;
        public ChoiceViewHolder(LinearLayout l) {
            super(l);
            tvChoice = l.findViewById(R.id.tvChoice);
            checkboxChoice = l.findViewById(R.id.checkboxChoice);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public ChoiceAdapter(List<QuestionMultipleChoice> listChoice) {
        list = listChoice;
    }

    // Create new views (invoked by the layout manager)
    @Override
    public ChoiceAdapter.ChoiceViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        LinearLayout v = (LinearLayout) LayoutInflater.from(parent.getContext())
                .inflate(R.layout.choice_item, parent, false);
        ChoiceViewHolder vh = new ChoiceViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ChoiceViewHolder holder, int position) {
        // - get element from your dataset at this position
        // - replace the contents of the view with that element
        holder.tvChoice.setText(list.get(position).statement);
        holder.checkboxChoice.setTag(list.get(position).id);
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return list.size();
    }
}
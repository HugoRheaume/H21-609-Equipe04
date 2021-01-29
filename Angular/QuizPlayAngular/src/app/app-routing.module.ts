import { CreateTrueOrFalseQuestion } from './../components/create-trueorfalse-question/create-trueorfalse-question.component';
import { CreateQuizComponent } from 'src/components/create-quiz/create-quiz.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuizQuestionListComponent } from './components/quiz-question-list/quiz-question-list.component';

const routes: Routes = [
  { path: '', component: CreateQuizComponent },
  { path: 'CreatQuestionTrueOrFalse', component: CreateTrueOrFalseQuestion },
  { path: 'quiz/:quizId', component: QuizQuestionListComponent },
  { path: 'quiz', component: CreateQuizComponent }
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}
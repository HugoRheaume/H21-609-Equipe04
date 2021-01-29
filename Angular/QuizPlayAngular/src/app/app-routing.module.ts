import { AlphanumericCodeComponent } from './../components/alphanumeric-code/alphanumeric-code.component';
import { CreateTrueOrFalseQuestion } from './../components/create-trueorfalse-question/create-trueorfalse-question.component';
import { CreateQuizComponent } from 'src/components/create-quiz/create-quiz.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', component: CreateQuizComponent },
  { path: 'CreatQuestionTrueOrFalse', component: CreateTrueOrFalseQuestion},
  {path: 'CreateQuiz/:code', component: AlphanumericCodeComponent}];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}

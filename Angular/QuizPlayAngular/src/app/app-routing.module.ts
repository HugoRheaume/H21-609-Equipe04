import { AlphanumericCodeComponent } from 'src/components/alphanumeric-code/alphanumeric-code.component';
import { CreateTrueOrFalseQuestion } from './../components/create-trueorfalse-question/create-trueorfalse-question.component';
import { CreateQuizComponent } from 'src/components/create-quiz/create-quiz.component';
import { ListQuizComponent } from '../components/list-quiz/list-quiz.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
	{ path: 'list', component: ListQuizComponent },
	{ path: 'createQuestion', component: CreateTrueOrFalseQuestion },
	{ path: 'createQuiz', component: CreateQuizComponent },
	{ path: 'CreateQuiz/:code', component: AlphanumericCodeComponent },
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}

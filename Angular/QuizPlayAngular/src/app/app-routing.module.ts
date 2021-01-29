import { AlphanumericCodeComponent } from './../components/alphanumeric-code/alphanumeric-code.component';
import { CreateQuestionComponent } from './../components/create-question/create-question.component';
import { CreateQuizComponent } from 'src/components/create-quiz/create-quiz.component';
import { ListQuizComponent } from '../components/list-quiz/list-quiz.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
	{ path: 'list', component: ListQuizComponent },
	{ path: 'createQuestion', component: CreateQuestionComponent },
	{ path: 'createQuiz', component: CreateQuizComponent },
	{path: 'CreateQuiz/:code', component: AlphanumericCodeComponent}
    ];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}

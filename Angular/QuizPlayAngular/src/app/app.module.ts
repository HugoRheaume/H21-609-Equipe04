import { RouterModule } from '@angular/router';
import { MaterialModule } from 'src/app/material/material.module';
import { CreateTrueOrFalseQuestion } from '../components/create-trueorfalse-question/create-trueorfalse-question.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
	CreateQuizComponent,
	CreateQuizConfirmDialog,
} from 'src/components/create-quiz/create-quiz.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AlphanumericCodeComponent } from 'src/components/alphanumeric-code/alphanumeric-code.component';
import { QuizQuestionListComponent } from './components/quiz-question-list/quiz-question-list.component';
@NgModule({
	declarations: [
		AppComponent,
		CreateQuizComponent,
		CreateQuizConfirmDialog,
		CreateTrueOrFalseQuestion,
		QuizQuestionListComponent,
    AlphanumericCodeComponent
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		FormsModule,
		HttpClientModule,
		MaterialModule,
		BrowserAnimationsModule,
		FormsModule,
		ReactiveFormsModule,

		RouterModule.forRoot([
			{path: 'create/:quizId', component: QuizQuestionListComponent},
			{path: 'create', component: CreateQuizComponent}
			
	  
		  ]),
	],
	entryComponents: [CreateQuizComponent],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule {}

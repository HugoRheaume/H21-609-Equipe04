import { CreateQuestionComponent } from './../components/create-question/create-question.component';
import { MaterialModule } from 'src/components/material/material.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
	CreateQuizComponent,
	CreateQuizConfirmDialog,
} from './create-quiz/create-quiz.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
	declarations: [
		AppComponent,
		CreateQuizComponent,
		CreateQuizConfirmDialog,
		CreateQuestionComponent,
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
	],
	entryComponents: [CreateQuizComponent],
	providers: [],
	bootstrap: [AppComponent],
})
export class AppModule {}

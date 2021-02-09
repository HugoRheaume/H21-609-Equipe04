import { CommonModule, registerLocaleData } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import localeFr from '@angular/common/locales/fr';
import { NgModule } from '@angular/core';
import { AngularFireModule } from '@angular/fire';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { AngularFireStorageModule } from '@angular/fire/storage';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from 'src/app/material/material.module';
import {
	CreateQuizComponent,
	CreateQuizConfirmDialog,
} from '../app/components/create-quiz/create-quiz.component';
import { LoginComponent } from '../app/components/login/login.component';
import { CreateTrueOrFalseQuestion } from '../app/components/create-trueorfalse-question/create-trueorfalse-question.component';
import { CreateQuestionComponent } from '../app/components/create-question/create-question.component';
import {
	DeleteQuizDialog,
	ListQuizComponent,
} from '../app/components/list-quiz/list-quiz.component';
import { WaitingRoomComponent } from '../app/components/waiting-room/waiting-room.component';
import { environment } from './../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CreateMultiplechoicesQuestionComponent } from './components/create-multiplechoices-question/create-multiplechoices-question.component';
import { QuizQuestionListComponent } from './components/quiz-question-list/quiz-question-list.component';

registerLocaleData(localeFr, 'fr');

@NgModule({
	declarations: [
		AppComponent,
		CreateQuizComponent,
		CreateQuizConfirmDialog,
		CreateTrueOrFalseQuestion,
		CreateQuestionComponent,
		CreateMultiplechoicesQuestionComponent,
		ListQuizComponent,
		DeleteQuizDialog,
		QuizQuestionListComponent,
		LoginComponent,
		WaitingRoomComponent,
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
		CommonModule,
		AngularFireModule.initializeApp(environment.firebase),
		AngularFirestoreModule,
		AngularFireStorageModule,
		AngularFireAuthModule,
	],
	entryComponents: [CreateQuizComponent],
	providers: [],
	bootstrap: [AppComponent],
})
export class AppModule {}

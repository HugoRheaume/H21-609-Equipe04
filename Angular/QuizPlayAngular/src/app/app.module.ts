import { environment } from './../environments/environment';
import { WaitingRoomComponent } from './../components/waiting-room/waiting-room.component';
import { RouterModule } from '@angular/router';
import { CreateMultiplechoicesQuestionComponent } from './../components/create-multiplechoices-question/create-multiplechoices-question.component';
import { CreateQuestionComponent } from './../components/create-question/create-question.component';
import { MaterialModule } from 'src/app/material/material.module';
import {
	ListQuizComponent,
	DeleteQuizDialog,
} from './../components/list-quiz/list-quiz.component';
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
import {
	CommonModule,
	HashLocationStrategy,
	LocationStrategy,
} from '@angular/common';
import { registerLocaleData } from '@angular/common';
import localeFr from '@angular/common/locales/fr';
registerLocaleData(localeFr, 'fr');
import { QuizQuestionListComponent } from './components/quiz-question-list/quiz-question-list.component';
import { AngularFireModule } from '@angular/fire';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { AngularFireStorageModule } from '@angular/fire/storage';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { LoginComponent } from 'src/components/login/login.component';

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

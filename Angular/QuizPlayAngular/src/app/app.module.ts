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
import { CommonModule, HashLocationStrategy, LocationStrategy } from '@angular/common';
import { registerLocaleData } from '@angular/common';
import localeFr from '@angular/common/locales/fr';
registerLocaleData(localeFr, 'fr');
import { QuizQuestionListComponent } from './components/quiz-question-list/quiz-question-list.component';
import { JoinQuizComponent } from './components/websocket/join-quiz/join-quiz.component';

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
		JoinQuizComponent,
    WaitingRoomComponent
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
  ],
  entryComponents: [CreateQuizComponent],
  providers: [
    { provide: LocationStrategy, useClass: HashLocationStrategy }
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }

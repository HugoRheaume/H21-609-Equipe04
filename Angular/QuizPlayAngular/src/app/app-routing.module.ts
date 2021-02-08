import { CreateTrueOrFalseQuestion } from './../components/create-trueorfalse-question/create-trueorfalse-question.component';
import { CreateQuizComponent } from 'src/components/create-quiz/create-quiz.component';
import { ListQuizComponent } from '../components/list-quiz/list-quiz.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuizQuestionListComponent } from './components/quiz-question-list/quiz-question-list.component';
import { LoginComponent } from 'src/components/login/login.component';
import { WaitingRoomComponent } from 'src/components/waiting-room/waiting-room.component';
const routes: Routes = [
  { path: 'list', component: ListQuizComponent },
  { path: '', component: LoginComponent },
  { path: 'quiz/:quizId', component: QuizQuestionListComponent },
  { path: 'quiz', component: CreateQuizComponent },
  { path: 'live', component: WaitingRoomComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }

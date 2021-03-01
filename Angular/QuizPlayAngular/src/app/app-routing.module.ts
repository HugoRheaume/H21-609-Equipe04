import { CreateQuizComponent } from 'src/components/create-quiz/create-quiz.component';
import { ListQuizComponent } from '../components/list-quiz/list-quiz.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomePageComponent } from 'src/components/home-page/home-page.component';
import { QuizQuestionListComponent } from '../components/quiz-question-list/quiz-question-list.component';
import { WaitingRoomComponent } from 'src/components/QuizLive/waiting-room/waiting-room.component';
import { QuizRoomComponent } from 'src/components/QuizLive/quiz-room/quiz-room.component';
import { AntiAuthGuard, AuthGuard } from './models/AuthGuard';
const routes: Routes = [
  { path: 'list', component: ListQuizComponent, canActivate: [AuthGuard] },
  {
    path: 'quiz/:quizShareCode',
    component: QuizQuestionListComponent,
    canActivate: [AuthGuard],
  },
  { path: 'quiz', component: CreateQuizComponent, canActivate: [AuthGuard] },
  { path: 'live/:quizShareCode', component: WaitingRoomComponent },
  { path: 'live/:quizShareCode/:questionIndex', component: QuizRoomComponent },
  { path: '', component: HomePageComponent, canActivate: [AntiAuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}

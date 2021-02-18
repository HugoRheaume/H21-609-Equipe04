import { CreateQuizComponent } from 'src/components/create-quiz/create-quiz.component';
import { ListQuizComponent } from '../components/list-quiz/list-quiz.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomePageComponent } from 'src/components/home-page/home-page.component';
import { QuizQuestionListComponent } from '../components/quiz-question-list/quiz-question-list.component';
import { LoginComponent } from 'src/components/login/login.component';
import { WaitingRoomComponent } from 'src/components/waiting-room/waiting-room.component';
import { QuizRoomComponent } from 'src/components/quiz-room/quiz-room.component';
const routes: Routes = [
	{ path: 'list', component: ListQuizComponent },
	{ path: 'quiz/:quizShareCode', component: QuizQuestionListComponent },
	{ path: 'quiz', component: CreateQuizComponent },
	{ path: 'live/:quizShareCode', component: WaitingRoomComponent },
	{ path: 'live/:quizShareCode/:questionIndex', component: QuizRoomComponent },
	{ path: '', component: HomePageComponent },
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}

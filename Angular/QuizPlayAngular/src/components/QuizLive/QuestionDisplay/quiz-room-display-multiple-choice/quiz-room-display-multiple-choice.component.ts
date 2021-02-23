import { Component, OnInit } from '@angular/core';
import { QuestionType } from 'src/app/models/question';
import { QuizRoomComponent } from '../../quiz-room/quiz-room.component';

@Component({
  selector: 'app-quiz-room-display-multiple-choice',
  templateUrl: './quiz-room-display-multiple-choice.component.html',
  styleUrls: ['./quiz-room-display-multiple-choice.component.css'],
})
export class QuizRoomDisplayMultipleChoiceComponent implements OnInit {
  constructor(public quizRoomComponent: QuizRoomComponent) { }

  ngOnInit(): void {
  }
}

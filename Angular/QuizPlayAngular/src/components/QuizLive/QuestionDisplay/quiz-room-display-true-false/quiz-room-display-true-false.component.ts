import { QuizRoomComponent } from 'src/components/QuizLive/quiz-room/quiz-room.component';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-quiz-room-display-true-false',
  templateUrl: './quiz-room-display-true-false.component.html',
  styleUrls: ['./quiz-room-display-true-false.component.css'],
})
export class QuizRoomDisplayTrueFalseComponent implements OnInit {
  constructor(public quizRoomComponent: QuizRoomComponent) {}

  ngOnInit(): void {}
}

import { QuizRoomComponent } from 'src/components/quiz-room/quiz-room.component';
import { Component, OnInit } from '@angular/core';
import { Score, User } from 'src/app/web-socket.service';

@Component({
  selector: 'app-scoreboard',
  templateUrl: './scoreboard.component.html',
  styleUrls: ['./scoreboard.component.css']
})
export class ScoreboardComponent implements OnInit {

 
  constructor(public quizRoomComponent:  QuizRoomComponent) { }

  ngOnInit(): void { 
    
  }
  
}

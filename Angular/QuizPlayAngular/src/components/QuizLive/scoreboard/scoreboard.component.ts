import { QuizRoomComponent } from 'src/components/QuizLive/quiz-room/quiz-room.component';
import { Component, OnInit } from '@angular/core';
import { Score, User, WebSocketService } from 'src/app/services/web-socket.service';

@Component({
  selector: 'app-scoreboard',
  templateUrl: './scoreboard.component.html',
  styleUrls: ['./scoreboard.component.scss']
})
export class ScoreboardComponent implements OnInit {


  public top5: Score[] = [];
  public topColors = ['rgb(255,215,0)', 'rgb(192,192,192)', 'rgb(205,127,50)'];
  public usernamesFormated: String[] = [];
  constructor(public quizRoomComponent: QuizRoomComponent) { }

  ngOnInit(): void {


    for (let i = 0; i < 10; i++) {
      let score = new Score();
      let user: User = { name: "user dasdsa asd sda sad dsa dsa dsa dsa sad dsa dsa sa dasbd sad kjhasgd kjhgas " + i, picture: "https://via.placeholder.com/64", isAnswer: true };
      score.user = user;
      score.score = i + 1;
      this.quizRoomComponent.wsService.updateUserDisplay();
      this.quizRoomComponent.wsService.scoreboard.push(score);
    }

    this.quizRoomComponent.wsService.scoreboard.sort((a, b) => {
      if (a.score > b.score) return -1;
      if (a.score < b.score) return 1;
      else return 0;
    })

    let max = this.quizRoomComponent.wsService.scoreboard.length < 5 ? this.quizRoomComponent.wsService.scoreboard.length : 5;
    for (let i = 0; i < max; i++) {
      this.top5.push(this.quizRoomComponent.wsService.scoreboard[i]);
    }

    for (let i = 0; i < this.top5.length; i++) {
      let item = this.top5[i];
      if (item.user.name.length > 25) {
        let firstChars = item.user.name.substring(0, 22);
        this.usernamesFormated[i] = firstChars + '...';
        continue;
      }
      this.usernamesFormated[i] = item.user.name;
    }
  }

}

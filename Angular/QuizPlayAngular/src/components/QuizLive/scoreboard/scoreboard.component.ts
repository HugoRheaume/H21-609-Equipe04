import { QuizRoomComponent } from 'src/components/QuizLive/quiz-room/quiz-room.component';
import { Component, OnInit } from '@angular/core';
import { Score } from 'src/app/models/Score';
@Component({
  selector: 'app-scoreboard',
  templateUrl: './scoreboard.component.html',
  styleUrls: ['./scoreboard.component.scss'],
})
export class ScoreboardComponent implements OnInit {
  public top5: Score[] = [];
  public topColors = ['rgb(255,215,0)', 'rgb(192,192,192)', 'rgb(205,127,50)'];
  public usernamesFormated: String[] = [];
  constructor(public quizRoomComponent: QuizRoomComponent) {}

  ngOnInit(): void {
    this.quizRoomComponent.wsService.scoreboard.sort((a, b) => {
      if (a.score > b.score) return -1;
      if (a.score < b.score) return 1;
      else return 0;
    });

    let max =
      this.quizRoomComponent.wsService.scoreboard.length < 5
        ? this.quizRoomComponent.wsService.scoreboard.length
        : 5;
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

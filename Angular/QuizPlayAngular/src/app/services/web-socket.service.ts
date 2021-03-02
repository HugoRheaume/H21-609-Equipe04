import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { webSocket } from 'rxjs/webSocket';
import { Router } from '@angular/router';
import { Question } from 'src/app/models/question';
import {
  BeginQuizWS,
  CommandName,
  CreateRoomWS,
  DeleteRoomWS,
  NextQuestionWS,
  ResultQuestionWS,
} from '../models/Command';

import { Score } from '../models/Score';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root',
})
export class WebSocketService {
  public currentQuestion$: Subject<Question> = new Subject<Question>();
  public _forceSkip$ = new Subject();
  public scoreboard: Score[] = [];
  public users: User[] = [];
  public currentShareCode: string = '';
  public usersDisplay: Array<number>;
  public usersFormated: string[] = [];
  public defaultTimeLimit: number;
  public nbGoodAnswer: number = 0;

  public canDestroy: boolean = true;

  subject = webSocket<any>({
    url: environment.backend.webSocketURL + '/connect',
    deserializer: (msg) => msg,
  });

  constructor(public http: HttpClient, public router: Router) {}

  public connect() {
    this.subject.subscribe((msg) => this.messageReceiver(msg.data));
  }

  //----------------------------------------
  private messageReceiver(data: any) {
    var d = JSON.parse(data);
    switch (d.commandName) {
      case CommandName.CreateRoom:
        this.currentShareCode = JSON.parse(data).shareCode;
        break;
      case CommandName.RoomSate:
        this.users = JSON.parse(data).users;
        this.updateUserDisplay();
        break;
      case CommandName.QuizBegin:
        this.router.navigate(['live/' + d.quizShareCode + '/0']);
        break;
      case CommandName.QuizScoreboard:
        this.nbGoodAnswer = d.nbGoodAnswer;
        this.scoreboard = d.scores;
        break;
      case CommandName.QuizNext:
        const q: Question = d.question;
        if (q.timeLimit == -1) q.timeLimit = this.defaultTimeLimit;
        this.currentQuestion$.next(q);
        break;
      case CommandName.QuizForceSkip:
        this._forceSkip$.next('skip');
        break;
    }
  }
  //----------------------------------------
  public create(quizShareCode: string) {
    let ws = new CreateRoomWS();
    ws.owner = 'Angular Master';
    ws.token = localStorage.getItem('token');
    ws.quizShareCode = quizShareCode;
    this.subject.next(ws);
  }
  public cancel() {
    if (!this.canDestroy) return;
    let ws = new DeleteRoomWS();
    ws.token = localStorage.getItem('token');
    ws.shareCode = this.currentShareCode;
    this.subject.next(ws);
    this.currentShareCode = '';
    this.users = [];
    this.updateUserDisplay();
  }
  public beginQuiz(time: number) {
    this.canDestroy = false;
    this.defaultTimeLimit = time;
    let ws = new BeginQuizWS();
    ws.token = localStorage.getItem('token');
    ws.shareCode = this.currentShareCode;
    this.subject.next(ws);
  }
  public nextQuestion(questionIndex: number) {
    this.nbGoodAnswer = 0;
    let ws = new NextQuestionWS();
    ws.token = localStorage.getItem('token');
    ws.questionIndex = questionIndex;
    this.subject.next(ws);
  }
  public resultQuestion() {
    let ws = new ResultQuestionWS();
    ws.token = localStorage.getItem('token');
    this.subject.next(ws);
  }
  public get forceSkip$(): Observable<any> {
    return this._forceSkip$.asObservable();
  }

  updateUserDisplay() {
    for (let i = 0; i < this.users.length; i++) {
      let user = this.users[i];
      if (user.name.length > 14) {
        let firstChars = user.name.substring(0, 11);
        this.usersFormated[i] = firstChars + '...';
        continue;
      }
      this.usersFormated[i] = user.name;
    }
    this.usersDisplay = Array(this.users.length)
      .fill(0)
      .map((x, i) => i);
  }
}

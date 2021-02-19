import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { webSocket } from 'rxjs/webSocket';
import { Router } from '@angular/router';
import { Question } from 'src/app/models/question';

@Injectable({
  providedIn: 'root',
})
export class WebSocketService {
  public scoreboard: Score[] = [];
  public messages$ = new Subject<string[]>();
  public users: User[] = [];
  public currentShareCode: string = '';
  public usersDisplay: Array<number>;
  public usersFormated: string[] = [];
  public defaultTimeLimit: number;
  public currentQuestion$: Subject<Question> = new Subject<Question>();

  public canDestroy: boolean = true;

  //subject = webSocket("wss://localhost:44351/api/websocket/joinwaitingroom");
  subject = webSocket<any>({
    //url: "wss://localhost:44351/websocket/connect",
    url: environment.backend.webSocketURL + '/connect',
    deserializer: (msg) => msg,
  });

  constructor(public http: HttpClient, public router: Router) {}

  public connect() {
    this.subject.subscribe((msg) => this.messageReceiver(msg.data));
  }
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
      case CommandName.LogMessage:
        this.handleLogMessage(d);
        break;
      case CommandName.QuizBegin:
        this.router.navigate(['live/' + d.quizShareCode + '/0']);
        break;
      case CommandName.QuizScoreboard:
        this.scoreboard = d.scores;
        break;
      case CommandName.QuizNext:
        const q: Question = d.question;
        if (q.timeLimit == -1) q.timeLimit = this.defaultTimeLimit;
        this.currentQuestion$.next(q);
        break;
    }
  }
  private handleLogMessage(data: any) {
    switch (data.messageType) {
      case MessageType.LogRoomCreated:
        this.messages$.next(['Room Created!']);
        break;
      case MessageType.LogRoomJoined:
        this.messages$.next(['Room Created!']);
        break;
      case MessageType.LogRoomDeleted:
        this.messages$.next(['The room as been deleted']);
        break;
      case MessageType.ErrorShareCodeNotExist:
        this.messages$.next(['Share code does not exist']);
        break;
      case MessageType.ErrorInvalidRequest:
        this.messages$.next(['Invalid Request']);
        break;
      case MessageType.ErrorUserAlreadyJoined:
        this.messages$.next(['User has already joined']);
        break;
    }
  }
  public logReceive$(): Observable<any> {
    return this.messages$.asObservable();
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
export class CreateRoomWS {
  public owner: string;
  public shareCode: string;
  public token: string;
  public quizShareCode: string;
  private readonly commandName: string = CommandName.CreateRoom;
}
export class DeleteRoomWS {
  public token: string;
  public shareCode: string;
  private readonly commandName: string = CommandName.RoomDestroy;
}
export class BeginQuizWS {
  public token: string;
  public shareCode: string;
  private readonly commandName: string = CommandName.QuizBegin;
}
export class NextQuestionWS {
  public token: string;
  public questionIndex: number;
  public question: Question;
  private readonly commandName: string = CommandName.QuizNext;
}
export class ResultQuestionWS {
  public token: string;
  public questionIndex: number;
  private readonly commandName: string = CommandName.QuizQuestionResult;
}
export interface User {
  name: string;
  picture: string;
  isAnswer: boolean;
}
export class Score {
  user: User;
  score: number;
}
export enum CommandName {
  CreateRoom = 'Room.Create',
  JoinRoom = 'Room.Join',
  RoomSate = 'Room.UserState',
  RoomLeave = 'Room.Leave',
  RoomDestroy = 'Room.Destroy',
  LogMessage = 'Log.Message',
  QuizBegin = 'Quiz.Begin',
  QuizNext = 'Quiz.Next',
  QuizScoreboard = 'Quiz.Scoreboard',
  QuizQuestionResult = 'Quiz.QuestionResult',
}
export enum MessageType {
  LogRoomCreated,
  LogRoomJoined,
  LogRoomLeft,
  LogRoomDeleted,
  ErrorShareCodeNotExist,
  ErrorInvalidRequest,
  ErrorUserAlreadyJoined,
}

import { environment } from './../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { webSocket } from 'rxjs/webSocket';

@Injectable({
  providedIn: 'root'
})
export class WebSocketService {
  
  public messages$ = new Subject<string[]>();
  public users: User[]= [];
  public currentShareCode: string= '';
  public usersDisplay: Array<number>;
  public usersFormated: string[] = [];

  //subject = webSocket("wss://localhost:44351/api/websocket/joinwaitingroom");
  subject = webSocket<any>({
    //url: "wss://localhost:44351/websocket/connect",
    url: environment.backend.webSocketURL + '/connect',
    deserializer: msg => msg,
    
  
    
  });

	constructor(public http: HttpClient) {}

    public connect()
    {
      this.subject.subscribe(
      msg => this.messageReceiver(msg.data));
    }
    public create()
    {
      let ws = new CreateRoomWS();     
      ws.owner = "Angular Master";
      ws.token = localStorage.getItem('token');
      this.subject.next(ws);
    }
    public cancel()
    {
      let ws = new DeleteRoomWS();  
      ws.name = "Angular Master";   
      ws.shareCode = this.currentShareCode;
      this.subject.next(ws);
      this.currentShareCode = '';
      this.users = [];
      this.updateUserDisplay();
    }
    public beginQuiz()
    {
      console.log('Quiz Begin!');
        let ws = new BeginQuizWS();
        ws.shareCode = this.currentShareCode;
        this.subject.next(ws);
    }
    

    
    private messageReceiver(data: any)
    {
      var d = JSON.parse(data)
      switch (d.CommandName)
      {
        case CommandName.CreateRoom:
          this.currentShareCode = JSON.parse(data).ShareCode;
          break;
          case CommandName.RoomSate:
          this.users = JSON.parse(data).users;
          this.updateUserDisplay();          
          break;
        case CommandName.LogMessage:
          this.handleLogMessage(d);
          break;
      }
      
      
    }
    private handleLogMessage(data: any)
    {
      switch (data.MessageType)
      {
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
    public logReceive$() : Observable<any>{
      return this.messages$.asObservable();
    }
    

    updateUserDisplay()
    {
      for (let i = 0; i < this.users.length; i++) {
        let user = this.users[i];
        if (user.Name.length > 14) {
          let firstChars = user.Name.substring(0, 11);
          this.usersFormated[i] = firstChars + '...';
          continue;
        }
        this.usersFormated[i] = user.Name;
      }
      this.usersDisplay = Array(this.users.length).fill(0).map((x, i) => i);
      
    }
    


}
export class CreateRoomWS
{
    public owner: string;
    public shareCode: string;
    public token: string;
    private readonly CommandName: string = CommandName.CreateRoom;
}
export class DeleteRoomWS
{
    public name: string;
    public shareCode: string;
    private readonly CommandName: string = CommandName.RoomDestroy;
}
export class BeginQuizWS
{
  public shareCode: string;
  private readonly CommandName: string = CommandName.QuizBegin;
}

export interface User
{
  Name: string;
  Picture: string;
}
export enum CommandName { 
  CreateRoom = 'Room.Create', 
  JoinRoom= 'Room.Join', 
  RoomSate = 'Room.UserState', 
  RoomLeave = 'Room.Leave', 
  RoomDestroy = 'Room.Destroy', 
  LogMessage = 'Log.Message', 
  QuizBegin = 'Quiz.Begin',
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
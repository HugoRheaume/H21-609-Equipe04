import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { webSocket } from 'rxjs/webSocket';

@Injectable({
  providedIn: 'root'
})
export class WebSocketService {
  
  public messages: string[] = [];
  public users: User[]= [];
  public currentShareCode: string= '';
  public usersDisplay: Array<number>;
  public usersFormated: string[] = [];

  //subject = webSocket("wss://localhost:44351/api/websocket/joinwaitingroom");
  subject = webSocket<any>({
    url: "wss://localhost:44351/api/websocket/joinwaitingroom",
    deserializer: msg => msg,
    //serializer: msg => JSON.stringify({channel: "webDevelopment", msg: msg})
    
  
    
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
      ws.messageType = MessageType.CreateRoom;
      ws.owner = "Angular Master";

      this.subject.next(ws);
    }
    
    private messageReceiver(data: any)
    {
      
      switch (JSON.parse(data).MessageType)
      {
        case MessageType.CreateRoom:
          this.currentShareCode = JSON.parse(data).ShareCode;
          break;
        case MessageType.RoomSate:
          this.users = JSON.parse(data).users;
          this.updateUserDisplay();
          
          break;
        case MessageType.LogMessage:
          
          this.handleLogMessage(JSON.parse(data))
        this.messages.push(JSON.parse(data).Message);
          break;
        case MessageType.ErrorMessage:
          this.handleErrorMessage(JSON.parse(data));
          break;
      }
      
      
    }
    private handleLogMessage(data: any)
    {
      switch (data.LogMessageType)
      {
        case LogMessageType.RoomCreated:
          this.messages.push('Room Created!');
          break;
      }
    }
    private handleErrorMessage(data: any)
    {
      switch (data.ErrorType)
      {
        case ErrorType.ShareCodeNotExist:
          this.messages.push('Share code does not exist');
          break;
        case ErrorType.InvalidRequest:
          this.messages.push('Invalid Request');
          break;
        case ErrorType.UserAlreadyJoined:
          this.messages.push('User has already joined');
          break;
        case ErrorType.RoomDeleted:
          this.messages.push('The as been deleted');
          break;

      }
       
    }

    updateUserDisplay()
    {
      for (let i = 0; i < this.users.length; i++) {
        let user = this.users[i];
        if (user.Username.length > 14) {
          let firstChars = user.Username.substring(0, 11);
          this.usersFormated[i] = firstChars + '...';
          continue;
        }
        this.usersFormated[i] = user.Username;
      }
      this.usersDisplay = Array(this.users.length).fill(0).map((x, i) => i);
      
    }
    


}
export class CreateRoomWS
{
    public owner: string;
    public shareCode: string;
    public messageType: MessageType;
}
export class JoinWaitingRoomWS
{
    public ShareCode: string;
    public messageType: MessageType;
    public username: string;
}
export class User
{
  public Username: string = '';
  public Picture: string = '';
}
export enum MessageType{ QuizInfo, JoinRoom, CreateRoom, LogMessage, RoomSate, ErrorMessage}
export enum ErrorType { ShareCodeNotExist, InvalidRequest, UserAlreadyJoined, RoomDeleted}
export enum LogMessageType { RoomCreated}
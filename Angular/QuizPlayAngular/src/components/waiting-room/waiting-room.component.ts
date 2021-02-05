import { Message } from './../../models/message';
import { Component, OnInit } from '@angular/core';
import { WebSocketService } from 'src/app/web-socket.service';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.scss']
})
export class WaitingRoomComponent implements OnInit {

  //public users: string[];
  public usersFormated: string[] = [];
  public iterator: Array<number>;
  public shareCode: string;

  public messages: Message[] = [];
  public dataSource: Message[];
  public currentInput: string;
  displayedColumns = ['users', 'messages'];
  constructor(public service: WebSocketService) {

  }

  ngOnInit() {
    this.service.connect();
/*

   // this.users = [];
    this.usersFormated = [];
    this.messages = [];


    
   

    for (let i = 0; i < this.service.users.length; i++) {
      let user = this.service.users[i];
      if (user.length > 14) {
        let firstChars = user.substring(0, 11);
        this.usersFormated[i] = firstChars + '...';
        continue;
      }
      this.usersFormated[i] = user;
    }
    this.iterator = Array(this.service.users.length).fill(0).map((x, i) => i);*/
  }
  create()
  {
    this.service.create();
  }
  addMessage($event) {
    this.messages.push(new Message(this.currentInput, 'User Test tres tres tres tres long'));
    this.dataSource = [...this.messages];
    this.currentInput = '';
  }  

  endQuiz() {
    return;
  }
  starQuiz() {
    return;
  }

  


  
  

}

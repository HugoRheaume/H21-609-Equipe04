import { Subject } from 'rxjs';
import { Message } from './../../models/message';
import { Component, OnInit } from '@angular/core';
import { WebSocketService } from 'src/app/web-socket.service';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.scss']
})
export class WaitingRoomComponent implements OnInit {
  public messages$ = new Subject<string[]>();
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
    this.service.logReceive$().subscribe(data =>{
      this.messages$.next(data);
    });
    this.service.create();
  }
  cancel()
  {
    this.service.cancel();
  }
  addMessage($event) {
    this.messages.push(new Message(this.currentInput, 'User Test tres tres tres tres long'));
    this.dataSource = [...this.messages];
    this.currentInput = '';
  } 

  starQuiz() {
    return;
  }

  


  
  

}

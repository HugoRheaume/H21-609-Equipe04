import { Message } from './../../models/message';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.scss']
})
export class WaitingRoomComponent implements OnInit {

  public users: string[];
  public usersFormated: string[];
  public iterator: Array<number>;
  public shareCode: string;

  public messages: Message[] = [];
  public dataSource: Message[];
  public currentInput: string;
  displayedColumns = ['users', 'messages'];
  constructor() {

  }

  ngOnInit() {
    this.users = [];
    this.usersFormated = [];
    this.messages = [];
    this.shareCode = "ABCDEF";


    this.users.push('WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW');
    this.users.push('lllllllllllllllllllllllllllllllllll');
    for (let index = 0; index < 2; index++) {
      this.users.push('User ' + (index + 2));
    }

    for (let i = 0; i < this.users.length; i++) {
      let user = this.users[i];
      if (user.length > 14) {
        let firstChars = user.substring(0, 11);
        this.usersFormated[i] = firstChars + '...';
        continue;
      }
      this.usersFormated[i] = user;
    }
    this.iterator = Array(this.users.length).fill(0).map((x, i) => i);
  }

  addMessage($event) {
    this.messages.push(new Message(this.currentInput, 'User Test tres tres tres tres long'));
    this.dataSource = [...this.messages];
    this.currentInput = '';
  }

  addPlayer() {
    this.users.push('Ajout #' + this.users.length * 10);
    let user = this.users[this.users.length - 1];
    if (user.length > 14) {
      let firstChars = user.substring(0, 11);
      this.usersFormated[this.users.length - 1] = firstChars + '...';
    }
    else {
      this.usersFormated[this.users.length - 1] = user;
    }

    this.iterator = Array(this.users.length).fill(0).map((x, i) => i);
  }

  endGame() {
    return;
  }
  startGame() {
    return;
  }

  randomInt(min, max){
    return Math.floor(Math.random() * (max - min + 1)) + min;
 }

}

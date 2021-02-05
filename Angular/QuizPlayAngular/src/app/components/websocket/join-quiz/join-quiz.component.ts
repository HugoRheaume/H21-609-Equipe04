import { webSocket } from 'rxjs/webSocket';

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WebSocketService } from 'src/app/web-socket.service';


@Component({
  selector: 'app-join-quiz',
  templateUrl: './join-quiz.component.html',
  styleUrls: ['./join-quiz.component.css']
})
export class JoinQuizComponent implements OnInit {
 
  constructor(private router: Router, private route: ActivatedRoute, public service: WebSocketService) { }

  ngOnInit(): void {
    this.service.connect();
  }



  create(): void
  {
    this.service.create();
  }
  

}

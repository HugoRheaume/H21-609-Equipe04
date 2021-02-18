import { QuizService } from './services/Quiz.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'QuizPlayAngular';

  ngOnInit(): void { }

  constructor(public service: QuizService) { }
}

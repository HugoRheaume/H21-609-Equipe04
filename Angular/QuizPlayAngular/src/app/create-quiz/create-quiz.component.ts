import { Component, OnInit } from '@angular/core';
import { QuizRequest } from 'src/models/QuizRequest';
import { QuizService } from 'src/Quiz.service';

@Component({
  selector: 'app-create-quiz',
  templateUrl: './create-quiz.component.html',
  styleUrls: ['./create-quiz.component.css']
})
export class CreateQuizComponent implements OnInit {
  title: string = "";
  desc:string = "";
  isPublic:boolean = false;

  constructor(public http: QuizService)  {}

  ngOnInit(): void {
  }

  public create(): void {
    this.http.createQuiz(new QuizRequest(this.title, this.desc, this.isPublic))
  }
}

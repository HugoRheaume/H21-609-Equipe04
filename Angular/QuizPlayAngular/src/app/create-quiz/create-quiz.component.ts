import { Component, OnInit } from '@angular/core';
import { QuizRequest } from 'src/models/QuizRequest';
import { QuizResponse } from 'src/models/QuizResponse';
import { QuizService } from 'src/Quiz.service';

@Component({
  selector: 'app-create-quiz',
  templateUrl: './create-quiz.component.html',
  styleUrls: ['./create-quiz.component.css']
})
export class CreateQuizComponent implements OnInit {
  title: string = "";
  desc:string = "";
  isPublic = false;

  constructor(public http: QuizService)  {}

  ngOnInit(): void {
  }

  public create(): void {
    this.http.createQuiz(new QuizRequest(this.title, this.desc, this.isPublic)).subscribe(r => {
      console.log(r);
      if (r.toConfirm)
        this.toConfirm();
      else if (r.errorMessage)
        this.errorMessage("Title is too short");
      else if (r == null)
        this.errorMessage("An unexpected error occured");
      else
        console.log("nice!");
    });
  }

  public toConfirm(): void {
    console.log("bruv")
  }

  public errorMessage(err: string): void {
    console.log(err)
  }
}

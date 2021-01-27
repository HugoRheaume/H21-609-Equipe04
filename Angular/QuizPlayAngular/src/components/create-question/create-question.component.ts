import { QuestionTrueOrFalse } from './../../models/questionTrueOrFalse';
import { QuizService } from './../../Quiz.service';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-create-question',
  templateUrl: './create-question.component.html',
  styleUrls: ['./create-question.component.scss']
})
export class CreateQuestionComponent implements OnInit {

  public question: QuestionTrueOrFalse;
  constructor(public service: QuizService) { }

  ngOnInit() {
    this.question = new QuestionTrueOrFalse();
  }

  sumbit(){
    if(!this.question.hasAMaxTime && this.question.allowedTime != null) this.question.allowedTime = null;

    //Submit to the server
    alert('The question is : ' + this.question.label +
    "\nThe answer is : " + this.question.answer +
    "\nThe allowed time is : " + this.question.allowedTime);
    this.question = new QuestionTrueOrFalse();
  }

  discard(){
    this.question = new QuestionTrueOrFalse();
  }

}

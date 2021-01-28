import { QuizService } from './../../Quiz.service';
import { Component, OnInit } from '@angular/core';
import { QuestionTrueOrFalse } from 'src/models/question';
import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-create-question',
  templateUrl: './create-question.component.html',
  styleUrls: ['./create-question.component.scss']
})
export class CreateQuestionComponent implements OnInit {

  public question: QuestionTrueOrFalse;
  public TrueFalse: FormGroup;
  public hasTimeLimit: boolean;
  constructor(public service: QuizService, public route: Router, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.question = new QuestionTrueOrFalse();
    this.TrueFalse = this.formBuilder.group({
      questionLabel: ['', [Validators.required, Validators.min(1), Validators.max(250)]],
      questionAnswer: ['', [Validators.required]],
      questionTimeLimit: ['', Validators.min(0)]
    })
  }

  sumbit(){
    if(!this.hasTimeLimit && this.question.timeLimit != null) this.question.timeLimit = -1;
    if(this.question.label == null) return;

    //Submit to the server
    alert('The question is : ' + this.question.label +
    "\nThe answer is : " + this.question.answer +
    "\nThe allowed time is : " + this.question.timeLimit +
    "\nThe question type : " + this.question.questionType.toString());


    this.service.addQuestion(this.question).subscribe(res => {
      console.log(res)
    });
    this.question = new QuestionTrueOrFalse();
    this.route.navigate['/'];
  }

  discard(){
    this.question = new QuestionTrueOrFalse();
  }

}

export class InputErrorStateMatcher {
  questionLabel = new FormControl('', [
    Validators.required,
    Validators.min(1),
    Validators.max(250)
  ]);


  matcher = new MyErrorStateMatcher();
}

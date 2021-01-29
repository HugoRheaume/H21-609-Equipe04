import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { QuizService } from 'src/quiz.service';
import { QuestionChoice } from 'src/models/questionChoice';
@Component({
  selector: 'app-create-multiplechoices-question',
  templateUrl: './create-multiplechoices-question.component.html',
  styleUrls: ['./create-multiplechoices-question.component.scss']
})
export class CreateMultiplechoicesQuestionComponent implements OnInit {
  public MultipleChoice: FormGroup;
  public choices: QuestionChoice[];
  constructor(public service: QuizService,
    public route: Router,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.MultipleChoice = this.formBuilder.group({
      questionLabel: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(250)]],
      questionTimeLimit: ['', [Validators.required, Validators.min(1)]],
      questionHasTimeLimit: [''],
      questionHasOnlyOneAnswer: [''],
      questionAnswers: ['', Validators.required],
    });
    this.choices = [];
  }

  //#region Error messages
  get labelErrorMessage(): string {
    const formField: FormControl = this.MultipleChoice.get(
      'questionLabel'
    ) as FormControl;
    return formField.hasError('required')
      ? 'The label is required'
      : formField.hasError('maxlength')
        ? 'You have exceeded the maximum amount of characters'
        : formField.hasError('nowhitespaceerror')
          ? ''
          : ''; // Default
  }

  get timeLimitErrorMessage(): string {
    const formField: FormControl = this.MultipleChoice.get(
      'questionTimeLimit'
    ) as FormControl;
    return formField.hasError('min')
      ? 'The minimum value is 1'
      : formField.hasError('required')
        ? 'The time limit is required'
        : ''; // Default
  }
  //#endregion

  submit() {
    return;
  }

  discard(){
    this.MultipleChoice.reset();
    this.choices = [];
  }

  addChoice(){
    let newChoice = new QuestionChoice();
    newChoice.choiceNumber = this.choices.length + 1;
    newChoice.answer = false;
    newChoice.statement = "";
    this.choices.push(newChoice);
  }

  removeChoice(i: number){
    this.choices.splice(i-1, 1);

    let iterator = 1;
    this.choices.forEach(choice => {
      choice.choiceNumber = iterator;
      iterator++;
    })
  }

  // Returns false if ok
  get checkForm(): boolean{

    let questionLabel = this.MultipleChoice.get('questionLabel') as FormControl;
    let questionTimeLimit = this.MultipleChoice.get(
      'questionTimeLimit'
    ) as FormControl;
    let questionHasTimeLimit = this.MultipleChoice.get(
      'questionHasTimeLimit'
    ) as FormControl;

    if(this.choices.length < 2) return true;
    if(this.MultipleChoice.pristine) return true;
    if (questionLabel.value === '' || questionLabel.value == null) {
      // console.log('Label value is nothing');
      // alert('The label\'s value is nothing.');
      return true;
    }
    if (questionLabel.value.length > 250) {
      // console.log('Label is too long');
      // alert('The label\'s value is too long.');
      return true;
    }
    if (questionHasTimeLimit.value && (questionTimeLimit.value === '' || questionTimeLimit.value == null)) {
      // console.log('Time limit not defined');
      // alert('The time limit has not defined.');
      return true;
    }
    if ((questionHasTimeLimit.value !== "" && questionHasTimeLimit.value != null && questionHasTimeLimit.value != false) && questionTimeLimit.value < 1) {
      // console.log('Time limit less than 1');
      // alert('The time limit can't be less than 1.');
      return true;
    }
    let oneChoiceEmpty = false;
    this.choices.forEach(element => {
      if(element.statement === "") oneChoiceEmpty = true;
    });
    if(oneChoiceEmpty) return true;

    return false;
  }
}

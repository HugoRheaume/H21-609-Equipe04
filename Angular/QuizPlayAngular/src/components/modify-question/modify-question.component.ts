import { QuestionMultipleChoice } from './../../models/question';
import { QuestionService } from './../../question.service';
import { Question, QuestionTrueOrFalse, QuestionType } from 'src/models/question';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { QuestionChoice } from 'src/models/questionChoice';

@Component({
  selector: 'app-modify-question',
  templateUrl: './modify-question.component.html',
  styleUrls: ['./modify-question.component.scss']
})
export class ModifyQuestionComponent implements OnInit {
  @Input() question?: Question;
  @Output() close = new EventEmitter();
  @Output() isReady = new EventEmitter();

  onCloseSaved() {
    this.close.emit("saved");
  }
  onCloseDiscarded() {
    this.close.emit("discarded");
  }

  onReady() {
    this.isReady.emit(true);
  }

  public questionTrueFalse: QuestionTrueOrFalse;
  public questionMultipleChoice: QuestionMultipleChoice;
  public currentForm;

  //#region stuff for true or false
  public TrueFalse: FormGroup;
  //#endregion

  //#region stuff for multiple choice
  public MultipleChoice: FormGroup;
  public choices: QuestionChoice[] = [];
  public needsAllRightAnswers: boolean;
  //#endregion

  //#region stuff for the mat select
  public enumNames: string[];
  public enumValues: number[];
  public enumFormated: string[];
  public selectedType: string;
  public iterator: Array<number>;
  //#endregion

  constructor(
    public service: QuestionService,
    public route: Router,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    switch (this.question.questionType) {
      case QuestionType.MultipleChoices:
        this.questionMultipleChoice = this.question as QuestionMultipleChoice;
        let timeLimitMC = this.questionMultipleChoice.timeLimit == (-1) ? '' : this.questionMultipleChoice.timeLimit;
        let hasTimeLimitMC = this.questionMultipleChoice.timeLimit == (-1) ? false : true;
        let needsAllAnswers = this.questionMultipleChoice.needsAllAnswers;
        this.MultipleChoice = this.formBuilder.group({
          questionLabel: [
            this.questionMultipleChoice.label,
            [
              Validators.required,
              Validators.minLength(1),
              Validators.maxLength(250),
            ],
          ],
          questionTimeLimit: [timeLimitMC, [Validators.required, Validators.min(1)]],
          questionHasTimeLimit: [hasTimeLimitMC],
          questionHasOnlyOneAnswer: [needsAllAnswers],
          questionAnswers: ['', Validators.required],
        });

        let iterator = 1;
        this.questionMultipleChoice.questionMultipleChoice.forEach(choice => {
          let newChoice = new QuestionChoice();
          newChoice.answer = choice.answer;
          newChoice.statement = choice.statement
          newChoice.choiceNumber = iterator;
          newChoice.id = choice.id;
          newChoice.questionId = choice.questionId;
          iterator++;
          this.choices.push(newChoice);
        });
        this.needsAllRightAnswers = this.questionMultipleChoice.needsAllAnswers;
        this.currentForm = this.MultipleChoice;
        break;
      case QuestionType.TrueFalse:
        this.questionTrueFalse = this.question as QuestionTrueOrFalse;
        let answer = this.questionTrueFalse.answer == false ? 'false' : 'true';
        let timeLimitTF = this.questionTrueFalse.timeLimit == (-1) ? '' : this.questionTrueFalse.timeLimit;
        let hasTimeLimitTF = this.questionTrueFalse.timeLimit == (-1) ? false : true;

        this.TrueFalse = this.formBuilder.group({
          questionLabel: [
            this.questionTrueFalse.label,
            [
              Validators.required,
              Validators.minLength(1),
              Validators.maxLength(250),
            ],
          ],
          questionAnswer: [answer, [Validators.required]],
          questionTimeLimit: [timeLimitTF, [Validators.required, Validators.min(1)]],
          questionHasTimeLimit: [hasTimeLimitTF],
        });
        this.currentForm = this.TrueFalse;
        break;
    }

    //#region Values in the select form control.
    this.enumNames = [];
    this.enumValues = [];
    this.enumFormated = [];

    let unsortedNames = [];
    for (var type in QuestionType) {
      if (isNaN(Number(type))) {
        unsortedNames.push(type);
      }
    }
    this.enumNames = unsortedNames.sort((a, b) => {
      if (a > b) {
        return 1;
      }

      if (a < b) {
        return -1;
      }

      return 0;
    });
    for (let i = 0; i < this.enumNames.length; i++) {
      this.enumValues.push(QuestionType[this.enumNames[i]]);
      switch (this.enumNames[i]) {
        case 'TrueFalse':
          this.enumFormated.push('Vrai ou faux');
          if (this.questionTrueFalse != null) this.selectedType = '1';
          break;
        case 'MultipleChoices':
          this.enumFormated.push('Choix multiples');
          if (this.questionMultipleChoice != null) this.selectedType = '2';
          break;
        default:
          break;
      }
    }
    this.iterator = Array(this.enumNames.length).fill(0).map((x, i) => i);
    //#endregion

    this.onReady();
  }

  discard() {
    this.question.questionType == 1 ? this.TrueFalse.reset() : '';
    this.question.questionType == 2 ? this.MultipleChoice.reset() : '';
    this.choices = [];
    this.needsAllRightAnswers = true;
    this.onCloseDiscarded();
  }

  sumbit() {
    if (this.question.questionType == QuestionType.TrueFalse) {
      let questionTimeLimit = this.TrueFalse.get(
        'questionTimeLimit'
      ) as FormControl;
      let questionHasTimeLimit = this.TrueFalse.get(
        'questionHasTimeLimit'
      ) as FormControl;

      let question = this.questionTrueFalse;

      if (this.checkForm) return;

      //The question doesn't have a time limit
      if (
        questionHasTimeLimit.value == null ||
        questionHasTimeLimit.value == false ||
        questionHasTimeLimit.value === ''
      )
        question.timeLimit = -1;
      else question.timeLimit = questionTimeLimit.value;

      question.answer = this.TrueFalse.get('questionAnswer').value;
      question.label = this.TrueFalse.get('questionLabel').value;
      question.quizIndex = this.questionTrueFalse.quizIndex;

      this.TrueFalse.reset();

      this.service.modifyQuestion(question);
    }
    if (this.question.questionType == QuestionType.MultipleChoices) {
      let questionTimeLimit = this.MultipleChoice.get(
        'questionTimeLimit'
      ) as FormControl;
      let questionHasTimeLimit = this.MultipleChoice.get(
        'questionHasTimeLimit'
      ) as FormControl;

      let question = this.questionMultipleChoice;

      if (this.checkForm) return;

      //The question doesn't have a time limit
      if (
        questionHasTimeLimit.value == null ||
        questionHasTimeLimit.value == false ||
        questionHasTimeLimit.value === ''
      )
        question.timeLimit = -1;
      else question.timeLimit = questionTimeLimit.value;

      question.questionMultipleChoice = this.choices;
      question.needsAllAnswers = this.needsAllRightAnswers;
      question.label = this.MultipleChoice.get('questionLabel').value;
      question.quizIndex = this.questionMultipleChoice.quizIndex;


      console.log(this.choices);
      this.service.modifyQuestion(null, question);
      this.MultipleChoice.reset();
      this.choices = [];
      this.needsAllRightAnswers = true;
    }

    this.onCloseSaved()
  }

  //Checks the form, return false if ok
  get checkForm(): boolean {
    if (this.question.questionType == QuestionType.TrueFalse) {
      let questionLabel = this.TrueFalse.get('questionLabel') as FormControl;
      let questionAnswer = this.TrueFalse.get('questionAnswer') as FormControl;
      let questionTimeLimit = this.TrueFalse.get(
        'questionTimeLimit'
      ) as FormControl;
      let questionHasTimeLimit = this.TrueFalse.get(
        'questionHasTimeLimit'
      ) as FormControl;
      //The user changed the label to nothing
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
      //The user didn't answer
      if (questionAnswer.value === '' || questionAnswer.value == null) {
        // console.log('Answer is not answered');
        // alert('The answer has not been defined.');
        return true;
      }
      if (
        questionHasTimeLimit.value &&
        (questionTimeLimit.value === '' || questionTimeLimit.value == null)
      ) {
        // console.log('Time limit not defined');
        // alert('The time limit has not defined.');
        return true;
      }
      if (
        questionHasTimeLimit.value !== '' &&
        questionHasTimeLimit.value != null &&
        questionHasTimeLimit.value != false &&
        questionTimeLimit.value < 1
      ) {
        // console.log('Time limit less than 1');
        // alert('The time limit can't be less than 1.');
        return true;
      }
      return false;
    }
    if (this.question.questionType == QuestionType.MultipleChoices) {
      let questionLabel = this.MultipleChoice.get('questionLabel') as FormControl;
      let questionTimeLimit = this.MultipleChoice.get(
        'questionTimeLimit'
      ) as FormControl;
      let questionHasTimeLimit = this.MultipleChoice.get(
        'questionHasTimeLimit'
      ) as FormControl;

      if (this.choices.length < 2) return true;
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
      if (
        questionHasTimeLimit.value &&
        (questionTimeLimit.value === '' || questionTimeLimit.value == null)
      ) {
        // console.log('Time limit not defined');
        // alert('The time limit has not defined.');
        return true;
      }
      if (
        questionHasTimeLimit.value !== '' &&
        questionHasTimeLimit.value != null &&
        questionHasTimeLimit.value != false &&
        questionTimeLimit.value < 1
      ) {
        // console.log('Time limit less than 1');
        // alert('The time limit can't be less than 1.');
        return true;
      }
      let oneChoiceEmpty = false;
      let noChoiceChecked = true;
      this.choices.forEach(element => {
        if (element.statement === '') oneChoiceEmpty = true;
        if (element.answer === true) noChoiceChecked = false;
      });
      if (oneChoiceEmpty) return true;
      if (noChoiceChecked) return true;

      return false;
    }
    return false;
  }

  //#region Error messages
  get labelErrorMessage(): string {
    const formField: FormControl = this.currentForm.get(
      'questionLabel'
    ) as FormControl;
    return formField.hasError('required')
      ? "L'énoncé est requis"
      : formField.hasError('maxlength')
        ? 'Vous avez dépassé le nombre de caractères maximum'
        : formField.hasError('nowhitespaceerror')
          ? ''
          : ''; // Default
  }

  get timeLimitErrorMessage(): string {
    const formField: FormControl = this.currentForm.get(
      'questionTimeLimit'
    ) as FormControl;
    return formField.hasError('min')
      ? 'La valeur minimale est 1'
      : formField.hasError('required')
        ? 'Le temps limite est requis'
        : ''; // Default
  }
  //#endregion

  addChoice() {
    let newChoice = new QuestionChoice();
    newChoice.choiceNumber = this.choices.length + 1;
    newChoice.answer = false;
    newChoice.statement = '';
    this.choices.push(newChoice);
  }

  removeChoice(i: number) {
    this.choices.splice(i - 1, 1);

    let iterator = 1;
    this.choices.forEach(choice => {
      choice.choiceNumber = iterator;
      iterator++;
    });
  }

  get trueAnswers(): number {
    let amount = 0;
    this.choices.forEach(choice => {
      if (choice.answer) amount++;
    });
    return amount;
  }
}

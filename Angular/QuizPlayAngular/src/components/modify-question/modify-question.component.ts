import { TranslateService } from '@ngx-translate/core';
import { QuestionAssociation } from './../../app/models/question';
import { QuestionAsso } from './../../models/questionAsso';
import { QuestionService } from './../../app/services/question.service';
import { QuestionMultipleChoice } from '../../app/models/question';
import {
  Question,
  QuestionTrueOrFalse,
  QuestionType,
} from 'src/app/models/question';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { QuestionChoice } from 'src/app/models/questionChoice';
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-modify-question',
  templateUrl: './modify-question.component.html',
  styleUrls: ['./modify-question.component.scss'],
})
export class ModifyQuestionComponent implements OnInit {
  @Input() question?: Question;
  @Output() close = new EventEmitter();
  @Output() isReady = new EventEmitter();

  onCloseSaved() {
    this.close.emit('saved');
  }
  onCloseDiscarded() {
    this.close.emit('discarded');
  }

  onReady() {
    this.isReady.emit(true);
  }

  public questionTrueFalse: QuestionTrueOrFalse;
  public questionMultipleChoice: QuestionMultipleChoice;
  public questionAssociation: QuestionAssociation;
  public currentForm;

  //#region stuff for true or false
  public TrueFalse: FormGroup;
  //#endregion

  //#region stuff for multiple choice
  public MultipleChoice: FormGroup;
  public choices: QuestionChoice[] = [];
  public needsAllRightAnswers: boolean;
  //#endregion

  //#region stuff for association
  public Association: FormGroup;
  public asso: QuestionAsso[] = [];
  public categories: string[] = [];
  public showCategory3: boolean;
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
    private formBuilder: FormBuilder,
    public translate: TranslateService
  ) { }

  ngOnInit() {
    switch (this.question.questionType) {
      case QuestionType.MultipleChoices:
        this.questionMultipleChoice = this.question as QuestionMultipleChoice;
        let timeLimitMC =
          this.questionMultipleChoice.timeLimit == -1
            ? ''
            : this.questionMultipleChoice.timeLimit;
        let hasTimeLimitMC =
          this.questionMultipleChoice.timeLimit == -1 ? false : true;
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
          questionTimeLimit: [
            timeLimitMC,
            [Validators.required, Validators.min(1), Validators.max(3600)],
          ],
          questionHasTimeLimit: [hasTimeLimitMC],
          questionHasOnlyOneAnswer: [needsAllAnswers],
          questionAnswers: ['', Validators.required],
        });

        let iterator = 1;
        this.questionMultipleChoice.questionMultipleChoice.forEach((choice) => {
          let newChoice = new QuestionChoice();
          newChoice.answer = choice.answer;
          newChoice.statement = choice.statement;
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
        let answer =
          this.questionTrueFalse.questionTrueFalse.answer == false
            ? 'false'
            : 'true';
        let timeLimitTF =
          this.questionTrueFalse.timeLimit == -1
            ? ''
            : this.questionTrueFalse.timeLimit;
        let hasTimeLimitTF =
          this.questionTrueFalse.timeLimit == -1 ? false : true;

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
          questionTimeLimit: [
            timeLimitTF,
            [Validators.required, Validators.min(1), Validators.max(3600)],
          ],
          questionHasTimeLimit: [hasTimeLimitTF],
        });
        this.currentForm = this.TrueFalse;
        break;
      case QuestionType.Association:
        this.questionAssociation = this.question as QuestionAssociation;

        let timeLimitA =
          this.questionAssociation.timeLimit == -1
            ? ''
            : this.questionAssociation.timeLimit;
        let hasTimeLimitA =
          this.questionAssociation.timeLimit == -1 ? false : true;
        this.Association = this.formBuilder.group({
          questionLabel: [
            this.questionAssociation.label,
            [
              Validators.required,
              Validators.minLength(1),
              Validators.maxLength(250),
            ],
          ],
          questionTimeLimit: [
            timeLimitA,
            [Validators.required, Validators.min(1), Validators.max(3600)],
          ],
          questionHasTimeLimit: [hasTimeLimitA],
          questionHasOnlyOneAnswer: [''],
          questionAnswers: ['', Validators.required],
        });
        let i = 1;
        this.questionAssociation.questionAssociation.forEach((a) => {
          let association = new QuestionAsso();
          association.assoNumber = i;
          association.categoryIndex = a.categoryIndex;
          association.statement = a.statement;
          this.asso.push(association);
          i++;
        });
        this.questionAssociation.categories.forEach((c) => {
          if (c != null) {
            let category = c.toString();
            this.categories.push(category);
          }
        });
        if (this.categories[2] == null) this.categories.splice(2, 1);
        this.showCategory3 = this.categories.length > 2;
        this.currentForm = this.Association;
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
          this.enumFormated.push(this.translate.instant('app.choice.tf'));
          if (this.questionTrueFalse != null) this.selectedType = '1';
          break;
        case 'MultipleChoices':
          this.enumFormated.push(this.translate.instant('app.choice.mc'));
          if (this.questionMultipleChoice != null) this.selectedType = '2';
          break;
        case 'Association':
          this.enumFormated.push(this.translate.instant('app.choice.as'));
          if (this.questionAssociation != null) this.selectedType = '3';
          break;
        default:
          break;
      }
    }
    this.iterator = Array(this.enumNames.length)
      .fill(0)
      .map((x, i) => i);
    //#endregion

    this.onReady();
  }

  discard() {
    this.question.questionType == 1 ? this.TrueFalse.reset() : '';
    this.question.questionType == 2 ? this.MultipleChoice.reset() : '';
    this.question.questionType == 3 ? this.Association.reset() : '';

    this.choices = [];
    this.needsAllRightAnswers = true;

    this.asso = [];
    this.categories = [];
    this.showCategory3 = false;

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

      this.service.modifyQuestion(null, question);
      this.MultipleChoice.reset();
      this.choices = [];
      this.needsAllRightAnswers = true;
    }
    if (this.question.questionType == QuestionType.Association) {
      let questionTimeLimit = this.Association.get(
        'questionTimeLimit'
      ) as FormControl;
      let questionHasTimeLimit = this.Association.get(
        'questionHasTimeLimit'
      ) as FormControl;

      let question = this.questionAssociation;

      if (this.checkForm) return;

      //The question doesn't have a time limit
      if (
        questionHasTimeLimit.value == null ||
        questionHasTimeLimit.value == false ||
        questionHasTimeLimit.value === ''
      )
        question.timeLimit = -1;
      else question.timeLimit = questionTimeLimit.value;

      question.questionAssociation = this.asso;

      let temp: string[] = [];
      temp[0] = this.categories[0];
      temp[1] = this.categories[1];
      if (this.showCategory3) temp[2] = this.categories[2];
      question.categories = temp;
      question.label = this.Association.get('questionLabel').value;

      this.service.modifyQuestion(null, null, question);
      this.asso = [];
      this.categories = ['', ''];
      this.showCategory3 = false;
    }

    this.onCloseSaved();
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
        (questionHasTimeLimit.value !== '' &&
          questionHasTimeLimit.value != null &&
          questionHasTimeLimit.value != false &&
          questionTimeLimit.value < 1) ||
        questionTimeLimit.value > 3600
      ) {
        // console.log('Time limit less than 1');
        // alert('The time limit can't be less than 1.');
        return true;
      }
      return false;
    }
    if (this.question.questionType == QuestionType.MultipleChoices) {
      let questionLabel = this.MultipleChoice.get(
        'questionLabel'
      ) as FormControl;
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
        (questionHasTimeLimit.value !== '' &&
          questionHasTimeLimit.value != null &&
          questionHasTimeLimit.value != false &&
          questionTimeLimit.value < 1) ||
        questionTimeLimit.value > 3600
      ) {
        // console.log('Time limit less than 1');
        // alert('The time limit can't be less than 1.');
        return true;
      }
      let oneChoiceEmpty = false;
      let noChoiceChecked = true;
      this.choices.forEach((element) => {
        if (element.statement === '') oneChoiceEmpty = true;
        if (element.answer === true) noChoiceChecked = false;
      });
      if (oneChoiceEmpty) return true;
      if (noChoiceChecked) return true;

      return false;
    }
    if (this.question.questionType == QuestionType.Association) {
      let questionLabel = this.Association.get('questionLabel') as FormControl;
      let questionTimeLimit = this.Association.get(
        'questionTimeLimit'
      ) as FormControl;
      let questionHasTimeLimit = this.Association.get(
        'questionHasTimeLimit'
      ) as FormControl;

      if (this.asso.length < 1) return true;
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
        (questionHasTimeLimit.value !== '' &&
          questionHasTimeLimit.value != null &&
          questionHasTimeLimit.value != false &&
          questionTimeLimit.value < 1) ||
        questionTimeLimit.value > 3600
      ) {
        // console.log('Time limit less than 1');
        // alert('The time limit can't be less than 1.');
        return true;
      }
      let oneChoiceEmpty = false;
      this.asso.forEach((element) => {
        if (element.statement === '') oneChoiceEmpty = true;
      });
      if (oneChoiceEmpty) return true;

      if (this.checkEmptyCategories) return true;
    }
    return false;
  }

  //#region Error messages
  get labelErrorMessage(): string {
    const formField: FormControl = this.currentForm.get(
      'questionLabel'
    ) as FormControl;
    return formField.hasError('required')
      ? this.translate.instant('app.error.question.create.labelRequired')
      : formField.hasError('maxlength')
        ? this.translate.instant('app.error.question.create.labelMax')
        : formField.hasError('nowhitespaceerror')
          ? ''
          : ''; // Default
  }

  get timeLimitErrorMessage(): string {
    const formField: FormControl = this.currentForm.get(
      'questionTimeLimit'
    ) as FormControl;
    return formField.hasError('min')
      ? this.translate.instant('app.error.question.create.timeMin')
      : formField.hasError('max')
        ? this.translate.instant('app.error.question.create.timeMax')
        : formField.hasError('required')
          ? this.translate.instant('app.error.question.create.timeRequired')
          : ''; // Default
  }
  //#endregion

  //#region Methods for multiple choices
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
    this.choices.forEach((choice) => {
      choice.choiceNumber = iterator;
      iterator++;
    });
  }

  get trueAnswers(): number {
    let amount = 0;
    this.choices.forEach((choice) => {
      if (choice.answer) amount++;
    });
    return amount;
  }
  //#endregion

  //#region Methods for association
  addChoiceAsso() {
    let newAsso = new QuestionAsso();
    newAsso.assoNumber = this.asso.length + 1;
    newAsso.categoryIndex = 0;
    newAsso.statement = '';
    this.asso.push(newAsso);
  }

  removeChoiceAsso(i: number) {
    this.asso.splice(i - 1, 1);

    let iterator = 1;
    this.asso.forEach((asso) => {
      asso.assoNumber = iterator;
      iterator++;
    });
  }

  get checkEmptyCategories(): boolean {
    let hasEmpty = false;
    this.categories.forEach((cat) => {
      if (cat != null) {
        cat.trim;
        if (cat.length == 0) {
          console.log(cat);
          hasEmpty = true;
        }
      }
    });
    return hasEmpty;
  }

  switchCategory3(): void {
    if (this.showCategory3) {
      this.categories.splice(2, 1);
      let list: QuestionAsso[] = this.GetListOfCategory(2);
      if (list.length > 0) {
        list.forEach((item) => {
          this.removeChoiceAsso(item.assoNumber);
        });
      }
    }
    if (!this.showCategory3) {
      this.categories[2] = '';
    }
    this.showCategory3 = !this.showCategory3;
  }

  drop(event: CdkDragDrop<string[]>, category: number) {
    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
      console.log(event.container.data,
        event.previousIndex,
        event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }
    event.item.data.categoryIndex = category;
  }

  public GetListOfCategory(index: number): QuestionAsso[] {
    let listAsso: QuestionAsso[] = [];

    this.asso.forEach((item) => {
      if (item.categoryIndex == index) listAsso.push(item);
    });
    console.log(listAsso);
    return listAsso;
  }
  //#endregion
}

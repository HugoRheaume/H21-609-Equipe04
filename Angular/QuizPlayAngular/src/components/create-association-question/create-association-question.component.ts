import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { QuestionAsso } from 'src/models/questionAsso';
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';
import { QuizService } from 'src/app/services/Quiz.service';
import { QuestionAssociation } from 'src/app/models/question';
@Component({
  selector: 'app-create-association-question',
  templateUrl: './create-association-question.component.html',
  styleUrls: ['./create-association-question.component.css'],
})
export class CreateAssociationQuestionComponent implements OnInit {
  public Association: FormGroup;
  public asso: QuestionAsso[];
  public categories: string[];
  public showCategory3: boolean;
  constructor(
    public service: QuizService,
    public route: Router,
    private formBuilder: FormBuilder,
    public translate: TranslateService
  ) { }

  ngOnInit() {
    this.Association = this.formBuilder.group({
      questionLabel: [
        '',
        [
          Validators.required,
          Validators.minLength(1),
          Validators.maxLength(250),
        ],
      ],
      questionTimeLimit: [
        '',
        [Validators.required, Validators.min(1), Validators.max(3600)],
      ],
      questionHasTimeLimit: [''],
      questionHasOnlyOneAnswer: [''],
      questionAnswers: ['', Validators.required],
    });
    this.asso = [];
    this.categories = ['', ''];
    this.showCategory3 = false;
    console.log(this.categories);
  }

  //#region Error messages
  get labelErrorMessage(): string {
    const formField: FormControl = this.Association.get(
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
    const formField: FormControl = this.Association.get(
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

  submit() {
    let questionTimeLimit = this.Association.get(
      'questionTimeLimit'
    ) as FormControl;
    let questionHasTimeLimit = this.Association.get(
      'questionHasTimeLimit'
    ) as FormControl;

    let question = new QuestionAssociation();

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

    this.discard();

    this.service.addQuestion(question.toAssociationDTO());
    //this.service.addQuestion(question.toDTO());
    this.route.navigate['/'];

    return;
  }

  discard() {
    this.Association.reset();
    this.asso = [];
    this.categories = [];
    this.showCategory3 = false;
  }

  addChoice() {
    let newAsso = new QuestionAsso();
    newAsso.assoNumber = this.asso.length + 1;
    newAsso.categoryIndex = 0;
    newAsso.statement = '';
    this.asso.push(newAsso);
  }

  removeChoice(i: number) {
    this.asso.splice(i - 1, 1);

    let iterator = 1;
    this.asso.forEach((asso) => {
      asso.assoNumber = iterator;
      iterator++;
    });
  }

  // Returns false if ok
  get checkForm(): boolean {
    let questionLabel = this.Association.get('questionLabel') as FormControl;
    let questionTimeLimit = this.Association.get(
      'questionTimeLimit'
    ) as FormControl;
    let questionHasTimeLimit = this.Association.get(
      'questionHasTimeLimit'
    ) as FormControl;

    if (this.asso.length < 1) return true;
    if (this.Association.pristine) return true;
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

    return false;
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
          this.removeChoice(item.assoNumber);
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
      console.log(event.container.data);
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
    return listAsso;
  }
}

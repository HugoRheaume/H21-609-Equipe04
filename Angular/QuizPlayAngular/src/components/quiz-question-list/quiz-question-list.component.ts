import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Question, QuestionType } from 'src/app/models/question';
import { Component, Input, OnInit } from '@angular/core';
import { QuizService } from 'src/app/services/Quiz.service';
import { ActivatedRoute, Router } from '@angular/router';
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';
import { QuizResponse } from 'src/app/models/QuizResponse';

@Component({
  selector: 'app-quiz-question-list',
  templateUrl: './quiz-question-list.component.html',
  styleUrls: ['./quiz-question-list.component.scss'],
})
export class QuizQuestionListComponent implements OnInit {
  @Input() quizId: number;
  public currentQuiz: QuizResponse;
  public selectedQuestion: Question;
  public isModifying: boolean = false;
  public enum = QuestionType;
  public modifyingQuestion: Question;
  public showModifyingModal: boolean = false;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    public service: QuizService,
    private snackBar: MatSnackBar,
    public translate: TranslateService
  ) {}
  ngOnInit(): void {
    var quizShareCode: string = this.route.snapshot.paramMap.get(
      'quizShareCode'
    );
    this.service.getQuiz(quizShareCode);
    this.service.getQuestionFromQuiz(quizShareCode);
    this.currentQuiz = this.service.currentQuiz;
  }

  deleteQuestion(questionId: number): void {
    this.service.deleteQuestion(questionId);
    this.openSnackBarDeleteQuestion();
  }

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }
  }

  finish() {
    let i = 0;
    this.service.currentQuestions.forEach((item) => {
      item.quizIndex = i;
      i++;
    });
    this.service.updateQuizIndex();
  }

  putFirst() {
    let i = 0;
    this.service.currentQuestions.forEach((item) => {
      if (item.id === this.selectedQuestion.id) {
        moveItemInArray(this.service.currentQuestions, i, 0);
      } else i++;
    });
  }

  putLast() {
    let i = 0;
    this.service.currentQuestions.forEach((item) => {
      if (item.id === this.selectedQuestion.id) {
        moveItemInArray(
          this.service.currentQuestions,
          i,
          this.service.currentQuestions.length - 1
        );
      }
      i++;
    });
  }

  goLive() {
    this.finish();
    this.router.navigate([`/live/${this.service.currentQuiz.shareCode}`]);
  }

  updateQuiz() {
    this.service.modifyQuiz(this.service.currentQuiz);
    this.openSnackBarSaveQuiz();
  }

  openSnackBarSaveQuiz() {
    this.isModifying = false;
    this.snackBar.open(this.translate.instant('app.quiz.edit.modify'), null, {
      duration: 3000,
    });
  }

  openSnackBarSaveQuestion() {
    this.modifyingQuestion = null;
    this.showModifyingModal = false;
    this.snackBar.open(
      this.translate.instant('app.quiz.edit.questionModify'),
      null,
      {
        duration: 3000,
      }
    );
  }

  openSnackBarDeleteQuestion() {
    this.snackBar.open(
      this.translate.instant('app.quiz.edit.questionDeleted'),
      null,
      {
        duration: 3000,
      }
    );
  }

  updateQuestions($event) {
    $event == 'saved' ? this.openSnackBarSaveQuestion() : null;
    this.showModifyingModal = false;
  }

  setQuestionToModify() {
    this.showModifyingModal = true;
    this.modifyingQuestion = this.selectedQuestion;
  }

  openModal() {
    document.getElementById('openModalButton').click();
  }
}

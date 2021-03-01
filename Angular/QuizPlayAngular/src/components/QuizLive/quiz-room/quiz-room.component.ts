import { TranslateService } from '@ngx-translate/core';
import { WebSocketService } from 'src/app/services/web-socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Question } from 'src/app/models/question';
import { QuizService } from '../../../app/services/Quiz.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { take, tap } from 'rxjs/operators';
import { interval, Observable, Subscription } from 'rxjs';
@Component({
  selector: 'app-quiz-room',
  templateUrl: './quiz-room.component.html',
  styleUrls: ['./quiz-room.component.css'],
})
export class QuizRoomComponent implements OnInit, OnDestroy {
  public currentQuestion$: Observable<Question>;
  public forceSkipSub: Subscription;

  public isShowResult: boolean = false;
  public isScoreboardPage: boolean = false;
  public isSkippable: boolean = false;
  public isLastQuestion: boolean = false;
  public currentIndex: number;
  public quizShareCode: string;
  public timeLeft: number = 0;
  private countdownSub: Subscription;
  public currentQuestion: Question;

  public button = {
    action: 'skip',
    text: this.translate.instant('app.room.btnSkip'),
  };
  public spinnerValue: number = 100;
  constructor(
    public route: ActivatedRoute,
    public quizService: QuizService,
    public router: Router,
    public wsService: WebSocketService,
    public translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.isShowResult = false;
    this.isScoreboardPage = false;
    this.wsService.scoreboard = [];
    this.quizShareCode = this.route.snapshot.paramMap.get('quizShareCode');
    this.currentIndex = parseInt(
      this.route.snapshot.paramMap.get('questionIndex')
    );
    this.quizService.getQuestionFromQuiz(this.quizShareCode);
    this.currentQuestion$ = this.wsService.currentQuestion$.pipe(
      tap((q) => {
        this.spinnerValue = 100;
        this.startTimer(q.timeLimit);
      })
    );
    this.forceSkipSub = this.wsService.forceSkip$.subscribe(() => this.skip());
  }

  ngOnDestroy(): void {
    this.stopTimer();
    this.wsService.canDestroy = true;
    this.wsService.cancel();

    this.isShowResult = false;
    this.isScoreboardPage = false;

    this.forceSkipSub.unsubscribe();
  }
  finish() {
    this.stopTimer();
    this.wsService.cancel();
    this.isShowResult = false;
    this.isScoreboardPage = false;
    this.wsService.scoreboard = [];
    this.router.navigate(['/quiz/' + this.quizShareCode]);
  }

  nextQuestion() {
    this.isScoreboardPage = !this.isScoreboardPage;
    if (this.currentIndex >= this.quizService.currentQuestions.length - 1) {
      this.isLastQuestion = true;
      this.updateButton(this.translate.instant('app.room.btnFinish'), 'finish');
    }

    if (!this.isScoreboardPage) {
      this.updateButton(this.translate.instant('app.room.btnSkip'), 'skip');

      this.currentIndex++;
      this.wsService.nextQuestion(this.currentIndex);
      this.isShowResult = false;
      this.wsService.scoreboard = [];
      this.router.navigate([
        '/live/' + this.quizShareCode + '/' + this.currentIndex,
      ]);
      this.isSkippable = true;
    }
  }

  skip(): void {
    this.updateButton(
      this.translate.instant('app.room.btnNext'),
      'nextQuestion'
    );

    this.stopTimer();
    this.wsService.resultQuestion();
    this.isShowResult = true;
    this.isSkippable = false;
  }

  private startTimer(timeLimit: number) {
    this.stopTimer();
    this.timeLeft = timeLimit;

    const countdown$ = interval(1000).pipe(
      tap(() => {
        this.timeLeft--;
        this.spinnerValue = (this.timeLeft * 100) / timeLimit;
      }),
      take(timeLimit)
    );

    this.countdownSub = countdown$.subscribe({ complete: () => this.skip() });
  }

  private stopTimer(): void {
    if (this.countdownSub) {
      this.countdownSub.unsubscribe();
      this.countdownSub = undefined;
    }
  }

  private updateButton(text: string, action: string): void {
    this.button.text = text;
    this.button.action = action;
  }
}

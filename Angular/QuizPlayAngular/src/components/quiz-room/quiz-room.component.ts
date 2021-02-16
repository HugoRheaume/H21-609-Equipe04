import { Subject, Observable, timer, Subscription, interval } from 'rxjs';
import { WebSocketService } from 'src/app/web-socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Question, QuestionType } from 'src/models/question';
import { QuizService } from './../../quiz.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { finalize, take, tap } from 'rxjs/operators';

@Component({
  selector: 'app-quiz-room',
  templateUrl: './quiz-room.component.html',
  styleUrls: ['./quiz-room.component.css']
})
export class QuizRoomComponent implements OnInit, OnDestroy {
  public currentQuestion$: Observable<Question>;

  public isShowResult: boolean = false;
  public isScoreboardPage: boolean = false;
  public isSkippable: boolean = false;
  public isLastQuestion: boolean = false;
  public currentIndex: number;
  public quizShareCode: string
  public timeLeft: number = 0;
  private countdownSub: Subscription;
  public currentQuestion: Question;

  constructor(public route: ActivatedRoute, public quizService: QuizService, public router: Router, public wsService: WebSocketService) { }
  
  ngOnInit(): void {
    this.isShowResult = false;
    this.isScoreboardPage = false;

    this.quizShareCode = this.route.snapshot.paramMap.get('quizShareCode');
    this.currentIndex = parseInt(this.route.snapshot.paramMap.get('questionIndex'));
    this.quizService.getQuestionFromQuiz(this.quizShareCode);
    this.currentQuestion$ =  this.wsService.currentQuestion$.pipe(
      tap((q) => {
        this.startTimer(q.timeLimit)
      })
    );    
  }
  
  ngOnDestroy(): void {
    this.stopTimer();
    this.wsService.cancel();

    this.isShowResult = false;
    this.isScoreboardPage = false;
  }

  nextQuestion()
  {
    this.isScoreboardPage = !this.isScoreboardPage;
    if(this.currentIndex >= this.quizService.currentQuestions.length-1)
      this.isLastQuestion = true;
    
    if(!this.isScoreboardPage)
    {
      this.currentIndex++;
      this.wsService.nextQuestion(this.currentIndex);
      this.isShowResult = false;
      this.router.navigate(['/live/'+ this.quizShareCode +  '/' + this.currentIndex]);
      this.isSkippable = true;

    }
  }

  skip(): void
  {
    //TODO
    //if(!this.isSkippable)
    //  return;
    this.stopTimer();
    this.wsService.resultQuestion();
    this.isShowResult = true;
    this.isSkippable = false;
  }

  private startTimer(timeLimit: number) {

    this.stopTimer();
    this.timeLeft = timeLimit;

    const countdown$ = interval(1000).pipe(
      tap(() => this.timeLeft--),
      finalize(() => this.skip()),
      take(timeLimit)
    );

    this.countdownSub= countdown$.subscribe();  
  }

  private stopTimer(): void{
    if(this.countdownSub)
      this.countdownSub.unsubscribe();
    
  }
}

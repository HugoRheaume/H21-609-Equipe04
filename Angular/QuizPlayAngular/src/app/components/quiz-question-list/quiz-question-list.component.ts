import { MatGridListModule } from '@angular/material/grid-list';
import { QuestionTrueOrFalse } from './../../../models/question';
import { Question } from 'src/models/question';
import { Component, Input, OnInit } from '@angular/core';
import { QuizService } from 'src/quiz.service';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-quiz-question-list',
  templateUrl: './quiz-question-list.component.html',
  styleUrls: ['./quiz-question-list.component.css']
})
export class QuizQuestionListComponent implements OnInit {

  @Input() quizId: number;
  constructor(private router: Router, private route: ActivatedRoute, public service: QuizService) { }

  ngOnInit(): void {
    console.log(this.route.snapshot.paramMap.get('quizId'));
    var quizId: number = +this.route.snapshot.paramMap.get('quizId');
    this.service.getQuiz(quizId);
    this.service.getQuestionFromQuiz(quizId);


    let q = new QuestionTrueOrFalse();
    q.label = 'Quelle est la couleur du cheval blanc de Napoléon?';
    q.timeLimit = 52;
    
    let q2 = new QuestionTrueOrFalse();
    q2.label = 'Es-tu un joueur épique?';
    q2.timeLimit = -1;
    this.service.currentQuestions.push(q);
    this.service.currentQuestions.push(q2);
  }


  deleteQuestion(questionId: number): void { 
    this.service.deleteQuestion(questionId)
  }


}

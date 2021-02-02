import { MatGridListModule } from '@angular/material/grid-list';
import { QuestionTrueOrFalse } from './../../../models/question';
import { Question, QuestionType } from 'src/models/question';
import { Component, Input, OnInit } from '@angular/core';
import { QuizService } from 'src/quiz.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';


@Component({
  selector: 'app-quiz-question-list',
  templateUrl: './quiz-question-list.component.html',
  styleUrls: ['./quiz-question-list.component.css']
})
export class QuizQuestionListComponent implements OnInit {

  @Input() quizId: number;
  constructor(private router: Router, private route: ActivatedRoute, public service: QuizService) { }


  enum = QuestionType;
  ngOnInit(): void {
    console.log(this.route.snapshot.paramMap.get('quizId'));
    var quizId: number = +this.route.snapshot.paramMap.get('quizId');
    this.service.getQuiz(quizId);
    this.service.getQuestionFromQuiz(quizId);

    
  }


  deleteQuestion(questionId: number): void { 
    this.service.deleteQuestion(questionId)
  }
  drop(event: CdkDragDrop<string[]>) {

    
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
                        event.container.data,
                        event.previousIndex,
                        event.currentIndex);
    }
  }

  finish()
  {
    
    let i = 0;
    this.service.currentQuestions.forEach(item => {
      item.quizIndex = i;
      i++;
    });

    console.log(this.service.currentQuestions);
    console.log(this.service.currentQuestions.sort((a, b) => (a.quizIndex > b.quizIndex) ? 1 : -1));
  }

}

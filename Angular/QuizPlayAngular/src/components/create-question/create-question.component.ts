import { Component, OnInit } from '@angular/core';
import { Question, QuestionType } from 'src/models/question';

@Component({
  selector: 'app-create-question',
  templateUrl: './create-question.component.html',
  styleUrls: ['./create-question.component.scss']
})
export class CreateQuestionComponent implements OnInit {

  public enumNames: string[];
  public iterator: Array<number>;
  public selectedValue: string;
  constructor() { }

  ngOnInit() {
    this.enumNames = [];
    for (var type in QuestionType) {
      if (isNaN(Number(type))) {
        this.enumNames.push(type);
        console.log(type)
      }
    }
    this.iterator = Array(this.enumNames.length).fill(1).map((x, i) => i +1);
    console.log(this.iterator)
    console.log(this.selectedValue)
  }
}

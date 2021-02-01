import { Component, OnInit } from '@angular/core';
import { Question, QuestionType } from 'src/models/question';

@Component({
  selector: 'app-create-question',
  templateUrl: './create-question.component.html',
  styleUrls: ['./create-question.component.scss']
})
export class CreateQuestionComponent implements OnInit {

  public enumNames: string[];
  public enumValues: number[];
  public iterator: Array<number>;
  public selectedValue: string;
  constructor() { }

  ngOnInit() {
    this.enumNames = [];
    this.enumValues = [];
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
    }
    this.iterator = Array(this.enumNames.length).fill(0).map((x, i) => i);

  }
}

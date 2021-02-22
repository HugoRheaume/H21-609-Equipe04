import { Component, OnInit } from '@angular/core';
import { QuestionType } from 'src/app/models/question';

@Component({
  selector: 'app-create-question',
  templateUrl: './create-question.component.html',
  styleUrls: ['./create-question.component.scss'],
})
export class CreateQuestionComponent implements OnInit {
  public enumNames: string[];
  public enumValues: number[];
  public enumFormated: string[];
  public iterator: Array<number>;
  public selectedValue: string;
  constructor() { }

  ngOnInit() {
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
          break;
        case 'MultipleChoices':
          this.enumFormated.push('Choix multiples');
          break;
        case 'Association':
          this.enumFormated.push("Association")
        default:
          break;
      }
    }

    this.iterator = Array(this.enumNames.length)
      .fill(0)
      .map((x, i) => i);
  }
}

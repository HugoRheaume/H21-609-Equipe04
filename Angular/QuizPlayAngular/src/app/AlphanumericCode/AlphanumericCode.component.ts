import { QuizService } from './../../Quiz.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-AlphanumericCode',
  templateUrl: './AlphanumericCode.component.html',
  styleUrls: ['./AlphanumericCode.component.css']
})
export class AlphanumericCodeComponent implements OnInit {

  alphanumericCode: string;

  constructor(public service: QuizService) { }

  ngOnInit(): void{
    this.service.getAlphanumericCode()
    .subscribe(response => {
      this.alphanumericCode = response;
    })
  }

}

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

  copyMessage(){
    const selBox = document.createElement('textarea');
    selBox.style.position = 'fixed';
    selBox.style.left = '0';
    selBox.style.top = '0';
    selBox.style.opacity = '0';
    selBox.value = this.alphanumericCode;
    document.body.appendChild(selBox);
    selBox.focus();
    selBox.select();
    document.execCommand('copy');
    document.body.removeChild(selBox);
  }

}

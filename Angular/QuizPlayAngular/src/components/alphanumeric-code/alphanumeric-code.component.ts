import { ActivatedRoute, Router } from '@angular/router';
import { QuizService } from 'src/quiz.service';
import { Component, OnInit } from '@angular/core';
import { Clipboard } from '@angular/cdk/clipboard';

@Component({
  selector: 'app-alphanumeric-code',
  templateUrl: './alphanumeric-code.component.html',
  styleUrls: ['./alphanumeric-code.component.css']
})
export class AlphanumericCodeComponent implements OnInit {

  alphanumericCode: string = '';

  constructor(public http: QuizService, public route: ActivatedRoute, public clipboard: Clipboard) { }

    ngOnInit() {
      this.alphanumericCode = this.route.snapshot.paramMap.get('code');
  }
}

import { QuizService } from './services/Quiz.service';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'QuizPlayAngular';

  ngOnInit(): void {}

  constructor(
    private translate: TranslateService,
    public service: QuizService
  ) {
    if (localStorage.getItem('language')) {
      translate.setDefaultLang(localStorage.getItem('language'));
    } else {
      translate.setDefaultLang('fr');
    }
  }
}

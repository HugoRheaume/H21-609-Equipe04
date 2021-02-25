import { AngularFireAuth } from '@angular/fire/auth';
import { QuizService } from 'src/app/services/Quiz.service';
import { Component, OnInit } from '@angular/core';
import * as firebase from 'firebase';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css'],
})
export class NavigationComponent implements OnInit {
  public isMenuOpen: boolean = false;
  constructor(
    private translate: TranslateService,
    public service: QuizService,
    public auth: AngularFireAuth
  ) {}
  ngOnInit(): void {}
  logout() {
    this.auth.signOut().then((res) => {
      this.service.logout();
    });
  }

  getPicture(): string {
    return localStorage.getItem('picture');
  }
  getUsername(): string {
    return localStorage.getItem('name');
  }
  changeLanguage() {
    if (localStorage.getItem('language') == 'fr') {
      this.translate.use('en');
      localStorage.setItem('language', 'en');
    } else {
      this.translate.use('fr');
      localStorage.setItem('language', 'fr');
    }
  }
}

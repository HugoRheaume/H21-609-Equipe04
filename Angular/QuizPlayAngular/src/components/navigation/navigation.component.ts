import { AngularFireAuth } from '@angular/fire/auth';
import { QuizService } from 'src/app/services/Quiz.service';
import { Component, OnInit } from '@angular/core';
import * as firebase from 'firebase';




@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {
public isMenuOpen: boolean = false;
  constructor(public service: QuizService, public auth: AngularFireAuth) { }
  private wasInside = false;
  ngOnInit(): void {
  }
  login() {
    this.auth.signInWithPopup(new firebase.default.auth.GoogleAuthProvider()).then((res) => {
      this.service.login("\"" + res.user['za'] + "\"");
    });

  }
  logout() {
    this.auth.signOut().then((res) => {
      this.service.logout();
    })
  }


  getPicture(): string{
    return localStorage.getItem('picture');
  }
  getUsername(): string
  {
    return localStorage.getItem('name');
  }


}

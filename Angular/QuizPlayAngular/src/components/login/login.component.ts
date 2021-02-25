import { QuizService } from 'src/app/services/Quiz.service';
import { Component, OnInit } from '@angular/core'
import { AngularFireAuth } from '@angular/fire/auth'
import * as firebase from 'firebase/app';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  return: string = '';

  constructor(
    public afAuth: AngularFireAuth,
    public http: QuizService,
    private route: ActivatedRoute
  ) { }
  ngOnInit(): void {
    this.route.queryParams
      .subscribe(params => this.return = params['returnUrl'] || '/list');
  }

  login() {
    this.afAuth.signInWithPopup(new firebase.default.auth.GoogleAuthProvider()).then((res) => {
      this.http.login("\"" + res.user['za'] + "\"", this.return);
    });
  }
  logout() {
    this.afAuth.signOut().then((res) => {
      this.http.logout();
    })
  }

}


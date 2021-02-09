import { AuthService } from './../../services/auth.service';
import { QuizService } from 'src/app/services/Quiz.service';
import { Component } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/auth';
import * as firebase from 'firebase/app';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
	constructor(
		public afAuth: AngularFireAuth,
		public authService: AuthService
	) {}

	login() {
		this.afAuth
			.signInWithPopup(new firebase.default.auth.GoogleAuthProvider())
			.then(res => {
				this.authService.login('"' + res.user['za'] + '"');
			});
	}
	logout() {
		this.afAuth.signOut().then(res => {
			this.authService.logout();
		});
	}
}

import { Router } from '@angular/router';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserDTO } from 'src/models/userDTO';

@Injectable({
	providedIn: 'root',
})
export class AuthService {
	constructor(public http: HttpClient, public router: Router) {}

	public login(firebaseToken: string) {
		const httpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
		};

		this.http
			.post<UserDTO>(
				environment.backend.baseURL + '/auth/login',
				firebaseToken,
				httpOptions
			)
			.subscribe(response => {
				localStorage.setItem('token', response.token);
				localStorage.setItem('name', response.name);
				localStorage.setItem('email', response.email);
				localStorage.setItem('picture', response.picture);

				this.router.navigate(['/list']);
			});
	}

	public logout() {
		const httpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
		};

		this.http
			.get<any>(environment.backend.baseURL + '/auth/logout', httpOptions)
			.subscribe(response => {
				localStorage.setItem('token', '');
				localStorage.setItem('name', '');
				localStorage.setItem('email', '');
				localStorage.setItem('picture', '');
			});
	}
}

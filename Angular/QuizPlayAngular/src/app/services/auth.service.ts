import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(public http: HttpClient) { }
  /**
   * name
   */
  async isAuthenticated() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      }),
    };

    let response = this.http.get<boolean>(environment.backend.baseURL + '/auth/CheckAuthorize', httpOptions).toPromise();

    return await response.then(res => { return res.valueOf() }).catch(error => {
      if (error.status == 401)
        return false;
    });;

  }

}

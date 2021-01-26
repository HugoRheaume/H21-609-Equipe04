import { environment } from './environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HelloWorldObj } from './models/HelloWorldObj';

@Injectable({
  providedIn: 'root'
})
export class QuizService {

constructor(public http: HttpClient) { }

public getBogusObject(): Observable<HelloWorldObj>{
  const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    })
  }

  return this.http.get<HelloWorldObj>(environment.backend.baseURL + '/HelloWorld', httpOptions);
}

}

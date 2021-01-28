import { environment } from './environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HelloWorldObj } from './models/HelloWorldObj';
import { Question } from './models/question';

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

public addQuestion(pQuestion: Question): Observable<any>{
  const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    })
  }
  return this.http.post<Question>(environment.backend.baseURL + "/question/add", pQuestion, httpOptions);
}

}

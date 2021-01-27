import { environment } from './environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuizRequest } from './models/QuizRequest';
import { QuizResponse } from './models/QuizResponse';

@Injectable({
  providedIn: 'root'
})
export class QuizService {

constructor(public http: HttpClient) { }

public createQuiz(quiz: QuizRequest): void{
  const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    })}
  this.http.post<any>(environment.backend.baseURL+"/", quiz, httpOptions).subscribe(r => {
    console.log(r);
  });
}

}

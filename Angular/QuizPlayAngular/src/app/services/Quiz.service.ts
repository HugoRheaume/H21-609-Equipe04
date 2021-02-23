import { QuizModifyDTO } from '../models/QuizModifyDTO';
import { Router } from '@angular/router';
import { UserDTO } from '../models/userDTO';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { QuizRequest } from '../models/QuizRequest';
import { QuizResponse } from '../models/QuizResponse';
import { Question } from '../models/question';

@Injectable({
  providedIn: 'root',
})
export class QuizService {
  constructor(public http: HttpClient, public router: Router) { }

  public currentQuestions: Question[] = [];
  public currentQuiz: QuizResponse;

  public createQuiz(quiz: QuizRequest): Observable<QuizResponse> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      }),
    };

    return this.http
      .post<any>(environment.backend.baseURL + '/Quiz/Create', quiz, {
        headers: httpOptions.headers,
        observe: 'response',
      })
      .pipe(
        map(r => {
          if (r.status == 201) return r.body as QuizResponse;
          else if (r.status == 202) {
            return new QuizResponse(
              null,
              null,
              null,
              null,
              null,
              null,
              true,
              false,
              null,
              null
            );
          } else return null;
        })
      );
  }

  public addQuestion(pQuestion: any) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      }),
    };

    if (this.currentQuiz) pQuestion.quizId = this.currentQuiz.id;

    this.http
      .post<Question>(
        environment.backend.baseURL + '/question/add',
        pQuestion,
        httpOptions
      )
      .subscribe(response => {
        this.currentQuestions.push(response as Question);
      }, (error) => {
        console.error("from AddQuestion");
        console.error(error);

        //ouvre un popup ou une indication qu' il y a eu une erreur.
      });
  }

  deleteQuestion(questionId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      }),
    };

    this.http
      .get<Question[]>(
        environment.backend.baseURL + '/question/delete/' + questionId,
        httpOptions
      )
      .subscribe(response => {
        this.currentQuestions = response;
      });
  }
  getQuiz(quizShareCode: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + localStorage.getItem('token'),
      }),
    };

    this.http
      .get<QuizResponse>(
        environment.backend.baseURL +
        `/quiz/GetQuizByCode?code=${quizShareCode}`,
        httpOptions
      )
      .subscribe(response => {
        this.currentQuiz = response;
      });
  }
  getQuestionFromQuiz(quizShareCode: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };

    this.http
      .get<Question[]>(
        environment.backend.baseURL +
        `/question/GetQuizQuestionsFromShareCode/${quizShareCode}`,
        httpOptions
      )
      .subscribe(response => {
        this.currentQuestions = response;
        this.currentQuestions.sort((a, b) =>
          a.quizIndex > b.quizIndex ? 1 : -1
        );
      });
  }

  public getAlphanumericCode(): Observable<string> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return this.http.get<any>(
      environment.backend.baseURL + '/Quiz/GenerateAlphanumeric',
      httpOptions
    );
  }
  public getQuizList(): Observable<QuizResponse[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      }),
    };
    return this.http
      .get<QuizResponse[]>(
        environment.backend.baseURL + '/Quiz/GetQuizFromUser',
        httpOptions
      )
      .pipe(
        map(res => {
          return res;
        })
      );
  }
  public deleteQuiz(quiz: QuizResponse): Observable<boolean> {
    return this.http
      .get<boolean>(environment.backend.baseURL + `/Quiz/DeleteQuiz/${quiz.id}`)
      .pipe(
        map(
          r => {
            return true;
          },
          e => {
            return false;
          }
        )
      );
  }
  public updateQuizIndex() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      }),
    };
    this.http
      .post<boolean>(
        environment.backend.baseURL + '/question/updatequizindex',
        this.currentQuestions,
        httpOptions
      )
      .subscribe(response => {
        response;
      });
  }

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
        this.router.navigate(['/']);
      });
  }


  public modifyQuiz(modifiedQuiz: QuizResponse) {
    let quizToSend = this.toModifyDTO(modifiedQuiz);
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      }),
    };

    this.http.post<any>(environment.backend.baseURL + '/quiz/modifyquiz', quizToSend, httpOptions).subscribe(response => {
    })
  }

  //Ça c'est ici pcq si on le met direct dans la classe QuizResponse, c'est tellement bien fait que sa marche pas :)
  public toModifyDTO(q: QuizResponse): QuizModifyDTO {
    let quizToExport = new QuizModifyDTO();
    quizToExport.id = q.id;
    quizToExport.title = q.title;
    quizToExport.isPublic = q.isPublic;
    quizToExport.description = q.description;
    return quizToExport;
  }
}

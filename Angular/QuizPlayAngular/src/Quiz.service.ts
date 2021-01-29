import { environment } from './environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { QuizRequest } from './models/QuizRequest';
import { QuizResponse } from './models/QuizResponse';
import { Question, QuestionCreateDTO } from './models/question';

@Injectable({
	providedIn: 'root',
})
export class QuizService {
	constructor(public http: HttpClient) {}

	public createQuiz(quiz: QuizRequest): Observable<QuizResponse> {
		return this.http
			.post<any>(environment.backend.baseURL + '/Quiz/Create', quiz, {
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

	public addQuestion(pQuestion: QuestionCreateDTO): Observable<any> {
		const httpOptions = {
			headers: new HttpHeaders({
				'Content-Type': 'application/json',
			}),
		};
		return this.http.post<Question>(
			environment.backend.baseURL + '/question/add',
			pQuestion,
			httpOptions
		);
	}

	public getQuizList(): Observable<QuizResponse[]> {
		return this.http
			.get<QuizResponse[]>(
				environment.backend.baseURL + '/Quiz/GetQuizFromUser'
			)
			.pipe(
				map(res => {
					return res;
				})
			);
	}
  }

  public getAlphanumericCode(): Observable<string>{
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      })
    }

    return this.http.get<any>(environment.backend.baseURL+ '/Quiz/GenerateAlphanumeric', httpOptions);
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
}

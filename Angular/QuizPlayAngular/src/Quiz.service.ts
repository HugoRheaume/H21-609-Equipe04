import { environment } from './environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { QuizRequest } from './models/QuizRequest';
import { QuizResponse } from './models/QuizResponse';

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
						return new QuizResponse(null, null, null, null, null, null, true);
					} else if (r.status == 200)
						return new QuizResponse(
							null,
							null,
							null,
							null,
							null,
							null,
							false,
							true
						);
					else return null;
				})
			);
	}
}

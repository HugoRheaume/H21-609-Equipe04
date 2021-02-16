import { QuestionType, QuestionCreateTrueFalseDTO, QuestionTrueOrFalse, QuestionMultipleChoice } from './models/question';
import { Question } from 'src/models/question';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from './environments/environment';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {

  constructor(public http: HttpClient) { }

  /// Mettre la question concernée en paramètre et mettre les autres paramètres à null.
  public modifyQuestion(modifiedTrueFalse?: QuestionTrueOrFalse, modifiedMultipleChoice?: QuestionMultipleChoice) {
    if ((modifiedTrueFalse == null && modifiedMultipleChoice == null) || (modifiedTrueFalse != null && modifiedMultipleChoice != null)) return;

    let questionToExport = modifiedTrueFalse == null ? modifiedMultipleChoice as Question : modifiedTrueFalse as Question;

    console.log(questionToExport);
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      }),
    };

    this.http.post<any>(environment.backend.baseURL + '/question/modifyquestion', questionToExport, httpOptions).subscribe(response => {
      console.log(response);
    })
  }
}

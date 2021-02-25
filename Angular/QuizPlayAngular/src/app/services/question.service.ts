import { QuestionType, QuestionCreateTrueFalseDTO, QuestionTrueOrFalse, QuestionMultipleChoice, QuestionAssociation } from 'src/app/models/question';
import { Question } from 'src/app/models/question';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {

  constructor(public http: HttpClient) { }

  /// Mettre la question concernée en paramètre et mettre les autres paramètres à null.
  public modifyQuestion(modifiedTrueFalse?: QuestionTrueOrFalse, modifiedMultipleChoice?: QuestionMultipleChoice, modifiedAssociation?: QuestionAssociation) {
    let questionToExport = modifiedTrueFalse == null ? modifiedMultipleChoice == null ? modifiedAssociation as QuestionAssociation : modifiedMultipleChoice as QuestionMultipleChoice : modifiedTrueFalse as QuestionTrueOrFalse;


    switch (questionToExport.questionType) {
      case QuestionType.MultipleChoices:
        break;
      case QuestionType.TrueFalse:
        questionToExport.questionTrueFalse.answer = modifiedTrueFalse.answer;
        break;
      case QuestionType.Association:
        break;
    }
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

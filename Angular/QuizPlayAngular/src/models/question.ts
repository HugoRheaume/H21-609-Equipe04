export abstract class Question {
  public id: number;
  public quizId: number;
  public label: string;
  public timeLimit: number;
  public questionType: QuestionType;
  public answer;

  public toDTO(): QuestionCreateDTO {
    let questionToExport = new QuestionCreateDTO();
    let questionTrueFalse = new QuestionTrueOrFalse();
    questionTrueFalse.answer = this.answer;

    questionToExport.label = this.label;
    questionToExport.QuestionType = this.questionType;
    questionToExport.TimeLimit = this.timeLimit;
    questionToExport.QuestionTrueFalse = questionTrueFalse;
    questionToExport.quizId = this.quizId;

    return questionToExport;
  }
}

export class QuestionTrueOrFalse extends Question {
  constructor() {
    super();
    this.answer = new Boolean();
    this.questionType = QuestionType.TrueFalse;
  }
}

export enum QuestionType {
  'TrueFalse' = 1,
  'MultipleChoices' = 2,
  'Association' = 3,
  'Image' = 4
}

export class QuestionCreateDTO {
  public label: string;
  public QuestionType: QuestionType;
  public TimeLimit: number;
  public QuestionTrueFalse: QuestionTrueOrFalse;
  public quizId: number;
}

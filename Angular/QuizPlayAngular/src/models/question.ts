import { QuestionChoice } from './questionChoice';
export abstract class Question {
  public id: number;
  public quizId: number;
  public label: string;
  public timeLimit: number;
  public questionType: QuestionType;
  public answer;
  public questionChoices: QuestionChoice[];

  public toDTO(): QuestionCreateDTO {
    let questionToExport = new QuestionCreateDTO();
    let questionTrueFalse = new QuestionTrueOrFalse();
    questionTrueFalse.answer = this.answer;

    questionToExport.label = this.label;
    questionToExport.QuestionType = this.questionType;
    questionToExport.TimeLimit = this.timeLimit;
    questionToExport.QuestionTrueFalse = this.questionType == QuestionType.TrueFalse ? questionTrueFalse : null;
    questionToExport.QuestionMultipleChoice = this.questionType == QuestionType.MultipleChoices ?  this.questionChoices : null;

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

export class QuestionMultipleChoice extends Question{
  constructor() {
    super();
    this.answer = null;
    this.questionType = QuestionType.MultipleChoices;
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
  public QuestionMultipleChoice: QuestionChoice[];
  public quizId: number;
}

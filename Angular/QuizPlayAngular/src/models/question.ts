import { QuestionChoice } from './questionChoice';
export abstract class Question {
  public id: number;
  public quizId: number;
  public label: string;
  public timeLimit: number;
  public questionType: QuestionType;
}

export class QuestionTrueOrFalse extends Question {
  public answer: boolean;
  constructor() {
    super();
    this.questionType = QuestionType.TrueFalse;
  }

  public toTrueOrFalseDTO(): QuestionCreateTrueFalseDTO{
    let questionToExport = new QuestionCreateTrueFalseDTO();
    let questionTrueFalse = new QuestionTrueOrFalse();
    questionTrueFalse.answer = this.answer;

    questionToExport.label = this.label;
    questionToExport.QuestionType = this.questionType;
    questionToExport.TimeLimit = this.timeLimit;
    questionToExport.quizId = this.quizId;
    questionToExport.QuestionTrueFalse = questionTrueFalse;

    return questionToExport;
  }
}

export class QuestionMultipleChoice extends Question{
  public needsAllAnswers: boolean;
  public questionChoices: QuestionChoice[];
  constructor() {
    super();
    this.questionChoices = [];
    this.needsAllAnswers = true;
    this.questionType = QuestionType.MultipleChoices;
  }

  public toMultipleChoiceDTO(): QuestionCreateMultipleChoiceDTO {
    let questionToExport = new QuestionCreateMultipleChoiceDTO();

    questionToExport.label = this.label;
    questionToExport.QuestionType = this.questionType;
    questionToExport.TimeLimit = this.timeLimit;
    questionToExport.NeedsAllAnswers = this.needsAllAnswers;
    questionToExport.QuestionMultipleChoice = this.questionChoices;

    return questionToExport;
  }
}

export enum QuestionType {
  'TrueFalse' = 1,
  'MultipleChoices' = 2,
  'Association' = 3,
  'Image' = 4
}


export class QuestionCreateTrueFalseDTO {
  public label: string;
  public QuestionType: QuestionType;
  public TimeLimit: number;
  public QuestionTrueFalse: QuestionTrueOrFalse;
}

export class QuestionCreateMultipleChoiceDTO {
  public label: string;
  public QuestionType: QuestionType;
  public TimeLimit: number;
  public QuestionMultipleChoice: QuestionChoice[];
  public quizId: number;
  public NeedsAllAnswers: boolean;
}

import { QuestionAsso } from 'src/models/questionAsso';
import { QuestionChoice } from './questionChoice';
export class Question {
  public id: number;
  public quizId: number;
  public label: string;
  public timeLimit: number;
  public questionType: QuestionType;
  public quizIndex: number;
  public questionTrueFalse: QuestionTrueOrFalse;
  public questionMultipleChoice: QuestionChoice[];
}

export class QuestionTrueOrFalse extends Question {
  public answer: boolean;
  constructor() {
    super();
    this.questionType = QuestionType.TrueFalse;
  }

  public toTrueOrFalseDTO(): QuestionCreateTrueFalseDTO {
    let questionToExport = new QuestionCreateTrueFalseDTO();
    let questionTrueFalse = new QuestionTrueOrFalse();
    questionTrueFalse.answer = this.answer;
    questionToExport.quizId = this.quizId;
    questionToExport.label = this.label;
    questionToExport.QuestionType = this.questionType;
    questionToExport.TimeLimit = this.timeLimit;
    questionToExport.QuestionTrueFalse = questionTrueFalse;

    return questionToExport;
  }
}

export class QuestionMultipleChoice extends Question {
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
    questionToExport.quizId = this.quizId;

    return questionToExport;
  }
}

export class QuestionAssociation extends Question {
  public categories: string[];
  public questionAsso: QuestionAsso[];
  constructor() {
    super();
    this.questionAsso = [];
    this.categories = [];
    this.questionType = QuestionType.Association;
  }

  public toAssociationDTO(): QuestionCreateAssociationDTO {
    let questionToExport = new QuestionCreateAssociationDTO();

    questionToExport.label = this.label;
    questionToExport.QuestionType = this.questionType;
    questionToExport.TimeLimit = this.timeLimit;
    questionToExport.Categories = this.categories;
    questionToExport.QuestionAssociation = this.questionAsso;
    questionToExport.quizId = this.quizId;

    return questionToExport;
  }
}

export enum QuestionType {
  'TrueFalse' = 1,
  'MultipleChoices' = 2,
  'Association' = 3,
  // 'Image' = 4
}


export class QuestionCreateTrueFalseDTO {
  public label: string;
  public QuestionType: QuestionType;
  public TimeLimit: number;
  public QuestionTrueFalse: QuestionTrueOrFalse;
  public quizId: number;
}

export class QuestionCreateMultipleChoiceDTO {
  public label: string;
  public QuestionType: QuestionType;
  public TimeLimit: number;
  public QuestionMultipleChoice: QuestionChoice[];
  public quizId: number;
  public NeedsAllAnswers: boolean;
}

export class QuestionCreateAssociationDTO {
  public label: string;
  public QuestionType: QuestionType;
  public TimeLimit: number;
  public QuestionAssociation: QuestionAsso[];
  public quizId: number;
  public Categories: string[];
}

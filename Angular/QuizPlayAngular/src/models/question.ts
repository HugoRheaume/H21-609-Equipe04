export abstract class Question {
  public label: string;
  public timeLimit: number;
  public questionType: QuestionType;
  public answer;
}

export class QuestionTrueOrFalse extends Question{
  constructor(){
    super();
    this.answer = new Boolean();
    this.questionType = QuestionType.TrueFalse;
  }
}

export enum QuestionType{
  'TrueFalse' = 0,
  'MultipleChoices' = 1,
  'Association' = 2,
  'Image' = 3
}

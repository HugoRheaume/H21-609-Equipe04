export class QuizRequest {
  constructor(
    public title: string,
    public description: string,
    public isPublic: boolean,
    public confirm: boolean = false) { }
}

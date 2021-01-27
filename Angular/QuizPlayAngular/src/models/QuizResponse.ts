export class QuizResponse {
    constructor(
        public id: number, 
        public author: string, 
        public title: string, 
        public isPublic: boolean, 
        public description: string, 
        public shareCode: string,
        public toConfirm: boolean = false,
        public errorMessage: boolean = false) {}
}
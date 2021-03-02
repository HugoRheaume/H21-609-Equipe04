import { Question } from './question';

export enum CommandName {
  CreateRoom = 'Room.Create',
  JoinRoom = 'Room.Join',
  RoomSate = 'Room.UserState',
  RoomLeave = 'Room.Leave',
  RoomDestroy = 'Room.Destroy',
  LogMessage = 'Log.Message',
  QuizBegin = 'Quiz.Begin',
  QuizNext = 'Quiz.Next',
  QuizScoreboard = 'Quiz.Scoreboard',
  QuizQuestionResult = 'Quiz.QuestionResult',
  QuizForceSkip = 'Quiz.ForceSkip',
}
export class CreateRoomWS {
  public owner: string;
  public shareCode: string;
  public token: string;
  public quizShareCode: string;
  private readonly commandName: string = CommandName.CreateRoom;
}
export class DeleteRoomWS {
  public token: string;
  public shareCode: string;
  private readonly commandName: string = CommandName.RoomDestroy;
}
export class BeginQuizWS {
  public token: string;
  public shareCode: string;
  private readonly commandName: string = CommandName.QuizBegin;
}
export class NextQuestionWS {
  public token: string;
  public questionIndex: number;
  public question: Question;
  private readonly commandName: string = CommandName.QuizNext;
}
export class ResultQuestionWS {
  public token: string;
  public questionIndex: number;
  private readonly commandName: string = CommandName.QuizQuestionResult;
}

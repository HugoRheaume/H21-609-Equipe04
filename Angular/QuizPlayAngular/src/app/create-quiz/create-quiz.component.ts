import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { QuizRequest } from 'src/models/QuizRequest';
import { QuizService } from 'src/Quiz.service';

@Component({
  selector: 'app-create-quiz',
  templateUrl: './create-quiz.component.html',
  styleUrls: ['./create-quiz.component.css']
})
export class CreateQuizComponent implements OnInit {
  title: string = "";
  desc:string = "";
  isPublic = false;
  errMessage:string = ""

  constructor(public http: QuizService, public dialog: MatDialog)  {}

  ngOnInit(): void {
  }

  public create(): void {
    this.http.createQuiz(new QuizRequest(this.title, this.desc, this.isPublic)).subscribe(r => {
      console.log(r);
      if (r.toConfirm)
        this.toConfirm();
      else if (r.errorMessage)
        this.errMessage = "Title is too short";
      else if (r == null)
        this.errMessage = "An unexpected error occured";
      else
        console.log("nice!");
    });
  }

  public toConfirm(): void {
    const dialogRef = this.dialog.open(CreateQuizConfirmDialog, {
      width: '500px',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
      if (result)
        this.http.createQuiz(new QuizRequest(this.title, this.desc, this.isPublic, true)).subscribe(r => console.log(r));
    });
  }
}

@Component({
  selector: 'dialog-create-quiz-confirm',
  templateUrl: './create-quiz-confirm-dialog.html'
})
export class CreateQuizConfirmDialog {
  constructor(public dialogRef: MatDialogRef<CreateQuizConfirmDialog>) {}
}

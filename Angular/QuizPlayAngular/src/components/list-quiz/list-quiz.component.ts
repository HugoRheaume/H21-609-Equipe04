import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { QuizResponse } from '../../app/models/QuizResponse';
import { QuizService } from 'src/app/services/Quiz.service';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { DecimalPipe } from '@angular/common';
@Component({
  selector: 'app-list-quiz',
  templateUrl: './list-quiz.component.html',
  styleUrls: ['./list-quiz.component.scss'],
})
export class ListQuizComponent implements OnInit {
  constructor(
    public service: QuizService,
    public dialog: MatDialog,
    public router: Router,
    public translate: TranslateService
  ) {}

  dataSource = new MatTableDataSource();
  displayedColumns: string[] = [
    'title',
    'numberOfQuestions',
    'date',
    'actions',
  ];
  decimalPipe = new DecimalPipe(navigator.language);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  ngOnInit(): void {
    this.loadList();
  }
  loadList() {
    this.service.getQuizList().subscribe((r) => {
      this.dataSource = new MatTableDataSource(r);
      this.dataSource.sortingDataAccessor = (
        data: any,
        sortHeaderId: string
      ): string => {
        if (typeof data[sortHeaderId] === 'string') {
          return data[sortHeaderId].toLocaleLowerCase();
        }
        return data[sortHeaderId];
      };
      this.paginator._intl.itemsPerPageLabel = this.translate.instant(
        'app.listQuiz.quizPerPage'
      );
      this.paginator._intl.getRangeLabel = (
        page: number,
        pageSize: number,
        length: number
      ) => {
        const start = page * pageSize + 1;
        const end = (page + 1) * pageSize;
        return `${start} - ${
          end > length ? length : end
        } de ${this.decimalPipe.transform(length)}`;
      };
      this.dataSource.paginator = this.paginator;

      this.dataSource.sort = this.sort;
    });
  }
  deleteQuiz(quiz: QuizResponse) {
    this.service.deleteQuiz(quiz).subscribe((r) => {
      this.loadList();
    });
  }

  // Dialog
  openDialog(event: Event, quiz: QuizResponse): void {
    event.stopPropagation();
    const dialogRef = this.dialog.open(DeleteQuizDialog, {
      width: '600px',
      height: 'auto',
      data: quiz,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result == true) {
        this.deleteQuiz(quiz);
      }
    });
  }

  goToQuizDetails(quizShareCode: Number) {
    this.router.navigate([`/quiz/${quizShareCode}`]);
  }

  goLive(event: Event, quizId: Number) {
    event.stopPropagation();
    this.router.navigate([`/live/${quizId}`]);
  }
}

@Component({
  selector: 'delete-quiz-dialog',
  templateUrl: 'delete-quiz-dialog.html',
})
export class DeleteQuizDialog {
  constructor(
    public dialogRef: MatDialogRef<DeleteQuizDialog>,
    @Inject(MAT_DIALOG_DATA) public data: QuizResponse
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}

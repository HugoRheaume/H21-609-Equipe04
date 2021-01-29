import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { QuizRequest } from 'src/models/QuizRequest';
import { QuizService } from 'src/quiz.service';

@Component({
	selector: 'app-create-quiz',
	templateUrl: './create-quiz.component.html',
	styleUrls: ['./create-quiz.component.css'],
})
export class CreateQuizComponent implements OnInit {
	title: string = '';
	desc: string = '';
	isPublic = false;
	errMessage: string = '';
	titleValidStyle = '';
	alphanumericCode: string = '';
	constructor(
		public http: QuizService,
		public dialog: MatDialog,
		public router: Router
	) {}

	ngOnInit(): void {}

	public create(): void {
		this.http
			.createQuiz(new QuizRequest(this.title, this.desc, this.isPublic))
			.subscribe(
				r => {
					if (r.toConfirm) this.toConfirm();
					else if (r == null) this.errMessage = 'An unexpected error occured';
					else {
						this.router.navigate(['/CreateQuiz/' + r.shareCode]);
						this.errMessage = '';
					}
				},
				e => {
					this.errMessage = 'Title is too short';
					this.titleValidStyle = 'mat-form-field-invalid';
				} else if (r == null) this.errMessage = 'An unexpected error occured';
				else {
					this.router.navigate(['/CreateQuiz/'+r.shareCode])
					this.errMessage = '';
					this.http.currentQuiz = r;
					console.log(this.http.currentQuiz);
				}
			);
	}

	public toConfirm(): void {
		const dialogRef = this.dialog.open(CreateQuizConfirmDialog, {
			width: '500px',
		});

		dialogRef.afterClosed().subscribe(result => {
			if (result)
				this.http
					.createQuiz(
						new QuizRequest(this.title, this.desc, this.isPublic, true)
					)
					.subscribe(r =>{
                        this.router.navigate(['/CreateQuiz/'+r.shareCode])
          
						this.http.currentQuiz = r;
						console.log(this.http.currentQuiz);
					});
		});
	}

	public updateInputState(event: string) {
		if (event.length >= 4) this.titleValidStyle = '';
		else this.titleValidStyle = 'mat-form-field-invalid';
	}
}

@Component({
	selector: 'dialog-create-quiz-confirm',
	templateUrl: './create-quiz-confirm-dialog.html',
})
export class CreateQuizConfirmDialog {
	constructor(public dialogRef: MatDialogRef<CreateQuizConfirmDialog>) {}
}

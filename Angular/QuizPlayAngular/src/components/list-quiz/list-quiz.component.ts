import { QuizService } from 'src/quiz.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
@Component({
	selector: 'app-list-quiz',
	templateUrl: './list-quiz.component.html',
	styleUrls: ['./list-quiz.component.scss'],
})
export class ListQuizComponent implements OnInit {
	constructor(public service: QuizService) {}

	dataSource = new MatTableDataSource();
	displayedColumns: string[] = ['name', 'nbrQuestions', 'date'];

	@ViewChild(MatPaginator) paginator: MatPaginator;

	ngOnInit(): void {
		this.service.getQuizList().subscribe(r => {
			this.dataSource = new MatTableDataSource(r);
			this.dataSource.paginator = this.paginator;
		});
	}
}

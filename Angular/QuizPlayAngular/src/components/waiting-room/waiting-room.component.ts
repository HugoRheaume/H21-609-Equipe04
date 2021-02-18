import { QuizService } from 'src/app/services/Quiz.service';
import { Subject } from 'rxjs';
import { Message } from '../../app/models/message';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { WebSocketService } from 'src/app/services/web-socket.service';
import { ActivatedRoute } from '@angular/router';

@Component({
	selector: 'app-waiting-room',
	templateUrl: './waiting-room.component.html',
	styleUrls: ['./waiting-room.component.scss'],
})
export class WaitingRoomComponent implements OnInit, OnDestroy {
	public messages$ = new Subject<string[]>();
	//public users: string[];
	public usersFormated: string[] = [];
	public iterator: Array<number>;
	public shareCode: string;

	public messages: Message[] = [];
	public dataSource: Message[];
	public currentInput: string;
	displayedColumns = ['users', 'messages'];
	constructor(
		public service: WebSocketService,
		public quizService: QuizService,
		private route: ActivatedRoute,
	) {}

	ngOnInit() {
		var quizShareCode: string = this.route.snapshot.paramMap.get('quizShareCode');
		this.service.connect();
		this.service.logReceive$().subscribe(data => {
			this.messages$.next(data);
		});
		this.service.create(quizShareCode);
	}
	ngOnDestroy()
	{
		this.service.cancel();
	}
	cancel() {
		this.service.cancel();
	}
	addMessage($event) {
		this.messages.push(
			new Message(this.currentInput, 'User Test tres tres tres tres long')
		);
		this.dataSource = [...this.messages];
		this.currentInput = '';
	}

	starQuiz() {
		this.service.beginQuiz();
	}
}

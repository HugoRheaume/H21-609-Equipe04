import { QuizService } from '../../quiz.service';
import { Component, OnInit } from '@angular/core';
import { Question, QuestionTrueOrFalse } from 'src/models/question';
import { Router } from '@angular/router';
import {
	FormBuilder,
	FormControl,
	FormGroup,
	Validators,
} from '@angular/forms';

@Component({
	selector: 'app-create-trueorfalse-question',
	templateUrl: './create-trueorfalse-question.component.html',
	styleUrls: ['./create-trueorfalse-question.component.scss'],
})
export class CreateTrueOrFalseQuestion implements OnInit {
	public TrueFalse: FormGroup;
	constructor(
		public service: QuizService,
		public route: Router,
		private formBuilder: FormBuilder
	) {}

	ngOnInit() {
		this.TrueFalse = this.formBuilder.group({
			questionLabel: [
				'',
				[
					Validators.required,
					Validators.minLength(1),
					Validators.maxLength(250),
				],
			],
			questionAnswer: ['', [Validators.required]],
			questionTimeLimit: ['', [Validators.required, Validators.min(1)]],
			questionHasTimeLimit: [''],
		});
	}

	sumbit() {
		let questionTimeLimit = this.TrueFalse.get(
			'questionTimeLimit'
		) as FormControl;
		let questionHasTimeLimit = this.TrueFalse.get(
			'questionHasTimeLimit'
		) as FormControl;

		let question = new QuestionTrueOrFalse();

		if (this.checkForm) return;

		//The question doesn't have a time limit
		if (
			questionHasTimeLimit.value == null ||
			questionHasTimeLimit.value == false ||
			questionHasTimeLimit.value === ''
		)
			question.timeLimit = -1;
		else question.timeLimit = questionTimeLimit.value;

		question.answer = this.TrueFalse.get('questionAnswer').value;
		question.label = this.TrueFalse.get('questionLabel').value;

		this.TrueFalse.reset();

		this.service.addQuestion(question.toTrueOrFalseDTO());
		this.route.navigate['/'];
	}

	//Checks the form, return false if ok
	get checkForm(): boolean {
		let questionLabel = this.TrueFalse.get('questionLabel') as FormControl;
		let questionAnswer = this.TrueFalse.get('questionAnswer') as FormControl;
		let questionTimeLimit = this.TrueFalse.get(
			'questionTimeLimit'
		) as FormControl;
		let questionHasTimeLimit = this.TrueFalse.get(
			'questionHasTimeLimit'
		) as FormControl;

		if (this.TrueFalse.pristine) {
			// console.log('Form is pristine');
			// alert('The form is has not been modified.');
			return true;
		}
		//The user changed the label to nothing
		if (questionLabel.value === '' || questionLabel.value == null) {
			// console.log('Label value is nothing');
			// alert('The label\'s value is nothing.');
			return true;
		}
		if (questionLabel.value.length > 250) {
			// console.log('Label is too long');
			// alert('The label\'s value is too long.');
			return true;
		}
		//The user didn't answer
		if (questionAnswer.value === '' || questionAnswer.value == null) {
			// console.log('Answer is not answered');
			// alert('The answer has not been defined.');
			return true;
		}
		if (
			questionHasTimeLimit.value &&
			(questionTimeLimit.value === '' || questionTimeLimit.value == null)
		) {
			// console.log('Time limit not defined');
			// alert('The time limit has not defined.');
			return true;
		}
		if (
			questionHasTimeLimit.value !== '' &&
			questionHasTimeLimit.value != null &&
			questionHasTimeLimit.value != false &&
			questionTimeLimit.value < 1
		) {
			// console.log('Time limit less than 1');
			// alert('The time limit can't be less than 1.');
			return true;
		}

		return false;
	}

	//#region Error messages
	get labelErrorMessage(): string {
		const formField: FormControl = this.TrueFalse.get(
			'questionLabel'
		) as FormControl;
		return formField.hasError('required')
			? "L'énoncé est requis"
			: formField.hasError('maxlength')
			? 'Vous avez dépassé le nombre de caractères maximum'
			: formField.hasError('nowhitespaceerror')
			? ''
			: ''; // Default
	}

	get timeLimitErrorMessage(): string {
		const formField: FormControl = this.TrueFalse.get(
			'questionTimeLimit'
		) as FormControl;
		return formField.hasError('min')
			? 'La valeur minimale est 1'
			: formField.hasError('required')
			? 'Le temps limite est requis'
			: ''; // Default
	}
	//#endregion
}

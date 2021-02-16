import { QuestionMultipleChoice } from './../../models/question';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import {
	FormBuilder,
	FormControl,
	FormGroup,
	Validators,
} from '@angular/forms';
import { QuizService } from 'src/quiz.service';
import { QuestionChoice } from 'src/models/questionChoice';
@Component({
	selector: 'app-create-multiplechoices-question',
	templateUrl: './create-multiplechoices-question.component.html',
	styleUrls: ['./create-multiplechoices-question.component.scss'],
})
export class CreateMultiplechoicesQuestionComponent implements OnInit {
	public MultipleChoice: FormGroup;
	public choices: QuestionChoice[];
	public needsAllRightAnswers: boolean;
	constructor(
		public service: QuizService,
		public route: Router,
		private formBuilder: FormBuilder
	) {}

	ngOnInit() {
		this.MultipleChoice = this.formBuilder.group({
			questionLabel: [
				'',
				[
					Validators.required,
					Validators.minLength(1),
					Validators.maxLength(250),
				],
			],
			questionTimeLimit: ['', [Validators.required, Validators.min(1)]],
			questionHasTimeLimit: [''],
			questionHasOnlyOneAnswer: [''],
			questionAnswers: ['', Validators.required],
		});
		this.choices = [];
		this.needsAllRightAnswers = true;
	}

	//#region Error messages
	get labelErrorMessage(): string {
		const formField: FormControl = this.MultipleChoice.get(
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
		const formField: FormControl = this.MultipleChoice.get(
			'questionTimeLimit'
		) as FormControl;
		return formField.hasError('min')
			? 'La valeur minimale est 1'
			: formField.hasError('required')
			? 'Le temps limite est requis'
			: ''; // Default
	}
	//#endregion

	submit() {
		let questionTimeLimit = this.MultipleChoice.get(
			'questionTimeLimit'
		) as FormControl;
		let questionHasTimeLimit = this.MultipleChoice.get(
			'questionHasTimeLimit'
		) as FormControl;

		let question = new QuestionMultipleChoice();

		if (this.checkForm) return;

		//The question doesn't have a time limit
		if (
			questionHasTimeLimit.value == null ||
			questionHasTimeLimit.value == false ||
			questionHasTimeLimit.value === ''
		)
			question.timeLimit = -1;
		else question.timeLimit = questionTimeLimit.value;

		question.questionChoices = this.choices;
		question.needsAllAnswers = this.needsAllRightAnswers;
		question.label = this.MultipleChoice.get('questionLabel').value;

		this.discard();

		this.service.addQuestion(question.toMultipleChoiceDTO());
		this.route.navigate['/'];

		return;
	}

	discard() {
		this.MultipleChoice.reset();
		this.choices = [];
		this.needsAllRightAnswers = true;
	}

	addChoice() {
		let newChoice = new QuestionChoice();
		newChoice.choiceNumber = this.choices.length + 1;
		newChoice.answer = false;
		newChoice.statement = '';
		this.choices.push(newChoice);
	}

	removeChoice(i: number) {
		this.choices.splice(i - 1, 1);

		let iterator = 1;
		this.choices.forEach(choice => {
			choice.choiceNumber = iterator;
			iterator++;
		});
	}

	// Returns false if ok
	get checkForm(): boolean {
		let questionLabel = this.MultipleChoice.get('questionLabel') as FormControl;
		let questionTimeLimit = this.MultipleChoice.get(
			'questionTimeLimit'
		) as FormControl;
		let questionHasTimeLimit = this.MultipleChoice.get(
			'questionHasTimeLimit'
		) as FormControl;

		if (this.choices.length < 2) return true;
		if (this.MultipleChoice.pristine) return true;
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
		let oneChoiceEmpty = false;
		let noChoiceChecked = true;
		this.choices.forEach(element => {
			if (element.statement === '') oneChoiceEmpty = true;
			if (element.answer === true) noChoiceChecked = false;
		});
		if (oneChoiceEmpty) return true;
		if (noChoiceChecked) return true;

		return false;
	}

	get trueAnswers(): number {
		let amount = 0;
		this.choices.forEach(choice => {
			if (choice.answer) amount++;
		});
		return amount;
	}
}

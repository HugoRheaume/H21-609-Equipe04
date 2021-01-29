/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CreateTrueOrFalseQuestion } from './create-trueorfalse-question.component';

describe('CreateQuestionComponent', () => {
  let component: CreateTrueOrFalseQuestion;
  let fixture: ComponentFixture<CreateTrueOrFalseQuestion>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateTrueOrFalseQuestion ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateTrueOrFalseQuestion);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAssociationQuestionComponent } from './create-association-question.component';

describe('CreateAssociationQuestionComponent', () => {
  let component: CreateAssociationQuestionComponent;
  let fixture: ComponentFixture<CreateAssociationQuestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateAssociationQuestionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateAssociationQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

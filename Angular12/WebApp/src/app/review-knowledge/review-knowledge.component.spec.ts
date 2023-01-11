import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewKnowledgeComponent } from './review-knowledge.component';

describe('ReviewKnowledgeComponent', () => {
  let component: ReviewKnowledgeComponent;
  let fixture: ComponentFixture<ReviewKnowledgeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReviewKnowledgeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewKnowledgeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

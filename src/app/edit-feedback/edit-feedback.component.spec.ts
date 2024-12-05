import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedbackEditComponent } from './edit-feedback.component';

describe('EditFeedbackComponent', () => {
  let component: FeedbackEditComponent;
  let fixture: ComponentFixture<FeedbackEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeedbackEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FeedbackEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

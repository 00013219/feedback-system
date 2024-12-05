import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import {MatInputModule} from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { FeedbackService } from '../service/feedback.service';


@Component({
  selector: 'app-feedback',
  templateUrl: './feedback-create.component.html',
  styleUrls: ['./feedback-create.component.css'],
  standalone: true,
  imports: [
    MatInputModule,
    MatButtonModule,
    MatOptionModule,
    MatSelectModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    CommonModule
  ],
})
export class FeedbackCreateComponent {

  feedbackForm: FormGroup;

  feedbackTypes = [
    { value: 0, label: 'Complaint' },
    { value: 1, label: 'Suggestion' },
    { value: 2, label: 'Bug' },
    { value: 3, label: 'Praise' },
  ];
  statuses = [
    { value: 0, label: 'Open' },
    { value: 1, label: 'In Progress' },
    { value: 2, label: 'Resolved' },
    { value: 3, label: 'Closed' },
  ];

  constructor(
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private feedbackService: FeedbackService
  ) {
    this.feedbackForm = this.fb.group({
      feedbackType: ['', Validators.required],
      feedbackContent: [
        '',
        [Validators.required, Validators.maxLength(1000)],
      ],
      status: ['', Validators.required],
    });
  }


  onSubmit(): void {
    if (this.feedbackForm.valid) {
      const feedbackData = this.feedbackForm.value;

      this.feedbackService.submitFeedback(feedbackData).subscribe({
        next: () => {
          this.snackBar.open('Feedback submitted successfully!', 'Close', {
            duration: 3000,
          });
          this.feedbackForm.reset();
        },
        error: (err) => {
          console.error('Feedback submission failed', err);
          this.snackBar.open('Failed to submit feedback.', 'Close', {
            duration: 3000,
          });
        },
      });
    } else {
      this.snackBar.open('Please correct the form errors.', 'Close', {
        duration: 3000,
      });
    }
  }
}

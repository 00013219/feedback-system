import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import {MatInputModule} from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FeedbackService } from '../service/feedback.service';

@Component({
  selector: 'app-edit-feedback',
  templateUrl: './edit-feedback.component.html',
  styleUrl: './edit-feedback.component.css',
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
export class FeedbackEditComponent implements OnInit {
  feedbackID!: number;
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
    private snackBar: MatSnackBar,
    private fb: FormBuilder,
    private feedbackService: FeedbackService,
    private route: ActivatedRoute,

  ) {
    this.feedbackForm = this.fb.group({
      feedbackType: ['', Validators.required],
      feedbackContent: ['', [Validators.required, Validators.maxLength(1000)]],
      status: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.feedbackID = Number(this.route.snapshot.paramMap.get('id'));

    this.feedbackService.getFeedbackById(this.feedbackID).subscribe(
      (data) => {
        this.feedbackForm.patchValue({
          feedbackType: data.feedbackType,
          feedbackContent: data.feedbackContent,
          status: data.status,
        });
      },
      (error) => {
        console.error('Error fetching feedback details:', error);
      }
    );
  }

  onSubmit(): void {
    if (this.feedbackForm.valid) {
      const feedbackData = this.feedbackForm.value;

      this.feedbackService.updateFeedback(feedbackData, this.feedbackID).subscribe({
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
        }
      });
    } else {
      this.snackBar.open('Please correct the form errors.', 'Close', {
        duration: 3000,
      });
    }
  }
}




import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FeedbackService } from '../service/feedback.service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommentService } from '../service/comment.service';

@Component({
  selector: 'app-feedback-details',
  templateUrl: './feedback-details.component.html',
  styleUrls: ['./feedback-details.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatListModule,
    MatCardModule,
  ],
})
export class FeedbackDetailsComponent implements OnInit {
  feedbackDetails: any = null;
  comments: any[] = [];
  feedbackID!: number;
  commentForm: FormGroup;


  constructor(
    private feedbackService: FeedbackService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private commentService: CommentService
  ) {
    this.commentForm = this.fb.group({
      commentText: ['', [Validators.required, Validators.maxLength(500)]],
    });
  }

  getFeedbackTypeLabel(type: number): string {
    return this.feedbackTypes.find(t => t.value === type)?.label || 'Unknown';
  }

  getStatusLabel(status: number): string {
    return this.statuses.find(s => s.value === status)?.label || 'Unknown';
  }

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

  ngOnInit(): void {
    this.feedbackID = Number(this.route.snapshot.paramMap.get('id'));
    this.loadFeedbackDetails();
    this.loadComments();
  }

  loadFeedbackDetails(): void {
    this.feedbackService.getFeedbackById(this.feedbackID).subscribe({
      next: (response) => {
        this.feedbackDetails = response;
      },
      error: (err) => console.error('Failed to fetch feedback details', err),
    });
  }

  loadComments(): void {
    this.feedbackService.getCommentsByFeedbackId(this.feedbackID).subscribe({
      next: (response) => {
        this.comments = response;
      },
      error: (err) => console.error('Failed to fetch comments', err),
    });
  }

  submitComment(): void {
    if (this.commentForm.valid) {
      const newComment = {
        commentText: this.commentForm.value.commentText,
        feedbackID: this.feedbackID,
      };

      this.commentService.postComment(newComment).subscribe({
        next: (response) => {
          this.comments.push(response);
          this.commentForm.reset();
          this.snackBar.open('Comment sent successful.', 'Close', {
            duration: 3000,
          });
          this.loadComments();
        },
        error: (err: any) => {
          this.snackBar.open('Please try again.', 'Close', {
            duration: 3000,
          });
        },
      });
    }
  }
}

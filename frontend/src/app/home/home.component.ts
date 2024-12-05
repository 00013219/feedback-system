import { Component, OnInit } from '@angular/core';
import { FeedbackService } from '../service/feedback.service';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  imports: [CommonModule, MatTableModule, MatButtonModule, RouterLink, MatIconModule],
})
export class HomeComponent implements OnInit {
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

  displayedColumns: string[] = ['id', 'type', 'content', 'status', 'userName', 'actions'];
  feedbackItems: any[] = [];

  constructor(private feedbackService: FeedbackService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.loadFeedbackItems();
  }

  loadFeedbackItems(): void {
    this.feedbackService.getFeedbackItems().subscribe({
      next: (items) => (this.feedbackItems = items),
      error: (err) => console.error('Failed to load feedback items', err),
    });
  }

  getFeedbackTypeLabel(type: number): string {
    return this.feedbackTypes.find((t) => t.value === type)?.label || 'Unknown';
  }

  getStatusLabel(status: number): string {
    return this.statuses.find((s) => s.value === status)?.label || 'Unknown';
  }

  deleteFeedback(feedbackID: number): void {
    if (confirm('Are you sure you want to delete this feedback?')) {
      this.feedbackService.deleteFeedback(feedbackID).subscribe({
        next: () => {
          this.feedbackItems = this.feedbackItems.filter(item => item.feedbackID !== feedbackID);
          this.snackBar.open('Feedback deleted successfully.', 'Close', { duration: 3000 });
        },
        error: (err) => {
          console.error('Failed to delete feedback', err);
          this.snackBar.open('Failed to delete feedback. Please try again.', 'Close', { duration: 3000 });
        },
      });
    }
  }
}

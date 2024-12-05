import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../service/auth.service';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [FormsModule, MatInputModule, MatButtonModule],
})
export class RegisterComponent {
  registerRequest = {
    name: '',
    email: '',
    password: '',
  };

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  onSubmit(): void {
    this.authService.register(this.registerRequest).subscribe({
      next: () => {
        this.snackBar.open('Registration successful! Redirecting to login...', 'Close', {
          duration: 3000,
        });
        this.registerRequest = { name: '', email: '', password: '' };
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Registration failed', err);
        this.snackBar.open('Registration failed. Please try again.', 'Close', {
          duration: 3000,
        });
      },
    });
  }
}

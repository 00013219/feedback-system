import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../service/auth.service';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [FormsModule, MatInputModule, MatButtonModule],
})
export class LoginComponent {
  loginRequest = {
    email: '',
    password: '',
  };

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  onSubmit(): void {
    this.authService.login(this.loginRequest).subscribe({
      next: (response) => {
        localStorage.setItem('token', response.token);
        this.snackBar.open('Login successful!', 'Close', { duration: 3000 });
        this.router.navigate(['/home']);
      },
      error: (err) => {
        console.error('Login failed', err);
        this.snackBar.open('Login failed. Please check your credentials.', 'Close', {
          duration: 3000,
        });
      },
    });
  }
}

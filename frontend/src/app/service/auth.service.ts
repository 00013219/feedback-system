import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7214/api';

  constructor(private http: HttpClient) {}

  login(loginRequest: { email: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/Login`, loginRequest);
  }

  register(registerRequest: { name: string; email: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/Register`, registerRequest);
  }
}

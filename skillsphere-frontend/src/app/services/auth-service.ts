import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {jwtDecode} from 'jwt-decode';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly BASE_URL = 'https://localhost:7042/api/User';
  private readonly TOKEN_KEY = 'token';

  constructor(private readonly http: HttpClient,
    private router: Router
  ) {}

  signup(data: any): Observable<any> {
    return this.http.post(`${this.BASE_URL}`, data);
  }

  login(data: any): Observable<any> {
    return this.http.post(`${this.BASE_URL}/login`, data);
  }

  // Authentication helpers
  isAuthenticated(): boolean {
    return !! this.getToken();
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    this.router.navigate(['/']);
  }

  // Role decoding
  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      return decoded.role || null;
    } catch (error) {
      console.error('Invalid token', error);
      return null;
    }
  }

  isAdmin(): boolean {
    return this.getUserRole() === 'Admin';
  }

  isLearner(): boolean {
    return this.getUserRole() === 'Learner';
  }

  getUserName(): string | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const decoded: any = jwtDecode(token);
      return decoded.unique_name || null;
    } catch (error) {
      console.error('Invalid token', error);
      return null;
    }
  }
}

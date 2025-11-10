import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import environment from '../environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly BASE_URL = environment.BASE_URL + '/User';
  private readonly TOKEN_KEY = 'token';

  constructor(private readonly http: HttpClient) {}

  // 🔹 User registration and authentication
  signup(data: any): Observable<any> {
    return this.http.post(`${this.BASE_URL}/Registration`, data);
  }

  verifyOtp(data: { email: string; otpCode: string }): Observable<any> {
    return this.http.post(`${this.BASE_URL}/verify-otp`, data);
  }

  login(data: any): Observable<any> {
    return this.http.post(`${this.BASE_URL}/login`, data);
  }

  // 🔹 Token Management
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    window.location.href = '/';
  }

  // 🔹 Authentication Checks
  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  // 🔹 Decode JWT safely
  private decodeToken(): any | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('❌ Invalid token:', error);
      return null;
    }
  }

  // 🔹 Get user info from token
  getUserId(): number | null {
    const decoded = this.decodeToken();
    if (!decoded || !decoded['nameid']) return null;
    return Number(decoded['nameid']);
  }

  getUserName(): string | null {
    const decoded = this.decodeToken();
    return decoded?.unique_name || null;
  }

  getUserEmail(): string | null {
    const decoded = this.decodeToken();
    return decoded?.email || null;
  }

  getUserRole(): string | null {
    const decoded = this.decodeToken();
    return decoded?.role || null;
  }

  // 🔹 Role helpers
  isAdmin(): boolean {
    return this.getUserRole() === 'Admin';
  }

  isLearner(): boolean {
    return this.getUserRole() === 'Learner';
  }
}

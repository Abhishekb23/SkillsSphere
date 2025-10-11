import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly BASE_URL = 'https://localhost:7042/api/User';

  constructor(private readonly http: HttpClient) {}

  signup(data: any): Observable<any> {
    return this.http.post(`${this.BASE_URL}`, data);
  }

  login(data: any): Observable<any> {
    return this.http.post(`${this.BASE_URL}/login`, data);
  }
}

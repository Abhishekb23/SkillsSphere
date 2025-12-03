import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Observable, throwError } from 'rxjs';
import { environment } from '../environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly BASE_URL = environment.BASE_URL + '/User';

  constructor(private readonly http: HttpClient, private readonly authService: AuthService) { }

  getUsers(): Observable<any>{
    if(!this.authService.isAdmin){
      return throwError(() => new Error('Not authorized'));
    }
    return this.http.get(this.BASE_URL);
  }

  getProfile(userId: number) {
    return this.http.get(`${this.BASE_URL}/profile/${userId}`);
  }

  saveProfile(formData: FormData) {
    return this.http.post(`${this.BASE_URL}/profile/`, formData);
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import environment from '../environment';
import { AuthService } from './auth.service';
import { Observable, throwError } from 'rxjs';

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
}

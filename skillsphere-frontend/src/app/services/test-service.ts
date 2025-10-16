import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TestService {

  private readonly BASE_URL = 'https://localhost:7042/api/admin/tests';

  constructor(private readonly http: HttpClient) {}

  create(data: any): Observable<any> {
    return this.http.post(`${this.BASE_URL}`, data);
  }

  getList(): Observable<any>{
    return this.http.get(this.BASE_URL);
  }

  getTestById(id: any): Observable<any>{
    return this.http.get(`${this.BASE_URL}/${id}`);
  }

  getTestsCount(): Observable<any>{
    return this.http.get(`${this.BASE_URL}/count`);
  }
}

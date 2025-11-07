import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import environment from '../environment';

@Injectable({
  providedIn: 'root'
})
export class TestService {

  private readonly ADMIN_BASE_URL = environment.BASE_URL+'/admin/tests';
  private readonly LEARNER_BASE_URL = environment.BASE_URL+'/learner/test';

  constructor(private readonly http: HttpClient) {}

  create(data: any): Observable<any> {
    return this.http.post(`${this.ADMIN_BASE_URL}`, data);
  }

  submitTest(payload: any) {
  return this.http.post(`${this.LEARNER_BASE_URL}/submit-test`, payload);
}




  getAdminList(): Observable<any>{
    return this.http.get(this.ADMIN_BASE_URL);
  }

  getAvailableTests(): Observable<any>{
    return this.http.get(this.LEARNER_BASE_URL);
  }

  getTestById(id: any): Observable<any>{
    return this.http.get(`${this.ADMIN_BASE_URL}/${id}`);
  }

    getLearnerTestById(testId: number): Observable<any> {
    return this.http.get(`${this.LEARNER_BASE_URL}/${testId}`);
  }

  getTestsCount(): Observable<any>{
    return this.http.get(`${this.ADMIN_BASE_URL}/count`);
  }
}

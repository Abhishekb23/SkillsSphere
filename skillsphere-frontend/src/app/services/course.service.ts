import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import environment from '../environment';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private readonly ADMIN_BASE_URL = environment.BASE_URL + '/admin/course';
  private readonly LEARNER_BASE_URL = environment.BASE_URL + '/learner/course';

  constructor(private readonly http: HttpClient) { }

  create(data: any): Observable<any> {
    return this.http.post(`${this.ADMIN_BASE_URL}`, data);
  }

  submitCourse(payload: any) {
    return this.http.post(`${this.LEARNER_BASE_URL}/submit-test`, payload);
  }

  getCoursesForAdmin(): Observable<any> {
    return this.http.get(this.ADMIN_BASE_URL);
  }

  getCoursesForLearner(): Observable<any> {
    return this.http.get(this.LEARNER_BASE_URL);
  }

  getCourseById(id: any): Observable<any> {
    return this.http.get(`${this.ADMIN_BASE_URL}/${id}`);
  }

  getLearnerCourseById(testId: number): Observable<any> {
    return this.http.get(`${this.LEARNER_BASE_URL}/${testId}`);
  }

  getCoursesCount(): Observable<any> {
    return this.http.get(`${this.ADMIN_BASE_URL}/count`);
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import environment from '../environment';

@Injectable({
  providedIn: 'root',
})
export class TestService {
  private readonly ADMIN_BASE_URL = environment.BASE_URL + '/admin/tests';
  private readonly LEARNER_BASE_URL = environment.BASE_URL + '/learner/test';

  constructor(private readonly http: HttpClient) {}

  create(data: any): Observable<any> {
    return this.http.post(`${this.ADMIN_BASE_URL}`, data);
  }

  submitTest(payload: any) {
    return this.http.post(`${this.LEARNER_BASE_URL}/submit-test`, payload);
  }

  getTestsForAdmin(): Observable<any> {
    return this.http.get(this.ADMIN_BASE_URL);
  }

  getTestsForLearner(): Observable<any> {
    return this.http.get(this.LEARNER_BASE_URL);
  }

  getTestById(id: any): Observable<any> {
    return this.http.get(`${this.ADMIN_BASE_URL}/${id}`);
  }

  getLearnerTestById(testId: number): Observable<any> {
    return this.http.get(`${this.LEARNER_BASE_URL}/${testId}`);
  }

  getTestsCount(): Observable<any> {
    return this.http.get(`${this.ADMIN_BASE_URL}/count`);
  }

  uploadThumbnail(testId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.ADMIN_BASE_URL}/${testId}/thumbnail`, formData);
  }

  // ✅ Get thumbnail (convert blob → base64 string)
  private thumbnailCache = new Map<number, string>();

  getThumbnail(testId: number): Observable<string | null> {
    if (this.thumbnailCache.has(testId)) {
      return of(this.thumbnailCache.get(testId)!);
    }

    return new Observable((observer) => {
      this.http
        .get(`${this.ADMIN_BASE_URL}/${testId}/thumbnail`, { responseType: 'blob' })
        .subscribe({
          next: (blob) => {
            const reader = new FileReader();
            reader.onloadend = () => {
              const base64 = reader.result as string;
              this.thumbnailCache.set(testId, base64);
              observer.next(base64);
              observer.complete();
            };
            reader.readAsDataURL(blob);
          },
          error: () => {
            observer.next(null);
            observer.complete();
          },
        });
    });
  }

  updateTest(payload: any): Observable<any> {
    return this.http.put(`${this.ADMIN_BASE_URL}/${payload.testId}`, payload);
  }

  getUserResults(userId: number): Observable<any> {
  return this.http.get(`${this.LEARNER_BASE_URL}/results/${userId}`);
}

}

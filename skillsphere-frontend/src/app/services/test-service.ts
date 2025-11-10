import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
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


    uploadThumbnail(testId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.ADMIN_BASE_URL}/${testId}/thumbnail`, formData);
  }

  // ✅ Get thumbnail (convert blob → base64 string)
  getThumbnail(testId: number): Observable<string | null> {
  return new Observable((observer) => {
    this.http
      .get(`${this.ADMIN_BASE_URL}/${testId}/thumbnail`, { responseType: 'blob' })
      .subscribe({
        next: (blob) => {
          const reader = new FileReader();
          reader.onloadend = () => {
            const base64data = reader.result as string;
            observer.next(base64data);
            observer.complete();
          };
          reader.onerror = () => {
            observer.next(null);
            observer.complete();
          };
          reader.readAsDataURL(blob);
        },
        error: () => {
          observer.next(null);
          observer.complete();
        }
      });
  });
}

}

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
  getThumbnail(testId: number): Observable<string> {
    return this.http
      .get(`${this.ADMIN_BASE_URL}/${testId}/thumbnail`, { responseType: 'blob' })
      .pipe(
        map(blob => {
          const reader = new FileReader();
          return new Promise<string>((resolve) => {
            reader.onloadend = () => resolve(reader.result as string);
            reader.readAsDataURL(blob);
          });
        }),
        map(promise => {
          let result = '';
          promise.then(res => result = res);
          return result;
        })
      );
  }
}
